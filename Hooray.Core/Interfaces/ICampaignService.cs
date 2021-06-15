using Hooray.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface ICampaignService
    {
        Task<AllCampaignViewModel> GetAllCampaign(int user_id, string lang);
        //Task<PagedResponse<List<Campaign>>> GetAllFeedList(int user_id, string lang, int page, int limit, string route, PaginationFilter pageFilter);
        Task<PagedResponse<List<Campaign>>> GetAllFeedCampaign2(int user_id, string lang, int company_id, PaginationFilter pageFilter, string token, string route);
        Task<HoorayUpdateLike> UpdateLikeCampaign(string user_id, string lang,int liketype, string campaign_id);
    }
}
