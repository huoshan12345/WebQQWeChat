using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using WebQQ.Im.Core;
using WebQQ.Im.Event;

namespace Application.Services
{
    public interface IQQService
    {
        IReadOnlyList<QQClientModel> GetQQList(string username);
        string LoginClient(string username);
        IReadOnlyList<QQNotifyEvent> GetAndClearEvents(string username, string id);
    }
}
