using System;
using HttpAction.Action;
using HttpAction.Event;
using WebWeChat.Im.Core;
using WebWeChat.Im.Service.Interface;
using HttpAction;

namespace WebWeChat.Im.Action
{
    public class WebWeChatActionFuture : ActionFuture
    {
        protected IWeChatActionFactory ActionFactory { get; }

        public WebWeChatActionFuture(IWeChatContext context, ActionEventListener listener = null)
            : base(listener)
        {
            ActionFactory = context.GetSerivce<IWeChatActionFactory>();
        }

        /// <summary>
        /// 动态创建action，要求Action的构造函数第一个参数必须为Context
        /// <para />args指的是Action的构造函数的除了Context以外的参数按顺序排列
        /// <para />和PushAction(IAction action)的区别就是不需要new一个action并且给它传一个Context参数(这个参数每个WeChatAction都需要传)
        /// <para />对，就是这么简单的原因，然后我就费了老大劲写了一个ActionFactory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public WebWeChatActionFuture PushAction<T>(params object[] args) where T : WebWeChatAction
        {
            var action = ActionFactory.CreateAction<T>(args);
            return (WebWeChatActionFuture)base.PushAction(action);
        }

        /// <summary>
        /// 只传一个listener的重载
        /// 当使用lambda表达式传递listener的时候会匹配到这个重载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listener"></param>
        /// <returns></returns>
        public WebWeChatActionFuture PushAction<T>(ActionEventListener listener) where T : WebWeChatAction
        {
            var action = ActionFactory.CreateAction<T>(listener);
            return (WebWeChatActionFuture)base.PushAction(action);
        }

        public WebWeChatActionFuture PushAction<T>(object obj, ActionEventListener listener) where T : WebWeChatAction
        {
            var action = ActionFactory.CreateAction<T>(obj, listener);
            return (WebWeChatActionFuture)base.PushAction(action);
        }
    }
}
