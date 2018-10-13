using Stand.UI.Infrastructure.EventArgs;
using System.Threading.Tasks;

namespace Stand.UI.Infrastructure.Events
{
    public delegate Task CommandEventHandler(object sender, CommandEventArgs e);
}
