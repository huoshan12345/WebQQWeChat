using System;
using WebQQ.Im.Modules.Interface;

namespace WebQQ.Im.Core
{
    public interface IQQClient : IDisposable, IQQContext, ILoginModule, IChatModule
    {
    }
}
