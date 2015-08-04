using System.Threading.Tasks;

namespace iQQ.Net.WebQQCore.Im.Actor
{
    public enum QQActorType
    {
        SimpleActor,
        PollMsgActor,
        GetRobotReply,
    }


    public interface QQActor
    {
        void Execute();

        Task ExecuteAsync();

        QQActorType Type { get; }
    }
}
