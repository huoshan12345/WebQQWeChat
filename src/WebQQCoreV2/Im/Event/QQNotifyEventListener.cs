using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Im.Event
{
    public delegate void QQNotifyEventListener(IQQClient sender, QQNotifyEvent e);

    public interface IQQNotifyEventHandler
    {
        event QQNotifyEventListener QQNotifyEvent;
    }
}
