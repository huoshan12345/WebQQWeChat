using System;

namespace iQQ.Net.WebQQCore.Im.Http
{
    public interface IQQHttpListener
    {
        void OnHttpFinish(QQHttpResponse response);

        void OnHttpError(Exception t);

        void OnHttpHeader(QQHttpResponse response);

        void OnHttpWrite(long current, long total);

        void OnHttpRead(long current, long total);
    }

}
