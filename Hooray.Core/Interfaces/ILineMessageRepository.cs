using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface ILineMessageRepository
    {
        Task AddLineChannel(string channel_token, int company_id);
        Task<string> GetTokenLine(string environment);
        Task AddSendMessageLog(string user_id);
    }
}
