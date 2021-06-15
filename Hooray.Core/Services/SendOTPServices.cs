using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.Manager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hooray.Core.Services
{
    public class SendOTPServices : ISendOTPServices
    {
        private IMySQLManagerRepository _sql;
        private IMySQLManager _msg;
        private int messagecode = 0;
        private readonly ILogger _logger;
        public SendOTPServices(IMySQLManagerRepository mySQLManagerRepository , IMySQLManager mySQLManager, ILogger<SendOTPServices> logger)
        {
            _logger = logger;
            _sql = mySQLManagerRepository;
            _msg = mySQLManager;
        }
        public BaseResponse<VerifyModel> VerifyAccount(string deviceID, string verifyCode,/* string mobile, string tokenID, */string lang)
        {
            BaseResponse<VerifyModel> obj = new BaseResponse<VerifyModel>();
            obj.data = new VerifyModel();
            obj.message = string.Empty;
            obj.device_token_status = true;
            obj.status = false;
            obj.data.verify_mobile_status = false;

            try
            {
                DateTime datetime = DateTime.Now;
                string value = string.Empty;
                if (!string.IsNullOrEmpty(deviceID))
                {
                    DataTable tableLastGen = _sql.GetLastGenVerifyMobile(deviceID);
                    DataRow rowLastGen = tableLastGen.Rows[0];

                    string reqDate = rowLastGen["last_mobile_verify_code_gen_date"].ToString();
                    string codeverify = rowLastGen["mobile_verify_code"].ToString();
                    int user_id = int.Parse(rowLastGen["user_id"].ToString());

                    if (verifyCode == codeverify)
                    {
                        if (datetime < DateTime.Parse(reqDate)) //มีเวลา 1 วัน
                        {
                            bool success = _sql.VerifyCampaignMobile(user_id, verifyCode);

                            if (success)
                            {
                                obj.status = true;
                                obj.data.verify_mobile_status = true;
                                messagecode = 301020; //OK,Account verify complete.
                                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");

                            }
                            else
                            {
                                obj.status = false;
                                messagecode = 301021; //Sorry,Cann't verify account please check your code and try again.
                                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                            }
                        }
                        else
                        {
                            obj.status = false;
                            messagecode = 301007; //Sorry, Code Verify Time Out.
                            obj.message = _msg.GetMessageLang(lang, messagecode, "", "");


                        }
                    }
                    else
                    {
                        obj.status = false;
                        messagecode = 301008; //Sorry, Verify Code Mismatch
                        obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                    }
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("VerifyAccount -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "VerifyAccount" + " " + deviceID);
            }
            finally
            {
            }

            return obj;
        }
    }
}
