using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iQQ.Net.BatchHangQQ.Extensions;
using iQQ.Net.BatchHangQQ.Util;
using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.BatchHangQQ
{
    public partial class FmQQList : Form
    {
        private readonly Dictionary<string, IQQClient> _qqClients = new Dictionary<string, IQQClient>();
        private QQNotifyListener _notifyListener;
        private readonly NotifyIcon _notifyIcon;// 创建NotifyIcon对象 
        private volatile bool _isLogining;
        private CancellationTokenSource _cts;

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
            _notifyListener = async (client, notifyEvent) =>
            {
                try
                {
                    switch (notifyEvent.Type)
                    {
                        case QQNotifyEventType.LoginSuccess:
                        {
                            client.Logger.LogInformation("登录成功");
                            break;
                        }
                        case QQNotifyEventType.GroupMsg:
                        {
                            var revMsg = (QQMsg)notifyEvent.Target;
                            if (revMsg.From.QQ.IsDefault()) await client.GetUserQQ(revMsg.From).WhenFinalEvent().ConfigureAwait(false);
                            client.Logger.LogInformation($"群[{revMsg.Group.Name}]-好友[{revMsg.From.Nickname}]：{revMsg.GetText()}");
                            break;
                        }
                        case QQNotifyEventType.ChatMsg:
                        {
                            var revMsg = (QQMsg)notifyEvent.Target;
                            if (revMsg.From.QQ.IsDefault()) await client.GetUserQQ(revMsg.From).WhenFinalEvent().ConfigureAwait(false);
                            client.Logger.LogInformation($"好友[{revMsg.From.Nickname}]：{revMsg.GetText()}");
                            if (chkUseRobot.Checked)
                            {
                                Thread.Sleep(3000);
                                var msgReply = new QQMsg()
                                {
                                    Type = QQMsgType.BUDDY_MSG,
                                    To = revMsg.From,
                                    From = client.Account,
                                    Date = DateTime.Now,
                                };
                                var replyEvent = await client.GetRobotReply(revMsg, RobotType.Tuling).WhenFinalEvent();
                                if (replyEvent.Type == QQActionEventType.EvtOK)
                                {
                                    var text = new TextItem((string)replyEvent.Target);
                                    msgReply.ContentList.Add(text);
                                    await client.SendMsg(msgReply).WhenFinalEvent().ConfigureAwait(false);
                                    client.Logger.LogInformation($"自动回复给[{revMsg.From.Nickname}]：{text.ToText()}");
                                }
                            }
                            break;
                        }
                        case QQNotifyEventType.QrcodeInvalid:
                        {
                            client.Logger.LogWarning("二维码已失效");
                            CancelLogin();
                            break;
                        }
                        case QQNotifyEventType.QrcodeReady:
                        {
                            client.Logger.LogWarning("二维码获取成功，请用手机qq扫码登录");
                            var verify = (Image)notifyEvent.Target;
                            pbQRCode.InvokeIfRequired(() =>
                            {
                                pbQRCode.Image = verify;
                            });
                            break;
                        }
                        case QQNotifyEventType.KickOffline:
                        {
                            client.Logger.LogInformation("被踢下线");
                            break;
                        }
                        case QQNotifyEventType.ShakeWindow:
                        {
                            var buddy = (QQBuddy)notifyEvent.Target;
                            if (buddy.QQ.IsDefault()) await client.GetUserQQ(buddy).WhenFinalEvent();
                            client.Logger.LogInformation($"[{buddy.ShowName}]发来抖动屏幕");
                            break;
                        }
                        case QQNotifyEventType.BuddyInput:
                        case QQNotifyEventType.BuddyStatusChange:
                        case QQNotifyEventType.QrcodeSuccess:
                        case QQNotifyEventType.NetError:
                        case QQNotifyEventType.UnknownError:
                        break;

                        default:
                        client.Logger.LogInformation(notifyEvent.Type + ", " + notifyEvent.Target);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    client.Logger.LogError(ex.ToString());
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


        private async void AddQQToLv(IQQClient qq)
        {
            var index = lvQQList.FindFirstItemIndex(qq.Account.QQ.ToString(), new[] { 1 });
            if (index < 0)
            {
                await qq.GetUserLevel(qq.Account).WhenFinalEvent();
                var subitems = new[]
                {
                    lvQQList.Items.Count.ToString(),
                    qq.Account.QQ.ToString(),
                    qq.Account.ClientType.ToString(),
                    qq.Account.Status.ToString(),
                    qq.Account.LevelInfo.Level.ToString(),
                    qq.Account.LevelInfo.RemainDays.ToString(),
                };
                var item = new ListViewItem(subitems) { Selected = true };
                lvQQList.Items.Add(item);
            }
        }

        private async void Login()
        {
            _cts = new CancellationTokenSource();
            btnLogin.InvokeIfRequired(() => btnLogin.Text = "取消登录");
            while (!_cts.IsCancellationRequested)
            {
                var client = new WebQQClient(notifyListener: _notifyListener, logger:new RichTextBoxLogger(tbMessage));
                var result = await client.LoginWithQRCode().WhenFinalEvent().ConfigureAwait(false);
                if (result.Type == QQActionEventType.EvtOK)
                {
                    var key = client.Account.QQ.ToString();
                    if (_qqClients.ContainsKey(key)) client.Destroy();
                    else
                    {
                        _qqClients.Add(key, client);
                        lvQQList.InvokeIfRequired(() =>
                        {
                            AddQQToLv(client);
                        });
                    }
                }

            }
        }

        private void CancelLogin()
        {
            _cts.Cancel();
            btnLogin.InvokeIfRequired(() => btnLogin.Text = "登录");
            _isLogining = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!_isLogining) Login();
            else CancelLogin();
        }

        private void fmQQList_FormClosing(object sender, FormClosingEventArgs e)
        {
            _notifyIcon.Visible = false;
        }

        private void btnClearQQlist_Click(object sender, EventArgs e)
        {
            foreach (var qq in _qqClients)
            {
                qq.Value.Destroy();
            }
            lvQQList.Items.Clear();
        }

        private void btnExportQQlist_Click(object sender, EventArgs e)
        {

        }
    }
}
