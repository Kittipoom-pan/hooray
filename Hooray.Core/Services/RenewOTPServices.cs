using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.Manager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace Hooray.Core.Services
{
    public class RenewOTPServices : IRenewOTPServices
    {
        private IMySQLManagerRepository _sql;
        private IMySQLManager _msg;
        private int messagecode = 0;
        private readonly ILogger _logger;
        public RenewOTPServices(IMySQLManagerRepository mySQLManagerRepository, IMySQLManager mySQLManager, ILogger<RenewOTPServices> logger)
        {
            _logger = logger;
            _sql = mySQLManagerRepository;
            _msg = mySQLManager;
        }
        public BaseResponse<string> ResetVerifyAccount(string deviceId, string lang)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<string> obj = new BaseResponse<string>();
            obj.status = true;
            obj.message = string.Empty;
            messagecode = 0;

            try
            {
                Random rand = new Random();
                int randNumberMobile = rand.Next(1000, 9999);

                DataTable dt = _sql.UpdateUserVerifyMobile(deviceId, Convert.ToString(randNumberMobile));
                DataRow dr = dt.Rows[0];
                string mobile_number = dr["display_mobile"].ToString();
                int userID = int.Parse(dr["user_id"].ToString());
                if (!string.IsNullOrEmpty(mobile_number))
                {
                    bool success = _sql.InsertSmsLog(userID, mobile_number, "ResetVerifyAccount : " + Convert.ToString(randNumberMobile), deviceId, 1);
                    if (success)
                    {
                        obj.message = "กรุณารอรับรหัสยืนยันทาง SMS";
                        string status; string messageID; string taskID;
                        _sql.SendMessage(mobile_number, "รหัสยืนยัน Hooray! คือ " + Convert.ToString(randNumberMobile), "", "", out status, out messageID, out taskID);
                    }
                    else
                    {
                        messagecode = 301054;
                        obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                        obj.status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error.
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("ResetVerifyAccount -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "ResetVerifyAccount" + " " + deviceId);
            }
            finally
            {
            }

            return obj;
        }
    }
}
