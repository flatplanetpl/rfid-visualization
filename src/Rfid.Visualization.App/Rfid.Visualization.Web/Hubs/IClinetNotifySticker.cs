using System.Threading.Tasks;

namespace Rfid.Visualization.Web.Hubs
{
    public interface IClinetNotifySticker
    {
        Task ClinetNotifySticker(string msg);
    }
}