using System;
using HttpActionTools.Core;
using HttpActionTools.Event;

namespace HttpActionTools.Action
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
