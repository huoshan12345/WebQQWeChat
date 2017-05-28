using System;
using WebQQ.Im.Core;

namespace Application.Models.QQModels
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
