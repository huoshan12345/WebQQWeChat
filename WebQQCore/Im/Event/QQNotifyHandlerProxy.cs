namespace iQQ.Net.WebQQCore.Im.Event
{
    /**
     *
     * 使用这个类简化事件的注册，分发
     * 只需在被代理的类使用@IMEventHandler注解需要处理的事件类型即可
     *
     * @author solosky
     */
    /*

    public class QQNotifyHandlerProxy : IQQNotifyListener{

        private Object proxyObject;
        private Dictionary<QQNotifyEventType, Method> methodMap;
        / **
         * <p>Constructor for QQNotifyHandlerProxy.</p>
         *
         * @param proxyObject a {@link java.lang.Object} object.
         * /
        public QQNotifyHandlerProxy(Object proxyObject){
            this.proxyObject = proxyObject;
            this.methodMap = new Dictionary<QQNotifyEventType, Method>();
             foreach (Method m in proxyObject.getClass().getDeclaredMethods()) {
                 if(m.isAnnotationPresent(QQNotifyHandler.class)){
                     QQNotifyHandler handler = m.getAnnotation(QQNotifyHandler.class);
                     this.methodMap.Add(handler.value(), m);
                     if(!m.isAccessible()){
                         m.setAccessible(true);
                     }
                 }
             }
        }
	
        / ** {@inheritDoc} * /
	
        public void OnNotifyEvent(QQNotifyEvent Event) {
            Method m =  methodMap[Event.Type];
            if(m != null){
                try {
                    m.Invoke(proxyObject, Event);
                } catch (Exception e) {
                    // LOG.warn("invoke QQNotifyHandler Error!!", e);
                }
            }else{
                // LOG.warn("Not found QQNotifyHandler for QQNotifyEvent = " + Event);
            }
        }

    }
    */
}

