namespace iQQ.Net.WebQQCore.Im.Event.Future
{
    /**
     *
     * 多个过程操作的异步等待对象
     * 需手动处理
     *
     * @author solosky
     */
    public class ProcActionFuture : AbstractActionFuture
    {

        /**
         * <p>Constructor for ProcActionFuture.</p>
         *
         * @param proxyListener a {@link iqq.im.IQQActionListener} object.
         * @param Cancelable a bool.
         */
        public ProcActionFuture(QQActionEventHandler proxyListener, bool cancelable)
            : base(proxyListener)
        {
            IsCanceled = false;
        }

        public override bool IsCancelable()
        {
            return true;
        }

        public override void Cancel()
        {
            IsCanceled = true;
            NotifyActionEvent(QQActionEventType.EVT_CANCELED, null);
        }

    }

}
