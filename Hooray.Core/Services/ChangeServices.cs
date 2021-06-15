using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hooray.Core.Services
{
    public class ChangeServices : IChangeServices
    {
        private IMySQLManagerRepository _sql;
        private IMySQLManager _msg;
        private readonly ILogger _logger;
        private int messagecode = 0;
      
        public ChangeServices(IMySQLManagerRepository mySQLManagerRepository, IMySQLManager mySQL, ILogger<ChangeServices> logger)
        {
            _logger = logger;
            _sql = mySQLManagerRepository;
            _msg = mySQL;
        }

        public async Task<BaseResponse<Register>> CreateAccount(CreateAccountModel model)
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
                var checkmobile = _sql.CheckMobile(model.mobile);
                if (checkmobile)
                {
                    obj.data = null;
                    obj.status = false;
                    messagecode = 720198;
                    obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                }
                else
                {
                    Random rand = new Random();
                    int randNumberMobile;

                    string decode_firstName = System.Web.HttpUtility.UrlDecode("");
                    string decode_lastName = System.Web.HttpUtility.UrlDecode("");
                    string decode_displayName = System.Web.HttpUtility.UrlDecode("");
                    string decode_email = System.Web.HttpUtility.UrlDecode("");
                    string decode_gender = System.Web.HttpUtility.UrlDecode("");
                    string decode_FfirstName = System.Web.HttpUtility.UrlDecode("");
                    string decode_FlastName = System.Web.HttpUtility.UrlDecode("");
                    string decode_FdisplayName = System.Web.HttpUtility.UrlDecode("");
                    string decode_Femail = System.Web.HttpUtility.UrlDecode("");
                    string decode_Fgender = System.Web.HttpUtility.UrlDecode("");

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

                    obj.data = _sql.RegisterCampaignApplicationNew2("0", decode_firstName, decode_lastName, decode_displayName, decode_email, "", decode_gender, model.mobile, decode_FfirstName, decode_FlastName, decode_FdisplayName, decode_Femail, "", decode_Fgender, "", "", model.tokenID, model.lang, 0, 0, model.deviceID, 0, Convert.ToString(randNumberMobile), model.versionApp, model.versionIOS, model.versionAndroid);
                    if (obj.data.user_id > 0)
                    {
                      
                        obj.device_token_status = true;
                        obj.status = true;
                      
                        obj.startup_badge = await _msg.GetStartupBadge(obj.data.user_id);
                        messagecode = 300001; //OK,Register new user complete

                        if ((obj.data.require_mobile_verify) && (model.mobile != "0123456789"))
                        {
                            if (last_verify_code == 0)
                            {
                                // เพิ่ม add event log เพื่อเก็บ lat lng
                                _sql.InsertEventLog(obj.data.user_id, 0, "open app", "", 1, "", "", 0, 0, "", "");
                                //
                                bool success = _sql.InsertSmsLog(obj.data.user_id, model.mobile, "CreateAccount : " + Convert.ToString(randNumberMobile), model.deviceID, 1);
                                if (success)
                                {
                                    string status; string messageID; string taskID;
                                    _sql.SendMessage(model.mobile, "รหัส: " + Convert.ToString(randNumberMobile), "", "", out status, out messageID, out taskID);
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
               
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("CreateAccount -- {0}", obj.message));
            }
            finally
            {
            }

            return obj;
        }

        public async Task<BaseResponse<Register>> CreatePassword(CreatePasswordModel model)
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
                if (model.pass1 != model.pass2)
                {
                    obj.status = false;
                    messagecode = 720196; 
                    obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                }
                else
                {
                    int userid = Convert.ToInt32(model.uid);
                    UserInfo user = _sql.GetUserInformation2(userid);
                    obj.startup_badge = await _msg.GetStartupBadge(userid);
                    if (user == null)
                    {
                        obj.status = false;
                        messagecode = 720197;
                        obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                    }
                    else
                    {
                       
                        obj.data = _sql.GetUser(userid);
                        obj.status = _sql.CreatePassword(userid, model.pass1);
                        messagecode = 200; 
                        obj.message = "Success";
                        obj.status = true;
                    }
                }
            }
            catch (Exception ex)
            {

                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("CreatePassword -- {0}", obj.message));
            }
            finally
            {
            }
            return obj;
        }

        public async Task<BaseResponse<Register>> Login(LoginModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
            BaseResponse<Register> obj = new BaseResponse<Register>();
            obj.data = new Register();
            obj.device_token_status = true;
            obj.message = "";
            obj.status = false;
            obj.startup_badge = new StartupBadgeViewModel();
            try
            {

                obj.data = _sql.Login(model);
                if (obj.data == null)
                {
                    obj.status = false;
                    messagecode = 720197;
                    obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                }
                else
                {
                    obj.startup_badge = await _msg.GetStartupBadge(obj.data.user_id);
                    messagecode = 200;
                    obj.message = "Success";
                    obj.status = true;
                }
            }
            catch (Exception ex)
            {

                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("Login -- {0}", obj.message));
            }
            finally
            {
            }
            return obj;
        }

        public async Task<BaseResponse<bool>> ForgotPassword(ForgotPasswordModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
            BaseResponse<bool> obj = new BaseResponse<bool>();
            obj.data = new bool();
            obj.device_token_status = true;
            obj.message = "";
            obj.status = false;
            obj.startup_badge = new StartupBadgeViewModel();
            //obj.data.sms_user = new List<Sms>();
            messagecode = 0;


            try
            {
                var user = _sql.GetUserMobile(model.mobile);
                if (user == null)
                {
                    messagecode = 720197;
                    obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                }
                else
                {
                    Random rand = new Random();
                    int randNumberMobile;
                    randNumberMobile = rand.Next(1000, 9999);
                    //bool success = _sql.InsertSmsLog(user.UserId, model.mobile, "ForgotPassword : " + Convert.ToString(randNumberMobile), user.DeviceId, 1);
                    bool result = _sql.CheckDateOtp(model.mobile);
                    if (result)
                    {
                        messagecode = 720199;
                        obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                    }
                    else
                    {
                        bool success = _sql.InsertOtpDetail(randNumberMobile, "ForgotPassword", model.mobile , out int number);
                        if (success )
                        {
                            string status; string messageID; string taskID;
                            if (number!=0)
                            {
                                randNumberMobile = number;
                            }
                            _sql.SendMessage(model.mobile, "ForgotPassword Code: " + Convert.ToString(randNumberMobile), "", "", out status, out messageID, out taskID);
                            obj.data = true;
                            obj.status = true;
                            obj.startup_badge = await _msg.GetStartupBadge(user.UserId);
                            obj.message = "Success";
                        }
                        else
                        {
                            obj.message = " Reset Verify ForgotPassword";
                        }
                    }

                }
               
              
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("ForgotPassword -- {0}", obj.message));
            }
            finally
            {
            }

            return obj;
        }

        public async Task<BaseResponse<string>> VerifyForgotPassword(VerifyPasswordModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
            BaseResponse<string> obj = new BaseResponse<string>();
            obj.device_token_status = true;
            obj.message = "";
            obj.status = false;
            obj.startup_badge = new StartupBadgeViewModel();
            messagecode = 0;


            try
            {
                var getuser = _sql.GetUserMobile(model.mobile);
                if (getuser == null)
                {
                    messagecode = 720197;
                    obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                }
                else
                {
                    /// OTP หมดอายุหรือยัง OTP มีเวลา 1 วัน
                    bool checkresult = _sql.CheckOtp(model);
                    if (checkresult)
                    {
                        messagecode = 720200;
                        obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                    }
                    else
                    {
                        var result = _sql.VerifyForgotPassword(model);
                        if (result)
                        {
                            //var user = _sql.GetUserMobile(model.mobile);
                            obj.startup_badge = await _msg.GetStartupBadge(getuser.UserId);
                            obj.data = getuser.UserId.ToString();
                            messagecode = 200;
                            obj.message = "Success";
                            obj.status = true;
                        }
                        else
                        {
                            obj.status = false;
                            messagecode = 301021; //Sorry,Cann't verify account please check your code and try again.
                            obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                        }
                    }
                }
               
              
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("VerifyForgotPassword -- {0}", obj.message));
            }
            finally
            {
            }

            return obj;
        }
    }
}
