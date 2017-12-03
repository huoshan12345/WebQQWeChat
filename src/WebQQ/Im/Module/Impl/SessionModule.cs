using WebQQ.Im.Bean;
using WebQQ.Im.Core;

namespace WebQQ.Im.Module.Impl
{
    public enum SessionState
    {
        Offline,
        Online,
        Logining,
    }

    /// <summary>
    /// QQSession保存了每次登陆时候的状态信息
    /// </summary>
    public class SessionModule : QQModule
    {
        private volatile SessionState _state; // 作为指令关键字，确保本条指令不会因编译器的优化而省略，且要求每次直接读值.
        public SessionState State
        {
            get => _state;
            set => _state = value;
        }
        
        public QQUser User { get; set; } = new QQUser();

        public long ClientId { get; set; } = 53999199;
        public string SessionId { get; set; } = "";
        public string Vfwebqq { get; set; } = "";
        public string Ptwebqq { get; set; } = "";

        //public string LoginSig { get; set; }
        //public string CfaceKey { get; set; }
        //public string CfaceSig { get; set; }
        //public string EmailAuthKey { get; set; }
        public int Index { get; set; }
        public int Port { get; set; }
        //public int PollErrorCnt { get; set; }
        //public string CapCd { get; set; }
        //public string Psessionid { get; set; }



        public string CheckSigUrl { get; set; }

        public SessionModule(IQQContext context) : base(context)
        {
        }
    }

}
