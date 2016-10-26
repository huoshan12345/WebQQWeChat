using System;

namespace HttpActionFrame.Event
{
    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// 当前进度
        /// </summary>
        public long Current { get;  }

        /// <summary>
        /// 总的进度
        /// </summary>
        public long Total { get;  }

        public ProgressEventArgs(long current, long total)
        {
            Current = current;
            Total = total;
        }
    }
}
