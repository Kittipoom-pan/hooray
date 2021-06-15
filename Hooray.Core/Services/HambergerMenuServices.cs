using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.MySQLManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Hooray.Core.Services
{
    public class HambergerMenuServices : IHambergerMenuServices
    {
        private IMySQLManagerRepository _sql;
        private IMySQLManager _msg;
        private int messagecode = 0;
        private readonly ILogger _logger;
        private string clear = "";
        private PushManager pushManager = null;
        public HambergerMenuServices(IMySQLManagerRepository mySQLManagerRepository, IMySQLManager mySQLManager, ILogger<HambergerMenuServices> logger)
        {
            _logger = logger;
            _sql = mySQLManagerRepository;
            _msg = mySQLManager;
            if (pushManager == null)
            {
                pushManager = new PushManager();
            }
        }
      
        public BaseResponse<DBNull> UpdateAddress(UpdateAddressModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<DBNull> obj = new BaseResponse<DBNull>();
            obj.message = string.Empty;
            obj.status = false;
            obj.device_token_status = true;
            messagecode = 0;

            try
            {
                if (clear == "")
                {
                    string decode_address = System.Web.HttpUtility.UrlDecode(model.address);
                    string decode_district = System.Web.HttpUtility.UrlDecode(model.district);
                    string decode_amphor = System.Web.HttpUtility.UrlDecode(model.amphor);
                    string decode_province = System.Web.HttpUtility.UrlDecode(model.province);
                    obj.status = _sql.UpdateUserAddress(model.uid, decode_address, decode_district, decode_amphor, decode_province, model.zipcode);
                }
                else
                {
                    obj.device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                }
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
            }
            catch (Exception ex)
            {
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");

                _logger.LogError(ex, string.Format("UpdateAddress -- {0}", obj.message));
            }
            finally
            {
            }

            return obj;
        }

        public BaseResponse<UserInfoForEditModel> UpdateUserInformation(UpdateUserInformationModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<UserInfoForEditModel> obj = new BaseResponse<UserInfoForEditModel>();
            obj.data = new UserInfoForEditModel();
            obj.message = string.Empty;
            obj.status = false;
            obj.device_token_status = true;
            messagecode = 0;

            try
            {
                long tryInt = long.Parse(model.facebookID);
            }
            catch (Exception)
            {
                model.facebookID = "0";
            }

            try
            {
                if (clear == "")
                {
                    DateTime dt = DateTime.Now;
                    string imgname = string.Format("{0}_{1:yyyyMMddHHmmss}", model.uid, dt);

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
                    obj.data = _sql.UpdateUserInformation(model.uid, decode_firstName, decode_lastName, decode_displayName, decode_email, model.birthday, decode_gender, model.mobile, decode_FfirstName, decode_FlastName, decode_FdisplayName, decode_Femail, model.Fbirthday, decode_Fgender, model.Fmobile, model.facebookID, imgname, model.isPhoto);

                    if (obj.data == null)
                    {
                        messagecode = 301011; //Sorry,Cann't update your profile.
                    }
                    else
                    {
                       
                        obj.status = true;
                       
                        messagecode = 301010; //OK,Update profile complete.
                    }
                   
                }
                else
                {
                    obj.status = false;
                    obj.device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                }
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error.
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("UpdateUserInformation -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "UpdateUserInformation" + " " + uid);
            }
            finally
            {
            }

            return obj;
        }

        public BaseResponse<StatusPushModel> ControlUserNotification(ControlUserNotificationModel model)
        {
            BaseResponse<StatusPushModel> obj = new BaseResponse<StatusPushModel>();
            obj.data = new StatusPushModel();
            obj.device_token_status = true;
            obj.status = false;
            messagecode = 0;

            try
            {
                if (_sql.VerifyDeviceToken(model.uid, model.tokenID))
                {
                    obj.device_token_status = true;
                    obj.data.status_push = _sql.UpdateUserNotification(model.uid, model.status);
                    obj.status = true;

                }
                else
                {
                    obj.status = false;
                    obj.device_token_status = true;
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                _logger.LogError(ex, string.Format("ControlUserNotification -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetAllLanguage" + " " + uid);
            }
            finally
            {
            }

            return obj;
        }

        public BaseResponse<StatusOTPModel> GetOtpReprize(GetOtpReprizeModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<StatusOTPModel> obj = new BaseResponse<StatusOTPModel>();
            obj.data = new StatusOTPModel();
            obj.message = string.Empty;
            obj.status = false;
            obj.device_token_status = true;

            try
            {
                if (clear == "")
                {
                    obj.device_token_status = true;
                    obj.status = false;
                    //messagecode = 306004;
                    //obj.message = "ฟังก์ชั่นนี้ยังไม่เปิดใช้งาน";
                    obj.data.otp = _sql.GetUniqueKey(8);
                    obj.status = _sql.UpdateRePrizeOTP(model.uid, obj.data.otp);
                    //obj.otp = "Not Available";
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("GetReprizeOTP -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetReprizeOTP");
            }
            finally
            {
            }

            return obj;
        }

 
        public BaseResponse<OldUserModel> RestorePrize(RestorePrizeModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<OldUserModel> obj = new BaseResponse<OldUserModel>();
            obj.message = string.Empty;
            obj.status = false;
            obj.device_token_status = true;
            obj.data = new OldUserModel();
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
                if (clear == "")
                {
                    obj.device_token_status = true;
                    obj.status = false;
                    obj.data = _sql.UpdateRestorePrize(model.uid, model.fbid, model.otpcode);
                    if ((obj.data.status_prize !=false) && (obj.data.user_id != 0))
                    {
                        messagecode = 306001;
                        obj.status = true;
                        if (obj.data.token_id != "")
                        {

                            string msgPush = _msg.GetMessageLangCard(model.lang, 201007, "", "", "");

                            List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();


                            int log_id = _sql.InsertLogPush(NotificationType.RESTOREPRIZE.ToString(), "", 0);
                            _sql.createPushFile(msgPush, NotificationType.RESTOREPRIZE.ToString(), 1, obj.data.token_id, obj.data.device_type.ToLower(), tempList, 1);

                        }
                    }
                    else
                    {
                        if (obj.data.user_id == 0)
                        {
                            messagecode = 306005;
                        }
                        else if (obj.data.status_prize == false)
                        {
                            messagecode = 306003; //message ไม่มีรางวัลที่ย้าย
                        }
                    }
                    obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                    //messagecode = 306004;
                    //obj.message = "ฟังก์ชั่นนี้ยังไม่เปิดใช้งาน";
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("RestorePrize -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "RestorePrize");
            }
            finally
            {
            }

            return obj;
        }

        public BaseResponse<DBNull> AddFeedback(AddFeedbackModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<DBNull> obj = new BaseResponse<DBNull>();
            obj.status = false;
            obj.device_token_status = true;
            obj.message = string.Empty;
            messagecode = 0;

            try
            {
                if (clear == "")
                {
                    obj.device_token_status = true;
                    string decode_feedback = System.Web.HttpUtility.UrlDecode(model.feedback);
                    obj.status = _sql.InsertFeedback(model.uid, decode_feedback);
                }
                else
                {
                    obj.status = false;
                    obj.device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                    obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error.
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("AddFeedback -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetUserInformation" + " " + uid);
            }
            finally
            {
            }

            return obj;
        }

        public BaseResponse<List<StoreConfigModel>> GetMoreDetail(string market, string lang)
        {
            BaseResponse<List<StoreConfigModel>> obj = new BaseResponse<List<StoreConfigModel>>();
            obj.status = true;
            obj.data = new List<StoreConfigModel>();
            messagecode = 0;

            try
            {
                obj.data = _sql.GetStoreConfig(market);
            }
            catch (Exception ex)
            {
                obj.status = false;
                _logger.LogError(ex, string.Format("GetMoreDetail -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetMoreDetail");
            }
            finally
            {
            }

            return obj;
        }
    }
}
