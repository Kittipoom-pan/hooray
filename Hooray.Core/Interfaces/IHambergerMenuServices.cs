using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.Interfaces
{
    public interface IHambergerMenuServices
    {
        BaseResponse<DBNull> UpdateAddress(UpdateAddressModel model);
        BaseResponse<UserInfoForEditModel> UpdateUserInformation(UpdateUserInformationModel model);
        BaseResponse<StatusPushModel> ControlUserNotification(ControlUserNotificationModel model);
        BaseResponse<StatusOTPModel> GetOtpReprize(GetOtpReprizeModel model);
        BaseResponse<OldUserModel> RestorePrize(RestorePrizeModel model);

        BaseResponse<DBNull> AddFeedback(AddFeedbackModel model);
        BaseResponse<List<StoreConfigModel>> GetMoreDetail(string market, string lang);
    }
}
