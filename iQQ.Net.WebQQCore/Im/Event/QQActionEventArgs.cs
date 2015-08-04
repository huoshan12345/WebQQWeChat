namespace iQQ.Net.WebQQCore.Im.Event
{
    public abstract class QQActionEventArgs
    {
        public class ProgressArgs
        {
            /** 当前进度 */
            public long current;
            /** 总的进度 */
            public long total;

            public override string ToString()
            {
                return "ProgressArgs [current=" + current + ", total=" + total + "]";
            }
        }

        public class CheckVerifyArgs
        {
            public int result;
            public string code;
            public long uin;
        }
    }
}
