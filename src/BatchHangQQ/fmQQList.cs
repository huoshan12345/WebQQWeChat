using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AngleSharp;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
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
        private readonly object _syncObj = new object();
        private readonly HttpCilentHelper _httpCilent = new HttpCilentHelper();
        private QQNotifyListener _notifyListener;
        private readonly NotifyIcon _notifyIcon; // 创建NotifyIcon对象 
        private CancellationTokenSource _cts;
        private readonly RichTextBoxLogger _logger;

        private async void GetIpInfo()
        {
            var result = await _httpCilent.GetAsync("http://ip.cn/", 3).ConfigureAwait(false);
            if (result.Success)
            {
                var document = await new HtmlParser().ParseAsync(result.ResponseString).ConfigureAwait(false);
                var items = document.QuerySelectorAll("#result .well p").ToList();
                var ip = items[0].QuerySelector("code").Text().Trim();
                var address = items[1].QuerySelector("code").Text().Trim();
                lbIp.InvokeIfRequired(() => lbIp.Text = ip);
                lbAddress.InvokeIfRequired(() => lbAddress.Text = address);
            }
            else
            {
                _logger.LogInformation("获取ip地址失败");
            }
        }

        protected override void WndProc(ref Message m)
        {   // 原来底层重绘每次会清除画布，然后再全部重新绘制，导致闪烁
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
        }

        public FmQQList()
        {
            SetStyle(ControlStyles.DoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint,
                true);
            UpdateStyles();

            InitializeComponent();
            chkUseRobot.Checked = false;
            _logger = new RichTextBoxLogger(tbMessage);

            _notifyIcon = new NotifyIcon() { Text = Text, Visible = true, Icon = Icon };

            _notifyIcon.DoubleClick += (sender, args) =>
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    Show();
                    WindowState = FormWindowState.Normal;
                    // this.ShowInTaskbar = true;
                }
                else
                {
                    WindowState = FormWindowState.Minimized;
                }
            };
            _notifyIcon.ContextMenu = new ContextMenu();
        }

        private void fmQQList_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                // this.ShowInTaskbar = false;
            }
        }

        private void fmQQList_Load(object sender, EventArgs e)
        {
            AfterInitialize();
            GetIpInfo();
        }

        private void AfterInitialize()
        {
            _notifyListener = GetListener();

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

        private QQNotifyListener GetListener()
        {
            return async (client, notifyEvent) =>
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
                                var replyEvent = await client.GetRobotReply(revMsg, RobotType.Turing).WhenFinalEvent();
                                if (replyEvent.Type == QQActionEventType.EvtOK)
                                {
                                    var str = (string)replyEvent.Target;
                                    var text = new TextItem($"{str} --来自机器人");
                                    msgReply.AddContentItem(text);
                                    msgReply.AddContentItem(new FontItem()); // 使用默认字体，不加这句对方收不到信息
                                    var result = await client.SendMsg(msgReply).WhenFinalEvent().ConfigureAwait(false);
                                    if (result.Type == QQActionEventType.EvtOK)
                                    {
                                        client.Logger.LogInformation($"自动回复给[{revMsg.From.Nickname}]：{text.ToText()}");
                                    }
                                    else
                                    {
                                        client.Logger.LogWarning($"自动回复给[{revMsg.From.Nickname}]发送失败");
                                    }
                                }
                                else
                                {
                                    client.Logger.LogWarning("获取机器人回复失败");
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
                            if (buddy.QQ.IsDefault()) await client.GetUserQQ(buddy).WhenFinalEvent().ConfigureAwait(false);
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
        }

        private void RemoveSelectedQQFromList()
        {
            if (lvQQList.SelectedIndices.Count <= 0) return;

            foreach (int index in lvQQList.SelectedIndices)
            {
                var qqNum = lvQQList.Items[index].SubItems[1].Text;
                try
                {
                    _qqClients[qqNum].Destroy();
                }
                catch
                {
                    _logger.LogError($"QQ[{qqNum}]关闭失败");
                }
                if (_qqClients.ContainsKey(qqNum)) _qqClients.Remove(qqNum);
                lvQQList.Items.RemoveAt(index);
            }
        }

        private async void AddQQToLv(IQQClient qq)
        {
            await qq.GetUserLevel(qq.Account).WhenFinalEvent().ConfigureAwait(false);
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

            var index = lvQQList.FindFirstItemIndex(qq.Account.QQ.ToString(), new[] { 1 });
            if (index < 0)
            {
                lvQQList.InvokeIfRequired(() =>
                {
                    lvQQList.Items.Add(item);
                });
            }
            else
            {
                lvQQList.InvokeIfRequired(() =>
                {
                    lvQQList.UpdateItem(index, item, new[] { 2, 3, 4, 5 });
                });
            }
        }

        private async void Login()
        {
            _cts = new CancellationTokenSource();
            btnLogin.InvokeIfRequired(() => btnLogin.Text = "取消登录");
            while (!_cts.IsCancellationRequested)
            {
                var client = new WebQQClient(notifyListener: _notifyListener, logger: new RichTextBoxLogger(tbMessage));
                var result = await client.LoginWithQRCode().WhenFinalEvent(_cts.Token).ConfigureAwait(false);
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
            _cts = null;
        }

        private void CancelLogin()
        {
            _cts.Cancel();
            btnLogin.InvokeIfRequired(() => btnLogin.Text = "登录");
            pbQRCode.InvokeIfRequired(() => pbQRCode.Image = null);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (_cts == null) Login();
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
