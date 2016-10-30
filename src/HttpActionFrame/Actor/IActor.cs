using System.Threading.Tasks;

namespace HttpActionFrame.Actor
{
    public interface IActor
    {
        Task ExecuteAsync();
    }

    public interface IActor<T>
    {
        Task<T> ExecuteAsync();
    }
}
