using Hooray.Core.ViewModels;
using Hooray.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.Interfaces
{
    
    public interface IIntroSplashScreenServices
    {
        BaseResponse<List<MessageUI>> GetDisplayTextUI(int uid, string token, string lang);

    }
}
