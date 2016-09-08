using HttpActionTools.Core;
using iQQ.Net.WebQQCore.Im.Core;

namespace iQQ.Net.WebQQCore.Im.Service.Http
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
