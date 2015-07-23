using System;
using System.Text.RegularExpressions;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Log;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>消息发送</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class UploadCustomFaceAction : AbstractHttpAction
    {

        private string file;

        public UploadCustomFaceAction(QQContext context, QQActionEventHandler listener, string file)
            : base(context, listener)
        {

            this.file = file;
        }

        public override QQHttpRequest OnBuildRequest()
        {

            QQSession session = Context.Session;

            QQHttpRequest req = CreateHttpRequest("POST", QQConstants.URL_UPLOAD_CUSTOM_FACE);
            req.AddGetValue("time", DateUtils.NowTimestamp() / 1000 + "");
            req.AddPostValue("from", "control");
            req.AddPostValue("f", "EQQ.Model.ChatMsg.callbackSendPicGroup");
            req.AddPostValue("vfwebqq", session.Vfwebqq);
            req.AddPostValue("fileid", Context.Store.GetPicItemListSize() + "");
            req.AddPostFile("custom_face", file);

            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            // {'ret':0,'msg':'5F7E31F0001EF4310865F1FF4549B12B.jPg'}
            Regex rex = new Regex(QQConstants.REGXP_JSON_SINGLE_RESULT);
            Match m = rex.Match(response.GetResponseString());
            CFaceItem pic = new CFaceItem();
            JObject obj = null;
            if (m.Success)
            {
                try
                {
                    string regResult = Regex.Replace(Regex.Replace(m.Groups[0].Value, "[\\r]?[\\n]", " "), "[\r]?[\n]", " ");
                    obj = JObject.Parse(regResult);

                    int retcode = obj["ret"].ToObject<int>();
                    if (retcode == 0)
                    {
                        pic.IsSuccess = true;
                        pic.FileName = obj["msg"].ToString();
                        NotifyActionEvent(QQActionEventType.EVT_OK, pic);

                        Context.Store.AddPicItem(pic);
                        return;
                    }
                    else if (retcode == 4)
                    {
                        // {'ret':4,'msg':'D81AB7A7627ED673FDCD4DD24220C192.jPg
                        // -6102 upload cface failed'}
                        string prefix = "\"msg\":\"";
                        string suffix = ".jPg";

                        rex = new Regex(prefix + "([\\s\\S]*)" + suffix, RegexOptions.IgnoreCase);
                        m = rex.Match(obj.ToString());

                        if (m.Success)
                        {
                            string r = Regex.Replace(m.Groups[0].Value, prefix, "");
                            //LOG.debug("ret 4: " + r);
                            pic.IsSuccess = true;
                            pic.FileName = r;
                            NotifyActionEvent(QQActionEventType.EVT_OK, pic);
                            return;
                        }
                    }
                    else
                    {
                        MyLogger.Default.Debug($"ret: {retcode}");
                    }
                }
                catch (Exception e)
                {
                    MyLogger.Default.Warn(e.Message, e);
                }
            }
            // 失败后返回路径
            pic.FileName = file;
            pic.IsSuccess = false;
            NotifyActionEvent(QQActionEventType.EVT_ERROR, new QQException(QQErrorCode.UNEXPECTED_RESPONSE, "CFace: " + response.GetResponseString()));
        }
    }
}
