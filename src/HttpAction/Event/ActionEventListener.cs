using System.Threading.Tasks;
using HttpAction.Action;

namespace HttpAction.Event
{
    public delegate Task ActionEventListener(IAction sender, ActionEvent actionEvent);
}
