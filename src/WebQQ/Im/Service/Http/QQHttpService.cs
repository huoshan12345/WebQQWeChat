using HttpActionFrame.Core;
using WebQQ.Im.Core;

namespace WebQQ.Im.Service.Http
{
    public class QQHttpService: HttpService, IQQHttpService
    {
        public void Init(IQQContext context)
        {
        }

        public void Destroy()
        {
            Dispose();
        }
    }
}
