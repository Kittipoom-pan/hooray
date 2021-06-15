using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface IRedeemRepository
    {
        Task<int> GetDeal(string campaignId);
        Task<HryUserProfile> GetUser(string lineUserId, string displayName);
        Task<DealCodeRedeem> GetUserByLineId(string campaignId,string lineUserId, string displayName);
    }
}
