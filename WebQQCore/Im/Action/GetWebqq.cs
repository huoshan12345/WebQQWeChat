using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im.Action
{
    public class GetWebqq : AbstractHttpAction
    {
        const string url = "http://wspeed.qq.com/w.cgi";

        /**
         * <p>Constructor for AbstractHttpAction.</p>
         *
         * @param context  a {@link IQQContext} object.
         * @param listener a {@link QQActionListener} object.
         */
        public GetWebqq(IQQContext context, QQActionEventHandler listener) : base(context, listener)
        {

        }

        public override QQHttpRequest BuildRequest()
        {
            var request = CreateHttpRequest("GET", url);
            request.AddGetValue("appid", "1000164");
            request.AddGetValue("touin", "null");
            request.AddGetValue("releaseversion", "SMARTQQ");
            request.AddGetValue("frequency", "1");
            request.AddGetValue("commandid", "http://s.web2.qq.com/api/getvfwebqq");
            request.AddGetValue("resultcode", "0");
            request.AddGetValue("tmcost", "160");
            return request;
        }

    }
}
