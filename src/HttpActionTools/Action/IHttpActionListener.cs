using System;
using HttpActionFrame.Core;
using HttpActionFrame.Event;

namespace HttpActionFrame.Action
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
