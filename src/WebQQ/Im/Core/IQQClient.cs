using System;
using WebQQ.Im.Module.Interface;

namespace WebQQ.Im.Core
{
    public interface IQQClient : IDisposable, IQQContext, ILoginModule, IChatModule
    {
    }
}
