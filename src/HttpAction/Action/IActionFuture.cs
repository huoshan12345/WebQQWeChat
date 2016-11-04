namespace HttpAction.Action
{    /// <summary>
     /// 用于按顺序执行一些action，前一个action成功则继续执行，否则则退出
     /// </summary>
    public interface IActionFuture : IActor
    {
        /// <summary>
        /// 放入一个action到执行队列末尾
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IActionFuture PushAction(IAction action);
    }
}
