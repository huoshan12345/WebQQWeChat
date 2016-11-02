using System;
using Utility.HttpAction;
using Utility.HttpAction.Action;
using Utility.HttpAction.Event;
using WebWeChat.Im.Core;
using WebWeChat.Im.Service.Interface;

namespace WebWeChat.Im.Action
{
    public class WeChatActionFuture : ActionFuture
    {
        protected IWeChatActionFactory ActionFactory { get; }

        public WeChatActionFuture(IWeChatContext context, ActionEventListener listener = null)
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
        public WeChatActionFuture PushAction<T>(params object[] args) where T : WeChatAction
        {
            var action = ActionFactory.CreateAction<T>(args);
            return (WeChatActionFuture)base.PushAction(action);
        }

        /// <summary>
        /// 只传一个listener的重载
        /// 当使用lambda表达式传递listener的时候会匹配到这个重载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listener"></param>
        /// <returns></returns>
        public WeChatActionFuture PushAction<T>(ActionEventListener listener) where T : WeChatAction
        {
            var action = ActionFactory.CreateAction<T>(listener);
            return (WeChatActionFuture)base.PushAction(action);
        }
    }
}
