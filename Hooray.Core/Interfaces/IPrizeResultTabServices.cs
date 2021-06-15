using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.Interfaces
{
    public interface IPrizeResultTabServices
    {
         BaseResponse<List<UserPrize>> GetPrizeList(int uid ,string lang);
        BaseResponse<PrizeDetailModel> GetPrizeDetail(int uid, int upid, string lang);
        BaseResponse<HoorayDeleteUserPrize> DeleteUserPrize(DeleteUserPrizeModel model);
    }
}
