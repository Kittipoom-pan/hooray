using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.Interfaces
{
    public interface IRenewOTPServices
    {
        BaseResponse<string> ResetVerifyAccount(string deviceid, string lang);
    }
}
