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
    /// <para>个人签名</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetGroupFaceAction : AbstractHttpAction
    {

        private QQGroup group;

        public GetGroupFaceAction(QQContext context, QQActionEventHandler listener,
                QQGroup group)
            : base(context, listener)
        {

            this.group = group;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            QQSession session = Context.Session;
            QQHttpRequest req = CreateHttpRequest("GET",
                    QQConstants.URL_GET_USER_FACE);
            req.AddGetValue("uin", group.Code + "");
            req.AddGetValue("vfwebqq", session.Vfwebqq);
            req.AddGetValue("t", DateUtils.NowTimestamp() / 1000 + "");
            req.AddGetValue("cache", "0");
            req.AddGetValue("type", "4");
            req.AddGetValue("fid", "0");
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            MemoryStream ms = new MemoryStream(response.ResponseData); // 输入流
            Image image = null;
            try
            {
                image = Image.FromStream(ms);
                group.Face = image;
            }
            catch (IOException e)
            {
                throw new QQException(QQErrorCode.IO_ERROR, e);
            }
            NotifyActionEvent(QQActionEventType.EVT_OK, group);
        }
    }
}
