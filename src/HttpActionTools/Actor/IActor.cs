using System.Threading.Tasks;

namespace HttpActionFrame.Actor
{
    public interface IActor
    {
        void Execute();

        Task ExecuteAsync();
    }
}
