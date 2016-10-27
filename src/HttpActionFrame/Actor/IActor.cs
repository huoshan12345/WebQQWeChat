using System.Threading.Tasks;

namespace HttpActionFrame.Actor
{
    public interface IActor
    {
        Task ExecuteAsync();
    }
}
