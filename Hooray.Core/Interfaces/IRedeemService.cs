using Hooray.Core.RequestModels;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface IRedeemService
    {
        Task<object> ClientDeal(string campaignId, string userlineId, string displayName);
        Task<object> ClientUserHistoryRedeem(string userlineId, string displayName);
        Task<object> DealCodeRedeem (UserLineRequest redeem);
    }
}
