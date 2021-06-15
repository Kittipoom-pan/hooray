using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface IChangeServices
    {
        Task<BaseResponse<Register>> CreateAccount(CreateAccountModel model);
        Task<BaseResponse<Register>> CreatePassword(CreatePasswordModel model);
        Task<BaseResponse<Register>> Login(LoginModel model);
        Task<BaseResponse<bool>> ForgotPassword(ForgotPasswordModel model);
        Task<BaseResponse<string>> VerifyForgotPassword(VerifyPasswordModel model);
    }
}
