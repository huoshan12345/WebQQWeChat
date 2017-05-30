using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.QQModels;
using WebQQ.Im.Core;
using WebQQ.Im.Event;

namespace Application.Services
{
    public interface IQQService
    {
        IReadOnlyList<QQClientModel> GetQQList(string username);
        string LoginClient(string username);
        IReadOnlyList<QQNotifyEvent> Poll(string username, string id);
        Task<DataResult> SendMsg(string username, string qqId, MessageModel message);
    }
}
