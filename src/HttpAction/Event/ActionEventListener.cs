using System.Threading.Tasks;
using HttpAction.Action;

namespace HttpAction.Event
{
    // 之所以设置成返回task的形式是因为可以在调用的时候await
    public delegate Task ActionEventListener(IAction sender, ActionEvent actionEvent);
}
