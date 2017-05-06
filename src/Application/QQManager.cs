using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using WebQQ.Im.Core;

namespace Application
{
    public class QQManager
    {
        private readonly ConcurrentDictionary<string, List<IQQClient>> _clients = new ConcurrentDictionary<string, List<IQQClient>>();

        public List<IQQClient> GetQQList(string username)
        {
            if(_clients.TryGetValue(username, out var list))
            {
                return list;
            }
            else return 
        }
    }
}
