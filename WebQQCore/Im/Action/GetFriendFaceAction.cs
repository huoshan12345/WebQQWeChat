using System.Drawing;
using System.IO;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取用户头像</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetFriendFaceAction : AbstractHttpAction
    {

        private QQUser user;
        
        public GetFriendFaceAction(IQQContext context, QQActionEventHandler listener,QQUser user)
            : base(context, listener)
        {
            this.user = user;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;
            QQHttpRequest req = CreateHttpRequest("GET", QQConstants.URL_GET_USER_FACE);
            req.AddGetValue("uin", user.Uin + "");
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateUtils.NowTimestamp() / 1000 + "");
            req.AddGetValue("cache", 0 + ""); // ??
            req.AddGetValue("type", 1 + ""); // ??
            req.AddGetValue("fid", 0 + ""); // ??

            req.AddHeader("Referer", QQConstants.REFFER);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            MemoryStream ms = new MemoryStream(response.ResponseData); // 输入流
            Image image = null;
            try
            {
                image = Image.FromStream(ms);
                user.Face = image;
            }
            catch (IOException e)
            {
                throw new QQException(QQErrorCode.IO_ERROR, e);
            }
            NotifyActionEvent(QQActionEventType.EVT_OK, user);
        }
    }
}
