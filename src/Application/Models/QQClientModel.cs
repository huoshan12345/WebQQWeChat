using System;
using System.Collections.Generic;
using System.Text;
using WebQQ.Im.Core;

namespace Application.Models
{
    public class QQClientModel
    {
        public Guid Id { get; }

        public IQQClient Client { get; }

        public QQClientModel(IQQClient client)
        {
            Id = Guid.NewGuid();
            Client = client;
        }
    }
}
