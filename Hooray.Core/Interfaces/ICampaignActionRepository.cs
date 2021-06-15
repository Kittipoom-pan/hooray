using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface ICampaignActionRepository
    {
        Task AddCampaignAction(string campaign_id, string user_id, string lat,string lng,string event_name,int event_type);
        Task AddUserJoinCampaign(string campaign_id, string user_id, string lat,string lng,int code_type);
    }
}
