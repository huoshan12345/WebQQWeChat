using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpActionTools.Action;
using HttpActionTools.Event;

namespace HttpActionTools.Core
{
    public interface IHttpActionListener
    {
        void OnHttpHeader(HttpResponseItem responseItem);

        void OnHttpContent(HttpResponseItem responseItem);

        void OnHttpRead(ProgressEventArgs args);

        void OnHttpWrite(ProgressEventArgs args);

        void OnHttpError(Exception ex);



        //event EventHandler<Exception> OnHttpError;

        //event EventHandler<ProgressEventArgs> OnHttpRead;

        //event EventHandler<ProgressEventArgs> OnHttpWrite;
    }
}
