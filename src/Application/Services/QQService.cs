using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using WebQQ.Im.Core;
using WebQQ.Im.Event;

namespace Application.Services
{
    public class QQService : IQQService
    {
        private static readonly QQManager _qqManager = new QQManager();
        
        public IReadOnlyList<QQClientModel> GetQQList(string username)
        {
            return _qqManager.GetQQList(username);
        }

        public string LoginClient(string username)
        {
            return _qqManager.LoginClient(username);
        }

        public IReadOnlyList<QQNotifyEvent> GetAndClearEvents(string username, string id)
        {
            return _qqManager.GetAndClearEvents(username, id);
        }
    }
}
