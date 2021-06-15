using Hooray.Core.AppsettingModels;
using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.Manager;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Hooray.Core.Services
{
    public class CheckVersionService : ICheckVersionService
    {
        private int messagecode = 0;

        private readonly IMySQLManager _mySQLManager;
        private readonly IMySQLManager _msg;
        private readonly IOptions<UrlDownloadApp> _appSettings;
        private readonly ILogger _logger;
        public CheckVersionService(IMySQLManager mySQLManager, IMySQLManager msg, IOptions<UrlDownloadApp> appSettings, ILogger<CheckVersionService> logger)
        {
            _logger = logger;
            _mySQLManager = mySQLManager;
            _msg = msg;
            _appSettings = appSettings;
        }

        public async Task<CheckVersionViewModel> CheckVersion(string versionapp, string devicetype, string lang)
        {
            CheckVersionViewModel obj = new CheckVersionViewModel();
            obj.message = string.Empty;
            obj.status = false;
            messagecode = 0;

            try
            {
                int count = 0;
                obj.status = true;
                char[] version = versionapp.ToCharArray();
                string versionFull = string.Empty;
                foreach (char str in version)
                {
                    if (str == '.')
                    {
                        count++;
                    }
                }
                if (count > 1)
                {
                    versionFull = versionapp.Split('.')[0] + "." + versionapp.Split('.')[1] + versionapp.Split('.')[2];
                }
                else
                {
                    versionFull = versionapp.Split('.')[0] + "." + versionapp.Split('.')[1];
                }

                //float versionApp = 1.1;
                obj.version_status = await _mySQLManager.CheckVersion(float.Parse(versionFull), devicetype.ToLower());
                if (!obj.version_status)
                {
                    messagecode = 311032;
                    obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                    if (devicetype.ToLower() == "android")
                    {
                        obj.url_download = _appSettings.Value.UrlAndroid;
                    }
                    else if (devicetype.ToLower() == "ios")
                    {
                        obj.url_download = _appSettings.Value.UrlIos;
                    }
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error.
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("CheckVersion -- {0}", obj.message));

                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "CheckVersion" + " " + devicetype);
            }
            finally
            {

            }

            return obj;
        }
    }
}
