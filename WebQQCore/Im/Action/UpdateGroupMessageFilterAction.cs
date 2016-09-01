using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using Newtonsoft.Json.Linq;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>更新群消息筛选</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-4-15</para>
    /// </summary>
    public class UpdateGroupMessageFilterAction : AbstractHttpAction
    {
        public UpdateGroupMessageFilterAction(IQQContext context, QQActionEventHandler listener) : base(context, listener) { }

        public override QQHttpRequest OnBuildRequest()
        {
/*
            retype:1 app:EQQ
            itemlist:{"groupmask":{"321105219":"1","1638195794":"0","cAll":0,"idx":1075,"port":37883}}
            vfwebqq:8b26c442e239630f250e1e74d135fd85ab78c38e7b8da1c95a2d1d560bdebd2691443df19d87e70d
 */
            var store = Context.Store;
            var session = Context.Session;
            var req = CreateHttpRequest("POST", QQConstants.URL_GROUP_MESSAGE_FILTER);
            req.AddPostValue("retype", "1");	// 群？？？
            req.AddPostValue("app", "EQQ");

            var groupmask = new JObject();
            groupmask.Add("cAll", 0);
            groupmask.Add("idx", session.Index);
            groupmask.Add("port", session.Port);
            foreach (var g in store.GetGroupList())
            {
                if (g.Gin > 0)
                {
                    groupmask.Add(g.Gin.ToString(), g.Mask);
                }
            }
            var itemlist = new JObject();
            itemlist.Add("groupmask", groupmask);
            req.AddPostValue("itemlist", itemlist.ToString());
            req.AddPostValue("vfwebqq", Context.Session.Vfwebqq);
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            // {"result":null,"retcode":0}
            var json = JObject.Parse(response.GetResponseString());
            if (json["retcode"].ToString() == "0")
            {
                NotifyActionEvent(QQActionEventType.EvtOK, Context.Store.GetGroupList());
            }
            else
            {
                NotifyActionEvent(QQActionEventType.EvtError, QQErrorCode.UNEXPECTED_RESPONSE);
            }
        }

    }

}
