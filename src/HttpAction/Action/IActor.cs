using System.Threading;
using System.Threading.Tasks;
using HttpAction.Event;

namespace HttpAction.Action
{
    public interface IActor
    {
        Task<ActionEvent> ExecuteAsync(CancellationToken token);
    }
}
