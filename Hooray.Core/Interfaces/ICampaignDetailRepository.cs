using Hooray.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface ICampaignDetailRepository
    {
        //Task<(bool, string)> CheckJoinUserCampaign(string campaign_id, string user_id);
        Task<UserJoin> CheckJoinUserCampaign(string campaign_id, string user_id);
        Task<(UserInfo, string)> GetUserInformation(int user_id);
        List<Campaign> GetAllCampaign(int user_id, string deviceType);
        List<Campaign> GetPerPageCampaign(int user_id, int page, int per_page);
        List<Campaign> GetAllFeedCampaign2(int user_id, int page, int per_page, string device_type);
        List<Campaign> GetAllFeedCampaign3(int user_id, int company_id, int page, int per_page, string device_type);
        Task<int> GetTotalCampaign2(int user_id);
        Task<int> GetTotalCampaign3(int user_id, int company_id);
        Task<int> InsertCampaignLike(string campaign_id, string user_id, int like_type);
        Task<int> GetLikeCampaign(string campaign_id, int like_type);
        Task<bool> UpdateLikeCampaign(string campaign_id, int like_count, int like_type);
        //Task<string> AddUserLineId(int user_id);
        //Task<string> GetUserCampaignResult(int campaign_id, int user_id);

    }
}
