using iQQ.Net.WebQQCore.Im.Event;
using System;
using iQQ.Net.WebQQCore.Im.Http;

namespace iQQ.Net.WebQQCore.Im
{
    public delegate void QQActionListener(object sender, QQActionEvent e);

    public interface IQQActionListener
    {
        event QQActionListener OnQQActionEvent;
    }
}
