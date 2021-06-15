using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using static Hooray.Core.ViewModels.CampaignJoinViewModel;

namespace Hooray.Core.Interfaces
{
    public interface IJoinOnFeedTabServices
    {
        BaseResponse<Join> AddJoinCampaign(int uid, string cpid, int sid, string tokenID, float lat, float lng, string lang);
        BaseResponse<CampaignResult> GetCampaignResult(int uid, string cpid, string token, string lang);
    }
}
