using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.Interfaces
{
    public interface ISendOTPServices
    {
        BaseResponse<VerifyModel> VerifyAccount(string deviceID, string verifyCode, /*string mobile, string tokenID,*/ string lang);
    }
}
