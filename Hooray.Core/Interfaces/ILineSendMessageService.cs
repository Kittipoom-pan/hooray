using Hooray.Core.ModelRequests;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface ILineSendMessageService
    {
        Task<(int,object)> SendMessageLine(LineSendMessage lineSendMessageRequest, string token);
    }
}
