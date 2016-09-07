using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpActionTools.Action
{
    public interface IActionListener
    {
        void OnSuccess();

        void OnFinish();

        void OnCancel();

        void OnError(Exception ex);
    }
}
