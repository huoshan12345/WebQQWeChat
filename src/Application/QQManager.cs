using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.QQModels;
using FclEx.Extensions;
using HttpAction;
using ImageSharp;
using Microsoft.Extensions.Logging;
using WebQQ.Im;
using WebQQ.Im.Bean.Friend;
using WebQQ.Im.Bean.Group;
using WebQQ.Im.Core;
using WebQQ.Im.Event;
using WebQQ.Im.Module.Impl;
using WebQQ.Im.Service.Impl;
using WebQQ.Util;

namespace Application
{
    public class QQManager
    {
        private readonly ConcurrentDictionary<string, List<QQClientModel>> _clients = new ConcurrentDictionary<string, List<QQClientModel>>();
        private readonly ConcurrentDictionary<IQQClient, BlockingCollection<QQNotifyEvent>> _msgs = new ConcurrentDictionary<IQQClient, BlockingCollection<QQNotifyEvent>>();

        private readonly IReadOnlyList<QQClientModel> _emptyQQList = new List<QQClientModel>();
        private readonly IReadOnlyList<QQNotifyEvent> _emptyMsgList = new List<QQNotifyEvent>();

        public IReadOnlyList<QQClientModel> GetQQList(string username)
        {
            return _clients.TryGetValue(username, out var list) ? list : _emptyQQList;
        }

        public string LoginClient(string username)
        {
            var list = _clients.GetOrAdd(username, m => new List<QQClientModel>());
            var model = list.FirstOrDefault(m => m.Client.IsOffline());
            if (model == null)
            {
                var client = new WebQQClient(m => new QQConsoleLogger(m, LogLevel.Debug), (c, e) =>
                {
                    var mList = _msgs.GetOrAdd(c, x => new BlockingCollection<QQNotifyEvent>(new ConcurrentQueue<QQNotifyEvent>()));
                    switch (e.Type)
                    {
                        case QQNotifyEventType.QRCodeReady:
                            mList.TryAdd(QQNotifyEvent.CreateEvent(e.Type, e.Target.CastTo<Image<Rgba32>>().ToRawBase64String()));
                            break;

                        case QQNotifyEventType.GroupMsg:
                            {
                                var msg = (GroupMessage)e.Target;
                                mList.TryAdd(QQNotifyEvent.CreateEvent(e.Type, $"[群消息][{msg.Group.ShowName}]{msg.GetText()}"));
                                break;
                            }
                        case QQNotifyEventType.ChatMsg:
                            {
                                var msg = (FriendMessage)e.Target;
                                mList.TryAdd(QQNotifyEvent.CreateEvent(e.Type, $"[好友消息][{msg.Friend.ShowName}]{msg.GetText()}"));
                                break;
                            }

                        default:
                            mList.TryAdd(e);
                            break;
                    }
                    return Task.CompletedTask;
                });
                model = new QQClientModel(client);
                list.Add(model);
            }
            else
            {
                _msgs[model.Client].Clear();
            }

            model.Client.Login().ContinueWith(t =>
            {
                if (t.IsCompleted && t.Result.IsOk()) model.Client.BeginPoll();
            });

            return model.Id.ToString();
        }

        public IReadOnlyList<QQNotifyEvent> GetAndClearEvents(string username, string id)
        {
            if (_clients.TryGetValue(username, out var qqList))
            {
                var client = qqList.FirstOrDefault(m => m.Id.ToString() == id)?.Client;
                if (client != null)
                {
                    if(_msgs.TryGetValue(client, out var col))
                    {
                        var list = new List<QQNotifyEvent>();
                        while (col.Count != 0 && col.TryTake(out var item, 1 * 1000))
                        {
                            list.Add(item);
                        }
                        if (list.Count == 0 && col.TryTake(out var temp, 20 * 1000))
                        {
                            list.Add(temp);
                        }
                        return list;
                    }
                    
                }
            }
            return _emptyMsgList;
        }
    }
}
