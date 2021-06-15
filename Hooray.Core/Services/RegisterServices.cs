using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.Manager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hooray.Core.Services
{
    public class RegisterServices : IRegisterServices
    {
        private IMySQLManagerRepository _sql;
        private IMySQLManager _msg;
        private readonly ILogger _logger;
        public RegisterServices(IMySQLManagerRepository mySQLManagerRepository , IMySQLManager mySQL , ILogger<RegisterServices> logger)
        {
            _logger = logger;
            _sql = mySQLManagerRepository;
            _msg = mySQL;
        }
        private int messagecode = 0;
        public async Task<BaseResponse<Register>> RegisterCampaignApplicationNew(RegisterNewModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
            BaseResponse<Register> obj = new BaseResponse<Register>();
            obj.data = new Register();
            obj.device_token_status = true;
            obj.message = "";
            obj.status = false;
            obj.startup_badge = new StartupBadgeViewModel();
            //obj.data.sms_user = new List<Sms>();
            messagecode = 0;

            try
            {
                long tryInt = long.Parse(model.fbid);
            }
            catch (Exception)
            {
                model.fbid = "0";
            }
            try
            {
                Random rand = new Random();
                int randNumberMobile;

                string decode_firstName = System.Web.HttpUtility.UrlDecode(model.firstName);
                string decode_lastName = System.Web.HttpUtility.UrlDecode(model.lastName);
                string decode_displayName = System.Web.HttpUtility.UrlDecode(model.displayName);
                string decode_email = System.Web.HttpUtility.UrlDecode(model.email);
                string decode_gender = System.Web.HttpUtility.UrlDecode(model.gender);
                string decode_FfirstName = System.Web.HttpUtility.UrlDecode(model.FfirstName);
                string decode_FlastName = System.Web.HttpUtility.UrlDecode(model.FlastName);
                string decode_FdisplayName = System.Web.HttpUtility.UrlDecode(model.FdisplayName);
                string decode_Femail = System.Web.HttpUtility.UrlDecode(model.Femail);
                string decode_Fgender = System.Web.HttpUtility.UrlDecode(model.Fgender);

                int last_verify_code = _sql.CheckLastVerifyCode(model.mobile);
                if (model.mobile == "0123456789")
                {
                    randNumberMobile = 4321;
                }
                else if (last_verify_code == 0)
                {
                    randNumberMobile = rand.Next(1000, 9999);
                }
                else
                {
                    randNumberMobile = last_verify_code;
                }

                obj.data = _sql.RegisterCampaignApplicationNew(model.fbid, decode_firstName, decode_lastName, decode_displayName, decode_email, model.birthday, decode_gender, model.mobile, decode_FfirstName, decode_FlastName, decode_FdisplayName, decode_Femail, model.Fbirthday, decode_Fgender, model.Fmobile, model.type, model.tokenID, model.lang, model.lat, model.lng, model.deviceID, model.isPhoto, Convert.ToString(randNumberMobile), model.versionApp, model.versionIOS, model.versionAndroid);
                if (obj.data.user_id > 0)
                {
                    //if (fbid != "0")
                    //{
                    //    _sql.UpdateRestorePrizeFB(obj.register.user_id, obj.register.old_user_id);
                    //}
                    obj.device_token_status = true;
                    obj.status = true;
                    //obj.data.sms_user = _sql.GetUserSMS(obj.data.register.user_id);
                    obj.startup_badge = await _msg.GetStartupBadge(obj.data.user_id);
                    messagecode = 300001; //OK,Register new user complete

                    if ((obj.data.require_mobile_verify) && (model.mobile != "0123456789"))
                    {
                        if (last_verify_code == 0)
                        {
                            // เพิ่ม add event log เพื่อเก็บ lat lng
                            _sql.InsertEventLog(obj.data.user_id, 0, "open app", "", 1, "", "", model.lat, model.lng, model.type, "");
                            //
                            bool success = _sql.InsertSmsLog(obj.data.user_id, model.mobile, "RegisterCampaignApplication : " + Convert.ToString(randNumberMobile), model.deviceID, 1);
                            if (success)
                            {
                                string status; string messageID; string taskID;
                                _sql.SendMessage(model.mobile, "รหัสลงทะเบียน Hooray! คือ " + Convert.ToString(randNumberMobile), "", "", out status, out messageID, out taskID);
                            }
                        }
                        else
                        {
                            //messagecode = 311029; //
                        }
                    }
                }
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("RegisterCampaignApplicationNew -- {0}", obj.message));

                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "RegisterCampaignApplicationNew" + " " + model.deviceID);
            }
            finally
            {
            }

            return obj;
        }

    }
}
