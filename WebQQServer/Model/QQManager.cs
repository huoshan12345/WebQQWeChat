using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WebQQServer.Model
{
    public class QQManager
    {
        private readonly ConcurrentDictionary<string, HashSet<QQItem>> QQlist;

        public QQManager()
        {
            QQlist = new ConcurrentDictionary<string, HashSet<QQItem>>();
        }

        public bool Add(string account, QQItem item)
        {
            // return QQlist.TryAdd(account, item);

            return true;
        }
    }
}
