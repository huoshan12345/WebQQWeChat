using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using FclEx.Extensions;
using HttpAction;
using Microsoft.Extensions.Logging;
using WebQQ.Im;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;
using WebQQ.Im.Service.Impl;

namespace Application
{
    public class QQManager
    {
        private readonly ConcurrentDictionary<string, List<QQClientModel>> _clients = new ConcurrentDictionary<string, List<QQClientModel>>();
        private readonly ConcurrentDictionary<IQQClient, List<QQNotifyEvent>> _msgs = new ConcurrentDictionary<IQQClient, List<QQNotifyEvent>>();

        private readonly IReadOnlyList<QQClientModel> _emptyQQList = new List<QQClientModel>();
        private readonly IReadOnlyList<QQNotifyEvent> _emptyMsgList = new List<QQNotifyEvent>();

        public IReadOnlyList<QQClientModel> GetQQList(string username)
        {
            return _clients.TryGetValue(username, out var list) ? list : _emptyQQList;
        }

        public string LoginClient(string username)
        {
            var client = new WebQQClient(m => new QQConsoleLogger(m, LogLevel.Debug), (c, e) =>
            {
                var mList = _msgs.GetOrAdd(c, x => new List<QQNotifyEvent>());
                mList.Add(e);
                return Task.CompletedTask;
            });

            var list = _clients.GetOrAdd(username, m => new List<QQClientModel>());
            var model = new QQClientModel(client);
            list.Add(model);

            client.Login().ContinueWith(t =>
            {
                if (t.IsCompleted && t.Result.IsOk()) client.BeginPoll();
            });

            return model.Id.ToString();
        }

        public IReadOnlyList<QQNotifyEvent> GetAndClearEvents(string username, string id)
        {
            if (_clients.TryGetValue(username, out var list))
            {
                var client = list.FirstOrDefault(m => m.Id.ToString() == id)?.Client;
                if (client != null)
                {
                    if (_msgs.TryGetValue(client, out var msgList))
                    {
                        _msgs.Remove(client);
                        return msgList;
                    }
                }
            }
            return _emptyMsgList;
        }
    }
}
