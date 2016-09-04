using System;
using System.IO;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取群聊天图片</para>
    /// <para>@author solosky</para>
    /// </summary>
    public class GetGroupPicAction : AbstractHttpAction
    {
        private readonly CFaceItem _cface;
        private readonly QQMsg _msg;
        private readonly Stream _picOut;

        /**
         * <p>Constructor for GetGroupPicAction.</p>
         *
         * @param context a {@link iqq.im.core.IQQContext} object.
         * @param listener a {@link iqq.im.IQQActionListener} object.
         * @param cface a {@link iqq.im.bean.content.CFaceItem} object.
         * @param msg a {@link iqq.im.bean.QQMsg} object.
         * @param picOut a {@link java.io.OutputStream} object.
         */
        public GetGroupPicAction(IQQContext context, QQActionListener listener,
                                            CFaceItem cface, QQMsg msg, Stream picOut)
            : base(context, listener)
        {

            _cface = cface;
            _msg = msg;
            _picOut = picOut;
        }

        /* (non-Javadoc)
         * @see iqq.im.action.AbstractHttpAction#OnHttpStatusOK(iqq.im.http.QQHttpResponse)
         */
        /** {@inheritDoc} */

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            NotifyActionEvent(QQActionEventType.EvtOK, _cface);
        }

        /* (non-Javadoc)
         * @see iqq.im.action.AbstractHttpAction#OnBuildRequest()
         */
        /** {@inheritDoc} */

        public override QQHttpRequest OnBuildRequest()
        {
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_GROUP_PIC);

            //		fid	3648788200
            //		gid	2890126166
            //		pic	{F2B04C26-9087-437D-4FD9-6A0ED84155FD}.jpg
            //		rip	123.138.154.167
            //		rport	8000
            //		t	1365343106
            //		type	0
            //		uin	3559750777
            //		vfwebqq	70b5f77bfb1db1367a2ec483ece317ea9ef119b9b59e542b2e8586f7ede6030ff56f7ba8798ba34b
            //		"cface",
            //        {
            //            "name": "{F2B04C26-9087-437D-4FD9-6A0ED84155FD}.jpg",
            //            "file_id": 3648788200,
            //            "key": "pcm4N6IKmQ852Pus",
            //            "server": "123.138.154.167:8000"
            //        }

            var session = Context.Session;
            req.AddGetValue("fid", _cface.FileId);
            req.AddGetValue("gid", _msg.Group?.Code ?? _msg.Discuz.Did);
            req.AddGetValue("pic", _cface.FileName);
            var parts = _cface.Server.Split(':');
            req.AddGetValue("rip", parts[0]);
            req.AddGetValue("rport", parts[1]);
            req.AddGetValue("t", DateTime.Now.CurrentTimeSeconds());
            req.AddGetValue("type", _msg.Group != null ? "0" : "1");
            req.AddGetValue("uin", _msg.From.Uin);
            req.AddGetValue("vfwebqq", session.Vfwebqq);

            req.OutputStream = _picOut;
            return req;
        }



    }

}
