using HttpAction.Actions;
using HttpAction.Event;
using WebQQ.Im.Core;
using WebQQ.Im.Service.Interface;

namespace WebQQ.Im.Actions
{
    public class WebQQActionFuture : ActionFuture
    {
        protected IQQActionFactory ActionFactory { get; }

        public WebQQActionFuture(IQQContext context, ActionEventListener listener = null)
            : base(listener)
        {
            ActionFactory = context.GetSerivce<IQQActionFactory>();
        }

        /// <summary>
        /// 动态创建action，要求Action的构造函数第一个参数必须为Context
        /// <para />args指的是Action的构造函数的除了Context以外的参数按顺序排列
        /// <para />和PushAction(IAction action)的区别就是不需要new一个action并且给它传一个Context参数(这个参数每个QQAction都需要传)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public WebQQActionFuture PushAction<T>(params object[] args) where T : WebQQAction
        {
            var action = ActionFactory.CreateAction<T>(args);
            return (WebQQActionFuture)base.PushAction(action);
        }

        /// <summary>
        /// 只传一个listener的重载
        /// 当使用lambda表达式传递listener的时候会匹配到这个重载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listener"></param>
        /// <returns></returns>
        public WebQQActionFuture PushAction<T>(ActionEventListener listener) where T : WebQQAction
        {
            var action = ActionFactory.CreateAction<T>(listener);
            return (WebQQActionFuture)base.PushAction(action);
        }

        public WebQQActionFuture PushAction<T>(object obj, ActionEventListener listener) where T : WebQQAction
        {
            var action = ActionFactory.CreateAction<T>(obj, listener);
            return (WebQQActionFuture)base.PushAction(action);
        }
    }
}
