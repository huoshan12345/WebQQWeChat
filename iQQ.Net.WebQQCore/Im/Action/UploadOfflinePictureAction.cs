using System.Text.RegularExpressions;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Im.Service;
using iQQ.Net.WebQQCore.Util;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>上传离线图片</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class UploadOfflinePictureAction : AbstractHttpAction
    {
        private string file;
        private QQUser user;

        public UploadOfflinePictureAction(QQContext context,
                QQActionEventHandler listener, QQUser user, string file)
            : base(context, listener)
        {
            this.user = user;
            this.file = file;
        }

        public override QQHttpRequest OnBuildRequest()
        {

            IHttpService httpService = Context.GetSerivce<IHttpService>(QQServiceType.HTTP);
            QQSession session = Context.Session;

            QQHttpRequest req = CreateHttpRequest("POST", QQConstants.URL_UPLOAD_OFFLINE_PICTURE);
            req.AddGetValue("time", DateUtils.NowTimestamp() / 1000 + "");
            req.AddPostFile("file", this.file);
            req.AddPostValue("callback", "parent.EQQ.Model.ChatMsg.callbackSendPic");
            req.AddPostValue("locallangid", "2052");
            req.AddPostValue("clientversion", "1409");
            req.AddPostValue("uin", Context.Account.Uin + ""); // 自己的账号
            req.AddPostValue("skey", httpService.GetCookie("skey", QQConstants.URL_UPLOAD_OFFLINE_PICTURE).Value);
            req.AddPostValue("appid", "1002101");
            req.AddPostValue("peeruin", user.Uin + ""); // 图片对方UIN
            req.AddPostValue("fileid", "1");
            req.AddPostValue("vfwebqq", session.Vfwebqq);
            req.AddPostValue("senderviplevel", Context.Account.Level.Level + "");
            req.AddPostValue("reciverviplevel", user.Level.Level + "");
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            Regex rex = new Regex(QQConstants.REGXP_JSON_SINGLE_RESULT);
            Match m = rex.Match(response.GetResponseString());

            OffPicItem pic = new OffPicItem();
            JObject obj = null;

            if (!m.Success)
            {
                pic.IsSuccess = false;
                NotifyActionEvent(QQActionEventType.EVT_ERROR,
                        new QQException(QQErrorCode.UNEXPECTED_RESPONSE, response.GetResponseString()));
            }

            string regResult = Regex.Replace(Regex.Replace(m.Groups[0].Value, "[\\r]?[\\n]", " "), "[\r]?[\n]", " ");
            obj = JObject.Parse(regResult);

            int retcode = obj["retcode"].ToObject<int>();
            if (retcode == 0)
            {
                pic.IsSuccess = (obj["progress"].ToObject<int>() == 100) ? true : false;
                pic.FileSize = obj["filesize"].ToObject<int>();
                pic.FileName = obj["filename"].ToString();
                pic.FilePath = obj["filepath"].ToString();
                NotifyActionEvent(QQActionEventType.EVT_OK, pic);
                return;
            }

            // 失败后返回路径
            pic.FilePath = file;
            pic.IsSuccess = false;
            NotifyActionEvent(QQActionEventType.EVT_ERROR,
                    new QQException(QQErrorCode.UNEXPECTED_RESPONSE, response.GetResponseString()));

        }

    }

}
