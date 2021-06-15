using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using Hooray.Infrastructure.Manager;
using Hooray.Infrastructure.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Infrastructure.Repositories
{
    public  class IntroSplashScreenServices : IIntroSplashScreenServices
    {
        private const string COMMA = ",";
        private int messagecode = 0;
        private string clear = "";
        private IMySQLManagerRepository _mySQLManagerRepository;
        private IMySQLManager _msg;
        private readonly ILogger _logger;
        public IntroSplashScreenServices(IMySQLManagerRepository mySQLManagerRepository , IMySQLManager mySQL , ILogger<IntroSplashScreenServices> logger)
        {
            _logger = logger;
            _mySQLManagerRepository = mySQLManagerRepository;
            _msg = mySQL;
        }

        public BaseResponse<List<MessageUI>> GetDisplayTextUI(int uid, string token, string lang)
        {

            BaseResponse<List<MessageUI>> obj = new BaseResponse<List<MessageUI>>();
            obj.message = string.Empty;
            obj.device_token_status = true;
            obj.status = true;
            obj.data = new List<MessageUI>();
            messagecode = 0;

            try
            {
                if ((uid.ToString() == "0" && token == "000") || clear == "")
                {
                    obj.data = _mySQLManagerRepository.GetDisplayTextUI(lang.ToUpper());
                }
                else
                {
                    obj.status = false;
                    obj.device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                    obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("GetDisplayTextUI -- {0}", obj.message));

                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetDisplayTextUI" + " " + uid);
            }
            finally
            {
            }

            return obj;
        }
    }
}
