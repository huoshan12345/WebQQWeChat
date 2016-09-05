using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;

namespace iQQ.Net.BatchHangQQ
{
    public partial class FmQQList : Form
    {
        private readonly AutoResetEvent _verifyCodeInputed = new AutoResetEvent(false);
        private readonly Dictionary<string, IQQClient> _qqClients = new Dictionary<string, IQQClient>();
        private QQNotifyListener _notifyListener;
        private QQActionListener _eventHandler;
        private static readonly string _rexQQNumPwd = @"^\d{5,}----.+$";
        private readonly NotifyIcon _notifyIcon;// 创建NotifyIcon对象 

        protected override void WndProc(ref Message m)
        {   // 原来底层重绘每次会清除画布，然后再全部重新绘制，导致闪烁
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
        }

        public FmQQList()
        {
            this.SetStyle(ControlStyles.DoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint,
                true);
            this.UpdateStyles();

            InitializeComponent();
            tbVerifyCode.Enabled = false;
            cboLoginStatus.Items.AddRange(_loginStatus[0].Cast<object>().ToArray());
            cboLoginProtocol.Items.AddRange(_loginProtocol.Cast<object>().ToArray());
            cboVerifyCodeDigit.Items.AddRange(_verifyCodeDigit.Cast<object>().ToArray());
            cboLoginStatus.SelectedIndex = 0;
            cboLoginProtocol.SelectedIndex = 0;
            cboVerifyCodeDigit.SelectedIndex = 0;
            chkAutoReply.Checked = false;
            chkUseRobot.Checked = false;

            _notifyIcon = new NotifyIcon() { Text = this.Text, Visible = true, Icon = Icon };

            _notifyIcon.DoubleClick += (sender, args) =>
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    // this.ShowInTaskbar = true;
                }
                else
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            };
            _notifyIcon.ContextMenu = new ContextMenu();
        }

        private void fmQQList_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                // this.ShowInTaskbar = false;
            }
        }

        private void fmQQList_Load(object sender, EventArgs e)
        {
            AfterInitialize();
        }

        private void AfterInitialize()
        {
            _eventHandler = (sender, Event) =>
            {
                switch (Event.Type)
                {
                    case QQActionEventType.EvtCanceled:
                    case QQActionEventType.EvtError:
                    ShowMessage(Event.ToString());
                    break;
                }
            };

            _notifyListener = (client, notifyEvent) =>
            {
                try
                {
                    switch (notifyEvent.Type)
                    {
                        case QQNotifyEventType.BuddyInput:
                        case QQNotifyEventType.BuddyStatusChange:
                        {
                            break;
                        }

                        case QQNotifyEventType.ChatMsg:
                        {
                            var msg = (QQMsg)notifyEvent.Target;
                            switch (msg.Type)
                            {
                                case QQMsgType.SESSION_MSG:
                                case QQMsgType.BUDDY_MSG:
                                {
                                    if (msg.From.QQ == default(long))
                                    {
                                        client.GetUserInfo(msg.From);
                                    }
                                    ShowMessage($"{client.Account.QQ}：【{msg.From.Nickname}】消息：{msg.GetText()}");

                                    if (chkAutoReply.Checked)
                                    {
                                        var msgReply = new QQMsg()
                                        {
                                            Type = QQMsgType.BUDDY_MSG,
                                            To = msg.From,
                                            From = client.Account,
                                            Date = DateTime.Now,
                                        };
                                        TextItem text = null;

                                        if (chkUseRobot.Checked)
                                        {
                                            var replyEvent = client.GetRobotReply(msg, RobotType.Tuling).WaitFinalEvent(10000);
                                            text = replyEvent.Type == QQActionEventType.EvtOK ? new TextItem(replyEvent.Target as string)
                                            : new TextItem("这是自动回复");
                                        }
                                        else
                                        {
                                            text = new TextItem("这是自动回复");
                                        }
                                        Thread.Sleep(3000);
                                        msgReply.ContentList.Add(text);
                                        client.SendMsg(msgReply);
                                        ShowMessage($"{client.Account.QQ}：自动回复给【{msg.From.Nickname}】：{text.ToText()}");
                                    }
                                    break;
                                }

                                case QQMsgType.DISCUZ_MSG:
                                {
                                    ShowMessage($"{client.Account.QQ}：讨论组【{msg.Discuz}】来自【{msg.From.Nickname}】消息：{msg.GetText()}");
                                    break;
                                }

                                case QQMsgType.GROUP_MSG:
                                {
                                    ShowMessage($"{client.Account.QQ}：群【{msg.Group}】来自【{msg.From.Nickname}】消息：{msg.GetText()}");
                                    break;
                                }
                            }
                            break;
                        }

                        case QQNotifyEventType.KickOffline:
                        ShowMessage(client.Account.QQ + "：被踢下线-" + (String)notifyEvent.Target);
                        break;

                        case QQNotifyEventType.CapachaVerify:
                        {
                            var verify = (ImageVerify)notifyEvent.Target;
                            this.Invoke(new MethodInvoker(() =>
                            {
                                pbVerifyPic.Image = verify.Image;
                                tbVerifyCode.Enabled = true;
                                tbVerifyCode.Text = "";
                                tbVerifyCode.Focus();
                            }));

                            ShowMessage(verify.Reason);
                            ShowMessage(client.Account.Username + "：请输入验证码：");
                            _verifyCodeInputed.WaitOne();
                            client.SubmitVerify(tbVerifyCode.Text, notifyEvent);
                            tbVerifyCode.Invoke(new MethodInvoker(() =>
                            {
                                tbVerifyCode.Enabled = false;
                            }));
                            break;
                        }

                        case QQNotifyEventType.ShakeWindow:
                        {
                            var buddy = notifyEvent.Target as QQBuddy;
                            if (buddy?.QQ == default(long))
                            {
                                client.GetUserQQ(buddy, null);
                            }
                            ShowMessage($"{client.Account.QQ}：【{buddy?.ShowName}】发来抖动屏幕");
                            if (chkAutoReply.Checked)
                            {
                                client.SendShake(buddy);
                            }
                            break;
                        }

                        case QQNotifyEventType.NetError:
                        case QQNotifyEventType.UnknownError:
                        ShowMessage(client.Account.QQ + "：出错-" + notifyEvent.Target.ToString());
                        break;

                        default:
                        ShowMessage(client.Account.QQ + "：" + notifyEvent.Type + ", " + notifyEvent.Target);
                        break;
                    }
                    UpdateQQInfo(client);
                }
                catch (QQException e)
                {
                    ShowMessage(client.Account.QQ + "：" + e.StackTrace);
                }
                catch (Exception e)
                {
                    ShowMessage(e.StackTrace);
                }
            };

            var rightButtonCms = new ContextMenuStrip();

            var tsmiRemove = new ToolStripMenuItem() { Text = "移除" };
            tsmiRemove.Click += (sender, e) =>
            {
                var tsmi = sender as ToolStripMenuItem;
                var cms = tsmi?.Owner as ContextMenuStrip;
                if (cms != null)
                {
                    var lv = cms.SourceControl as ListView;
                    if (lv == lvQQList)
                    {
                        RemoveSelectedQQFromList();
                    }
                }
            };
            rightButtonCms.Items.Add(tsmiRemove);
            lvQQList.ContextMenuStrip = rightButtonCms;
        }

        public void ShowMessage(string msg)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");
            var text = $"[{time}] {msg}{Environment.NewLine}";
            if (tbMessage.InvokeRequired)
            {
                tbMessage.Invoke(new MethodInvoker(() =>
                {
                    tbMessage.AppendText(text);
                    tbMessage.SelectionStart = tbMessage.Text.Length;
                    tbMessage.ScrollToCaret();
                }));
            }
            else
            {
                tbMessage.AppendText(text);
                tbMessage.SelectionStart = tbMessage.Text.Length;
                tbMessage.ScrollToCaret();
            }
        }

        private static readonly string[][] _loginStatus =
        {
            new []{"在线", "隐身", "离开", "隐身", "忙碌", "Q我吧", "请勿打扰"},
            new []{"online", "hidden", "away","hidden","busy","callme", "silent"}
        };

        private static readonly string[] _loginProtocol = { "WebQQ" };
        private static readonly string[] _verifyCodeDigit = { "4", "5" };

        private void btnAddQQ_Click(object sender, EventArgs e)
        {
            var addQQForm = new FmAddQQ();
            addQQForm._addQQ = AddQQToList;
            addQQForm.ShowDialog();
        }

        private void AddQQToList(string qqNum, string qqPassword, string type = "webqq")
        {
            IQQClient iqqClient = null;
            switch (type.ToLower())
            {
                case "webqq":
                iqqClient = new WebQQClient(qqNum, qqPassword, _notifyListener, _threadActorDispatcher);
                break;

                default:
                iqqClient = new WebQQClient(qqNum, qqPassword, _notifyListener, _threadActorDispatcher);
                break;
            }
            _qqClients[qqNum] = iqqClient;
            AddQQInfo(iqqClient);
        }

        private void RemoveSelectedQQFromList()
        {
            if (lvQQList.SelectedIndices.Count <= 0) return;

            foreach (int index in lvQQList.SelectedIndices)
            {
                var qqNum = lvQQList.Items[index].SubItems[1].Text;
                try
                {
                    _qqClients[qqNum].Logout();
                }
                catch { }
                if (_qqClients.ContainsKey(qqNum)) _qqClients.Remove(qqNum);
                lvQQList.Items.RemoveAt(index);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            // Task.Factory.StartNew(Login);
            await LoginAsync();
        }

        private void Login()
        {
            var loginStatusIndex = 0;
            cboLoginStatus.Invoke(new MethodInvoker(() =>
            {
                loginStatusIndex = cboLoginStatus.SelectedIndex;
            }));
            var status = QQStatus.ValueOfRaw(_loginStatus[1][loginStatusIndex]);

            var qqCount = 0;
            lvQQList.Invoke(new MethodInvoker(() =>
            {
                qqCount = lvQQList.Items.Count;
            }));
            for (var i = 0; i < qqCount; ++i)
            {
                var qqNumber = "";
                var index = i;
                lvQQList.Invoke(new MethodInvoker(() =>
                {
                    qqNumber = lvQQList.Items[index].SubItems[1].Text;
                    lvQQList.SelectedIndices.Clear();
                    lvQQList.Focus();
                    lvQQList.Items[index].Selected = true;
                    lvQQList.Items[index].EnsureVisible();
                    lvQQList.Items[index].SubItems[4].Text = "正在登录";
                }));

                var client = _qqClients[qqNumber];
                if (QQStatus.IsGeneralOnline(client.Account.Status))
                {
                    if (client.Account.Status != status)
                    {
                        client.ChangeStatus(status).WaitFinalEvent(1000);
                    }
                    UpdateQQInfo(client, i);
                    continue;
                }
                //测试同步模式登录
                var future = client.Login(status, _eventHandler);
                ShowMessage(client.Account.Username + "-登录中......");
                var Event = future.WaitFinalEvent();
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    ShowMessage(client.Account.Username + "-登录成功！！！");
                    client.GetUserInfo(client.Account, _eventHandler);
                    client.GetUserLevel(client.Account, _eventHandler).WaitFinalEvent();
                    UpdateQQInfo(client, i);

                    if (qqCount < 5)
                    {
                        if (QQActionEventType.EvtOK == client.GetBuddyList(_eventHandler).WaitFinalEvent().Type)
                        {
                            ShowMessage($"{client.Account.Username}-好友数量：{client.GetBuddyList().Count}");
                        }
                        else
                        {
                            ShowMessage(string.Format(client.Account.Username + "-获取好友列表失败"));
                        }

                        if (QQActionEventType.EvtOK == client.GetGroupList(_eventHandler).WaitFinalEvent().Type)
                        {
                            ShowMessage($"{client.Account.Username}-群数量：{client.GetGroupList().Count}");
                        }
                        else
                        {
                            ShowMessage(string.Format(client.Account.Username + "-获取群列表失败"));
                        }
                    }

                    client.BeginPollMsg();
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    client.Account.Status = QQStatus.OFFLINE;
                    var ex = (QQException)Event.Target;
                    ShowMessage(ex.Message);
                    UpdateQQInfo(client);
                }
                else
                {
                    client.Account.Status = QQStatus.OFFLINE;
                    ShowMessage(client.Account.Username + "-登录失败");
                    UpdateQQInfo(client);
                }
            }
            lvQQList.Invoke(new MethodInvoker(() =>
            {
                lvQQList.SelectedIndices.Clear();
            }));
        }

        private async Task LoginAsync()
        {
            var loginStatusIndex = 0;
            cboLoginStatus.Invoke(new MethodInvoker(() =>
            {
                loginStatusIndex = cboLoginStatus.SelectedIndex;
            }));
            var status = QQStatus.ValueOfRaw(_loginStatus[1][loginStatusIndex]);

            var qqCount = 0;
            lvQQList.Invoke(new MethodInvoker(() =>
            {
                qqCount = lvQQList.Items.Count;
            }));
            for (var i = 0; i < qqCount; ++i)
            {
                var qqNumber = "";
                var index = i;
                lvQQList.Invoke(new MethodInvoker(() =>
                {
                    qqNumber = lvQQList.Items[index].SubItems[1].Text;
                    lvQQList.SelectedIndices.Clear();
                    lvQQList.Focus();
                    lvQQList.Items[index].Selected = true;
                    lvQQList.Items[index].EnsureVisible();
                    lvQQList.Items[index].SubItems[4].Text = "正在登录";
                }));

                var client = _qqClients[qqNumber];
                if (QQStatus.IsGeneralOnline(client.Account.Status))
                {
                    if (client.Account.Status != status)
                    {
                        client.ChangeStatus(status).WaitFinalEvent(1000);
                    }
                    UpdateQQInfo(client, i);
                    continue;
                }
                //测试同步模式登录
                var future = client.Login(status, _eventHandler);
                ShowMessage(client.Account.Username + "-登录中......");

                var Event = await future.WhenFinalEvent();

                if (Event.Type == QQActionEventType.EvtOK)
                {
                    ShowMessage(client.Account.Username + "-登录成功！！！");
                    await client.GetUserInfo(client.Account, _eventHandler).WhenFinalEvent();
                    await client.GetUserLevel(client.Account, _eventHandler).WhenFinalEvent();
                    UpdateQQInfo(client, i);

                    if (qqCount < 5)
                    {
                        var actionEvent = await client.GetBuddyList(_eventHandler).WhenFinalEvent();
                        ShowMessage(QQActionEventType.EvtOK == actionEvent.Type
                            ? $"{client.Account.Username}-好友数量：{client.GetBuddyList().Count}"
                            : string.Format(client.Account.Username + "-获取好友列表失败"));

                        actionEvent = await client.GetGroupList(_eventHandler).WhenFinalEvent();
                        ShowMessage(QQActionEventType.EvtOK == actionEvent.Type
                            ? $"{client.Account.Username}-群数量：{client.GetGroupList().Count}"
                            : string.Format(client.Account.Username + "-获取群列表失败"));
                    }

                    client.BeginPollMsg();
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    client.Account.Status = QQStatus.OFFLINE;
                    var ex = (QQException)Event.Target;
                    ShowMessage(ex.Message);
                    UpdateQQInfo(client);
                }
                else
                {
                    client.Account.Status = QQStatus.OFFLINE;
                    ShowMessage(client.Account.Username + "-登录失败");
                    UpdateQQInfo(client);
                }
            }
            lvQQList.Invoke(new MethodInvoker(() =>
            {
                lvQQList.SelectedIndices.Clear();
            }));
        }

        private void AddQQInfo(IQQClient iqqClient)
        {
            var index = GetIndexInLv(iqqClient.Account.Username);
            if (index >= 0)
            {
                UpdateQQInfo(iqqClient, index);
                return;
            }
            var listViewItem = new ListViewItem((lvQQList.Items.Count + 1).ToString());
            listViewItem.SubItems.Add(iqqClient.Account.Username);
            listViewItem.SubItems.Add(iqqClient.Account.Password);
            listViewItem.SubItems.Add(iqqClient.ClientType.Value);
            listViewItem.SubItems.Add("离线");
            listViewItem.SubItems.Add("0");
            listViewItem.SubItems.Add("0");
            if (lvQQList.InvokeRequired)
            {
                lvQQList.Invoke(new MethodInvoker(() =>
                {
                    lvQQList.Items.Add(listViewItem);
                }));
            }
            else
            {
                lvQQList.Items.Add(listViewItem);
            }
        }

        private void UpdateQQInfo(IQQClient iqqClient, int indexInLv = -1)
        {
            var qqAccount = iqqClient.Account;
            var index = indexInLv >= 0 ? indexInLv : GetIndexInLv(qqAccount.Username);
            if (index < 0) return;

            if (lvQQList.InvokeRequired)
            {
                lvQQList.Invoke(new MethodInvoker(() =>
                {
                    var item = lvQQList.Items[index];
                    item.SubItems[2].Text = iqqClient.Account.Password;
                    item.SubItems[3].Text = iqqClient.ClientType.Value;
                    item.SubItems[4].Text = qqAccount.Status.Description;
                    item.SubItems[5].Text = qqAccount.Level.Level.ToString();
                    item.SubItems[6].Text = qqAccount.Level.RemainDays.ToString();
                }));
            }
            else
            {
                var item = lvQQList.Items[index];
                item.SubItems[2].Text = iqqClient.Account.Password;
                item.SubItems[3].Text = iqqClient.ClientType.Value;
                item.SubItems[4].Text = qqAccount.Status.Description;
                item.SubItems[5].Text = qqAccount.Level.Level.ToString();
                item.SubItems[6].Text = qqAccount.Level.RemainDays.ToString();
            }
        }

        private void SelectQqlvItem(string qqNumber = "", int indexInLv = -1)
        {
            var index = indexInLv > 0 ? indexInLv : GetIndexInLv(qqNumber);
            if (index < 0)
            {
                return;
            }
            if (lvQQList.InvokeRequired)
            {
                lvQQList.Invoke(new MethodInvoker(() =>
                {
                    lvQQList.Focus();
                    lvQQList.SelectedIndices.Clear();
                    lvQQList.Items[index].Selected = true;
                    lvQQList.Items[index].SubItems[4].Text = "正在登录";
                }));
            }
            else
            {
                lvQQList.Focus();
                lvQQList.SelectedIndices.Clear();
                lvQQList.Items[index].Selected = true;
                lvQQList.Items[index].SubItems[4].Text = "正在登录";
            }
        }

        private int GetIndexInLv(string qqNumber)
        {
            var index = -1;
            lvQQList.Invoke(new MethodInvoker(() =>
            {
                foreach (ListViewItem item in lvQQList.Items)
                {
                    if (item.SubItems[1].Text == qqNumber)
                    {
                        index = item.Index;
                    }
                }
            }));
            return index;
        }

        private void tbVerifyCode_TextChanged(object sender, EventArgs e)
        {
            var verifyCodeDigit = Int32.Parse(_verifyCodeDigit[cboVerifyCodeDigit.SelectedIndex]);
            if (tbVerifyCode.Text.Length >= verifyCodeDigit)
            {
                _verifyCodeInputed.Set();                      // 当输入的文本为5位，发一个通知
            }
        }

        private void btnClearQQlist_Click(object sender, EventArgs e)
        {
            _qqClients.Clear();
            lvQQList.Items.Clear();
        }

        private void btnImportQQlist_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"Text Files|*.txt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FileName = string.Empty;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var fileName = openFileDialog.FileName;
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);//读取文件设定
            var sr = new StreamReader(fs);//设定读写的编码
            var strLine = "";
            var rex = new Regex(_rexQQNumPwd);
            while ((strLine = sr.ReadLine()) != null)
            {
                var m = rex.Match(strLine);
                if (m.Success)
                {
                    var qq = m.Groups[0].Value.Split(new string[] { "----" }, StringSplitOptions.RemoveEmptyEntries);
                    AddQQToList(qq[0], qq[1]);
                }
            }
            sr.Close();
            fs.Close();
        }

        private void btnExportQQlist_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = @"Text Files|*.txt";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = false;
            dialog.FileName = string.Empty;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var fileName = dialog.FileName;
            var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);//读取文件设定
            var sw = new StreamWriter(fs);

            foreach (var qq in _qqClients.Values)
            {
                sw.WriteLine("{0}----{1}", qq.Account.Username, qq.Account.Password);
            }

            sw.Close();
            fs.Close();
        }

        private void chkUseRobot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseRobot.Checked)
            {
                chkAutoReply.Checked = true;
            }
        }

        private void lvQQList_DoubleClick(object sender, EventArgs e)
        {
            if (lvQQList.SelectedIndices.Count > 0)
            {
                var lvi = lvQQList.Items[lvQQList.SelectedIndices[0]];
                var addQQForm = new FmAddQQ(lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text);
                addQQForm._addQQ = AddQQToList;
                addQQForm.ShowDialog();
            }
        }

        private void fmQQList_FormClosing(object sender, FormClosingEventArgs e)
        {
            _notifyIcon.Visible = false;
        }
    }
}
