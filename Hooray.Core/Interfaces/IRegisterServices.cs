using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface IRegisterServices
    {
        Task<BaseResponse<Register>> RegisterCampaignApplicationNew(RegisterNewModel model);

    }
}
