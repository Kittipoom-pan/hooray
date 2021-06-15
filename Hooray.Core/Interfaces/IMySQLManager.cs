using Hooray.Core.ViewModels;
using Hooray.Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Hooray.Core.ViewModels.CampaignJoinViewModel;

namespace Hooray.Core.Interfaces
{
    public interface IMySQLManager
    {
        Task<StartupBadgeViewModel> GetStartupBadge(int user_id);
        Task<CheckCampaignViewModel> CheckCampaignBeforeJoin(string campaign_id,int user_id);


        string GetMessageLang(string Lang, int MsgCode, string UserIDAction, string UserIDReaction);

        List<MessageUI> GetMessageLang(string pLang, int pMsgCode, int pMsgType);

        Task<bool> CheckVersion(float version_app, string device_type);
        Task<bool> CheckGroup(string campaign_id);
        Task<bool> CheckWinGroup(int user_id, string campaign_id);
        Task<int> CheckFollowAndJoin(int company_id, int user_id, string campaign_id);
        Task<int> GetCampaignDigit(string campaign_id);
        Task<Join> InsertNewUserJoin(string campaign_id, int user_id, double join_number, float lat, float lng, string verify);
        //BaseResponsePagination GetPerPageCampaign(int pUserID, int pPage, int pPerPage);

        string GetMessageLangCard(string Lang, int MsgCode, string UserIDAction, string UserIDReaction, string CardName);
    }
}
