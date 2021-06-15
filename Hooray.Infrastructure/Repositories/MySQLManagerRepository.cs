using Hooray.Core.AppsettingModels;
using Hooray.Core.Entities;
using Hooray.Core.Interfaces;
using Hooray.Core.Manager;
using Hooray.Infrastructure.DBContexts;
using Hooray.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using static Hooray.Core.ViewModels.CampaignJoinViewModel;

namespace Hooray.Core.ViewModels
{
    public  class MySQLManagerRepository : IMySQLManagerRepository
    {
        private readonly IOptions<Resource> _appSettings;
        private readonly devhoorayContext _context;
        private readonly MySqlConnection _conn;

        public MySQLManagerRepository(devhoorayContext context , IOptions<Resource> appSettings)
        {
            _context = context;
            _appSettings = appSettings;
            _conn = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
        }

        public List<MessageUI> GetDisplayTextUI(string pLang)
        {
            List<MessageUI> msgUIList = new List<MessageUI>();

            var table = _context.SystemMessage.Where(s => s.MessageLang.Contains(pLang)).ToList();
            if (table != null && table.Count > 0)
            {
                MessageUI msgUI;
                foreach (var e in table)
                {
                    msgUI = new MessageUI();
                    msgUI.loadDataMessageUI(e);
                    msgUIList.Add(msgUI);
                }
            }
            
            return msgUIList;
        }

        public int CheckLastVerifyCode(string pMobile)
        {
            int last_verify_code = 0;

            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_check_last_verify_code";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pMobile", pMobile);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    last_verify_code = int.Parse(dr["mobile_verify_code"].ToString());
                }
                _conn.Close();
                return last_verify_code;
            }
        }

        public Register RegisterCampaignApplicationNew(string pFacebookID, string pFirstName, string pLastName, string pDisplayName, string pEmail, string pBirthday, string pGender, string pMobile, string pFFirstName, string pFLastName, string pFDisplayName, string pFEmail, string pFBirthday, string pFGender, string pFMobile, string pRegisterType, string pDeviceToken, string pLang, float pLat, float pLng, string pDeviceID, int isPhoto, string pMobileVerifyCode, string pVersionApp, string pVersionIOS, string pVersionAndroid)
        {
            Register register = new Register();
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_register_campaign_application_new";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pFacebookID", pFacebookID);

                cmd.Parameters.AddWithValue("@pFirstName", pFirstName);

                cmd.Parameters.AddWithValue("@pLastName", pLastName);

                cmd.Parameters.AddWithValue("@pDisplayName", pDisplayName);

                cmd.Parameters.AddWithValue("@pEmail", pEmail);

                DateTime datetime = DateTime.Now;
                bool canPares = DateTime.TryParseExact(pBirthday, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out datetime);
                if (canPares)
                {
                    cmd.Parameters.AddWithValue("@pBirthday", datetime);
                }

                cmd.Parameters.AddWithValue("@pGender", pGender);

                cmd.Parameters.AddWithValue("@pMobile", pMobile);

                cmd.Parameters.AddWithValue("@pFFirstName", pFFirstName);

                cmd.Parameters.AddWithValue("@pFLastName", pFLastName);

                cmd.Parameters.AddWithValue("@pFDisplayName", pFDisplayName);

                cmd.Parameters.AddWithValue("@pFEmail", pFEmail);

                DateTime datetime2 = DateTime.Now;
                bool canPares2 = DateTime.TryParseExact(pFBirthday, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out datetime2);
                if (canPares2)
                {
                    cmd.Parameters.AddWithValue("@pFBirthday", datetime2);
                }

                cmd.Parameters.AddWithValue("@pFGender", pFGender);

                cmd.Parameters.AddWithValue("@pFMobile", pFMobile);

                cmd.Parameters.AddWithValue("@pRegisterType", pRegisterType);

                cmd.Parameters.AddWithValue("@pDeviceToken", pDeviceToken);

                cmd.Parameters.AddWithValue("@pLang", pLang);

                cmd.Parameters.AddWithValue("@pLat", pLat);

                cmd.Parameters.AddWithValue("@pLng", pLng);

                cmd.Parameters.AddWithValue("@pDeviceID", pDeviceID);

                cmd.Parameters.AddWithValue("@pMobileVerifyCode", pMobileVerifyCode);

                cmd.Parameters.AddWithValue("@pVersionApp", pVersionApp);

                cmd.Parameters.AddWithValue("@pVersionIOS", pVersionIOS);

                cmd.Parameters.AddWithValue("@pVersionAndroid", pVersionAndroid);


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    register.loadDataRegister(table.Rows[0]);
                    DateTime dt = DateTime.Now;
                    string imgname = string.Format("{0}_{1:yyyyMMddHHmmss}", register.user_id, dt);
                    UpdateUserProfile(register.user_id, imgname);

                    string img = (isPhoto == 1) ? imgname : "no-image";

                    #region imgUserProfileRound
                    string user_r_img_path = "/user_profile_image/" + img + ImageType.image_round;
                    string user_r_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                       _appSettings.Value.Photo.UserPhotoPath, img + ImageType.image_round);
                    register.user_image_round = GetImagePhoto(user_r_img_path, user_r_img_url);
                    #endregion

                    #region imgUserProfileFull
                    string user_f_img_path = "/user_profile_image/" + img + ImageType.imagejpg;
                    string user_f_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                     _appSettings.Value.Photo.UserPhotoPath, img + ImageType.imagejpg);

                    register.user_image_full = GetImagePhoto(user_f_img_path, user_f_img_url);
                    #endregion

                    //register.startup_badge = GetStartupBadge(register.user_id);
                }
                _conn.Close();
                return register;
            }
        }

        public Register RegisterCampaignApplicationNew2(string pFacebookID, string pFirstName, string pLastName, string pDisplayName, string pEmail, string pBirthday, string pGender, string pMobile, string pFFirstName, string pFLastName, string pFDisplayName, string pFEmail, string pFBirthday, string pFGender, string pFMobile, string pRegisterType, string pDeviceToken, string pLang, float pLat, float pLng, string pDeviceID, int isPhoto, string pMobileVerifyCode, string pVersionApp, string pVersionIOS, string pVersionAndroid)
        {
            Register register = new Register();
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_register_campaign_application_new";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pFacebookID", pFacebookID);

                cmd.Parameters.AddWithValue("@pFirstName", pFirstName);

                cmd.Parameters.AddWithValue("@pLastName", pLastName);

                cmd.Parameters.AddWithValue("@pDisplayName", pDisplayName);

                cmd.Parameters.AddWithValue("@pEmail", pEmail);

                //DateTime datetime = DateTime.Now;
                //bool canPares = DateTime.TryParseExact(pBirthday, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out datetime);
                //if (canPares)
                //{
                    cmd.Parameters.AddWithValue("@pBirthday", null);
                //}

                cmd.Parameters.AddWithValue("@pGender", pGender);

                cmd.Parameters.AddWithValue("@pMobile", pMobile);

                cmd.Parameters.AddWithValue("@pFFirstName", pFFirstName);

                cmd.Parameters.AddWithValue("@pFLastName", pFLastName);

                cmd.Parameters.AddWithValue("@pFDisplayName", pFDisplayName);

                cmd.Parameters.AddWithValue("@pFEmail", pFEmail);

                //DateTime datetime2 = DateTime.Now;
                //bool canPares2 = DateTime.TryParseExact(pFBirthday, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out datetime2);
                //if (canPares2)
                //{
                    cmd.Parameters.AddWithValue("@pFBirthday", null);
                //}

                cmd.Parameters.AddWithValue("@pFGender", pFGender);

                cmd.Parameters.AddWithValue("@pFMobile", pFMobile);

                cmd.Parameters.AddWithValue("@pRegisterType", pRegisterType);

                cmd.Parameters.AddWithValue("@pDeviceToken", pDeviceToken);

                cmd.Parameters.AddWithValue("@pLang", pLang);

                cmd.Parameters.AddWithValue("@pLat", pLat);

                cmd.Parameters.AddWithValue("@pLng", pLng);

                cmd.Parameters.AddWithValue("@pDeviceID", pDeviceID);

                cmd.Parameters.AddWithValue("@pMobileVerifyCode", pMobileVerifyCode);

                cmd.Parameters.AddWithValue("@pVersionApp", pVersionApp);

                cmd.Parameters.AddWithValue("@pVersionIOS", pVersionIOS);

                cmd.Parameters.AddWithValue("@pVersionAndroid", pVersionAndroid);


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    register.loadDataRegister(table.Rows[0]);
                    DateTime dt = DateTime.Now;
                    string imgname = string.Format("{0}_{1:yyyyMMddHHmmss}", register.user_id, dt);
                    UpdateUserProfile(register.user_id, imgname);

                    string img = (isPhoto == 1) ? imgname : "no-image";

                    #region imgUserProfileRound
                    string user_r_img_path = "/user_profile_image/" + img + ImageType.image_round;
                    string user_r_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                       _appSettings.Value.Photo.UserPhotoPath, img + ImageType.image_round);
                    register.user_image_round = GetImagePhoto(user_r_img_path, user_r_img_url);
                    #endregion

                    #region imgUserProfileFull
                    string user_f_img_path = "/user_profile_image/" + img + ImageType.imagejpg;
                    string user_f_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                     _appSettings.Value.Photo.UserPhotoPath, img + ImageType.imagejpg);

                    register.user_image_full = GetImagePhoto(user_f_img_path, user_f_img_url);
                    #endregion

                    //register.startup_badge = GetStartupBadge(register.user_id);
                }
                _conn.Close();
                return register;
            }
        }
        public bool UpdateUserProfile(int pUserID, string pImgName)
        {
            bool success = false;

            DataTable table = new DataTable();
          

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_user_profile";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pImgName", pImgName);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    success = true;
                }
                _conn.Close();
                return success;
            }
        }

        public ImagePhoto GetImagePhoto(string pImagePath, string image_url)
        {
            ImagePhoto imgPhoto = new ImagePhoto();
            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {

                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_update_image_info";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pImagePath", pImagePath);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    imgPhoto.loadDataImagePhoto(row);
                }
                else
                {
                    string fileName = "C:/inetpub/wwwroot/MobileServices/HoorayProd" + pImagePath;
                    if (File.Exists(fileName))
                    {
                        imgPhoto.image_path = pImagePath;
                        imgPhoto.image_url = image_url;
                        imgPhoto.image_update_date = 0;
                    }
                    else
                    {
                        string[] imageName = pImagePath.Split('/');
                        try
                        {
                            if (Array.IndexOf(imageName, "user_profile_image") >= 0)
                            {
                                string[] temp = imageName[2].Split('_');
                                if (Array.IndexOf(temp, "r.png") >= 0)
                                {
                                    imgPhoto.image_path = "/user_profile_image/no-image" + ImageType.image_round;

                                    imgPhoto.image_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.Photo.UserPhotoPath, "no-image" + ImageType.image_round);
                                    imgPhoto.image_update_date = 0;
                                }
                                else
                                {
                                    imgPhoto.image_path = "/user_profile_image/no-image" + ImageType.imagejpg;
                                    imgPhoto.image_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.Photo.UserPhotoPath, "no-image" + ImageType.imagejpg);
                                    imgPhoto.image_update_date = 0;
                                }
                            }
                            else
                            {
                                imgPhoto.image_path = pImagePath;
                                imgPhoto.image_url = image_url;
                                imgPhoto.image_update_date = 0;
                            }
                        }
                        catch
                        {

                        }
                    }

                }
                _conn.Close();
            }
            return imgPhoto;
        }

        public StartupBadge GetStartupBadge(int pUserID)
        {
            StartupBadge startupBadge = new StartupBadge();

            DataTable table = new DataTable();
           
           
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_startup_badge";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    startupBadge.loadDataStartupBadge(table.Rows[0]);
                }
                _conn.Close();
                return startupBadge;
            }
           
        }

        public List<Sms> GetUserSMS(int pUserID)
        {
            List <Sms> SmsList = new List<Sms>();

            var table = _context.HryUserJoin.Where(s => s.UserId == pUserID.ToString() && s.CodeType ==1 ).ToList();
            if (table != null && table.Count > 0)
            {
                foreach (var row in table)
                {
                    Sms sms = new Sms();
                    sms.campaign_id = row.CampaignId;
                    sms.sms_text = row.SmsText;
                    sms.sms_image = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                    _appSettings.Value.Photo.SmsPhotoPath, row.SmsImage.ToString());
                    //sms.sms_image = string.Format(_appSettings.Value.Photo.SmsPhotoPath, _appSettings.Value.BaseUrl, row.SmsImage.ToString());
                    sms.create_date = Utility.convertToDateServiceFormatString(row.CreateDate.ToString());
                    SmsList.Add(sms);
                }
            }
            return SmsList;
        }

        public bool InsertEventLog(int pUserID, int pCampaignID, string pEventName, string pEventDesc, int pEventType, string pImgName, string pDeviceInfo, float pLat, float pLng, string pDeviceOS, string pShopName)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_insert_event_log";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pEventName", pEventName);
                cmd.Parameters.AddWithValue("@pEventDesc", pEventDesc);
                cmd.Parameters.AddWithValue("@pEventType", pEventType);
                cmd.Parameters.AddWithValue("@pImgName", pImgName);
                cmd.Parameters.AddWithValue("@pDeviceInfo", pDeviceInfo);
                cmd.Parameters.AddWithValue("@pLat", pLat);
                cmd.Parameters.AddWithValue("@pLng", pLng);
                cmd.Parameters.AddWithValue("@pDeviceOS", pDeviceOS);
                cmd.Parameters.AddWithValue("@pShopName", pShopName);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    success = true;
                }
                _conn.Close();
            }
            return success;
        }

        public bool InsertSmsLog(int pUserID, string pMobile, string pSmsMsg, string pDeviceID, int pSmsType)
        {
            bool success = false;

            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_insert_sms_log";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pMobile", pMobile);
                cmd.Parameters.AddWithValue("@pSmsMsg", pSmsMsg);
                cmd.Parameters.AddWithValue("@pDeviceID", pDeviceID);
                cmd.Parameters.AddWithValue("@pSmsType", pSmsType);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    success = Convert.ToBoolean(int.Parse(dr["smsCount"].ToString()));
                }
                _conn.Close();
            }
            
            return success;
        }

        #region SendMessage
        public bool SendMessage(string mobileNo, string message, string category, string senderName, out string status, out string messageID, out string taskID)
        {
            string url = string.Format(_appSettings.Value.Url);
            string username = string.Format(_appSettings.Value.AccountSMS);
            string encPassword = string.Format(_appSettings.Value.PasswordSMS);

            //string Url = "https://otp.sc4msg.com/SendMessage";
            //string username = "ferverly@nsisotp";
            //string encPassword = "BB76C2C1DD14126E8FA156EC8719E548BF42E2766B4017DC5009A0991AE9097C41F8BC9BCC73864F";

            status = string.Empty;
            messageID = string.Empty;
            taskID = string.Empty;

            HttpWebResponse webResponse = null;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ProtocolVersion = HttpVersion.Version11;
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                string option = "";
                if (category.Length == 0) { category = "General"; }
                option += ",send_type=" + category;
                if (senderName.Length > 0) { option += ",SENDER=" + senderName; }

                string postHeader = String.Format("ACCOUNT={0}&PASSWORD={1} &MOBILE={2}&MESSAGE={3}&OPTION={4}", username, encPassword, mobileNo, message, option);
                //if (scheduleTime != DateTime.MinValue)
                //{
                //    postHeader += "&SCHEDULE=" + scheduleTime.ToString("yyyy-MM-dd HH:mm", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                //}

                byte[] postHeaderBytes = Encoding.GetEncoding(874).GetBytes(postHeader);
                webRequest.ContentLength = postHeaderBytes.Length;

                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                }

                webResponse = (HttpWebResponse)webRequest.GetResponse();
                string response = "";
                using (Stream respStream = webResponse.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(respStream, Encoding.GetEncoding(874)))
                    {
                        response = sr.ReadToEnd().Trim();
                    }
                }

                status = this.GetResponseStatue(response);
                if (status == "0")
                {
                    string temp = response;
                    int index = 0;
                    while ((index = temp.IndexOf("\n")) >= 0)
                    {
                        string temp2 = temp.Substring(0, index);
                        int sindex = temp2.IndexOf("=");
                        if (sindex >= 0)
                        {
                            string name = temp2.Substring(0, sindex);
                            if (name.Equals("MESSAGE_ID"))
                            {
                                messageID = temp2.Substring(sindex + 1).Trim();
                            }
                            else if (name.Equals("TASK_ID"))
                            {
                                taskID = temp2.Substring(sindex + 1).Trim();
                                return true;
                            }
                        }
                        temp = temp.Substring(index + 1);
                    }
                }
                return false;
            }
            finally
            {
                if (webResponse != null)
                    webResponse.Close();
            }
        }

        private string GetResponseStatue(string responseString)
        {
            string temp = responseString;
            if (temp.IndexOf("END=OK") >= 0)
            {
                int index = temp.IndexOf("\n");
                string temp2 = temp.Substring(0, index);
                int index2 = temp2.IndexOf("=");
                if (index2 >= 0)
                {
                    string name2 = temp2.Substring(0, index2);
                    string status = temp2.Substring(index2 + 1).TrimEnd();
                    if (name2.Equals("STATUS"))
                    {
                        return status;
                    }
                }
            }
            throw new Exception("Incorrect Response");
        }


        #endregion

        #region SendOTP

        public DataTable GetLastGenVerifyMobile(string pDeviceID)
        {
            DataTable table = new DataTable();


            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_last_gen_verify_mobile";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pDeviceID", pDeviceID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                _conn.Close();
                return table;

            }
        }

        public bool VerifyCampaignMobile(int pUserID, string pVerifyCode)
        {
            bool success = true;


            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_verify_campaign_mobile";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pVerifyCode", pVerifyCode);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    success = Convert.ToBoolean(int.Parse(dr["smsCount"].ToString()));
                }
                _conn.Close();
            }

            return success;
        }
        #endregion

        #region RenewOTP
        public DataTable UpdateUserVerifyMobile(string pDeviceID, string pVerifyCode)
        {
            DataTable table = new DataTable();


            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_user_verify_mobile";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pDeviceID", pDeviceID);
                cmd.Parameters.AddWithValue("@pVerifyCode", pVerifyCode);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                _conn.Close();
                return table;

            }
        }
        #endregion

        #region Campaign
        public UserInfo GetUserInformation2(int pUserID)
        {
            UserInfo userInfo = new UserInfo();
            userInfo = null;
            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_user_information";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    userInfo = new UserInfo();
                    userInfo.loadDataUserInfo(table.Rows[0]);

                    #region imgUserProfile
                    string user_img_path = "/user_profile_image/" + userInfo.image_name_profile + ImageType.image_thumbnail;
                    string user_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                     _appSettings.Value.Photo.UserPhotoPath, userInfo.image_name_profile + ImageType.image_thumbnail);
                    userInfo.user_image = GetImagePhoto(user_img_path, user_img_url);
                    #endregion

                    #region imgUserProfileFull
                    string user_f_img_path = "/user_profile_image/" + userInfo.image_name_profile + ImageType.imagejpg;
                    string user_f_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                     _appSettings.Value.Photo.UserPhotoPath, userInfo.image_name_profile + ImageType.imagejpg);

                    userInfo.user_image_full = GetImagePhoto(user_f_img_path, user_f_img_url);
                    #endregion
                }
                _conn.Close();
            }
            return userInfo;
        }

        public List<CampaignNew> GetAllCampaign(int pUserID, string deviceType)
        {
            List<CampaignNew> campaignList = new List<CampaignNew>();

            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_all_campaign";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    CampaignNew campaign;
                    foreach (DataRow row in table.Rows)
                    {
                        campaign = new CampaignNew();
                        campaign.loadDataCampaign(row);

                        #region images

                        #region imgShoplogo
                        string shop_logo_img_path = "/company_logo/" + campaign.company_image_name + ImageType.image_logo;

                        string shop_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                  _appSettings.Value.CompanyLogoPathVideo, campaign.company_image_name + ImageType.image_logo);
                        campaign.company_logo_image = GetImagePhoto(shop_logo_img_path, shop_logo_img_url);
                        #endregion

                        //check campaign image type 20170315
                        //if (deviceType.ToLower() == "android")
                        //{
                        //    campaign.campaign_image_type = 1;
                        //}
                        //////////////////////////

                        if (campaign.campaign_image_type == 2)
                        {
                            if (campaign.video_url != "")
                            {
                                #region imgCampaignVideo
                                string hry_video_path = campaign.video_url;
                                string hry_video_url = string.Format(_appSettings.Value.YoutubeVideoURL, campaign.video_url);
                                campaign.campaign_photo_image = GetImagePhoto(hry_video_path, hry_video_url);
                                #endregion
                            }
                            else
                            {
                                #region imgCampaignPhoto
                                string hry_video_path = "/campaign_video/" + campaign.campaign_id + ImageType.video_type;
                                string hry_video_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.CampaignVideoPath, campaign.campaign_id + ImageType.video_type);
                                campaign.campaign_photo_image = GetImagePhoto(hry_video_path, hry_video_url);
                                #endregion
                            }
                        }
                        else if (campaign.campaign_image_type == 1)
                        {
                            #region imgCampaignPhoto
                            string hry_photo_img_path = "/campaign_image/" + campaign.campaign_image_name + ImageType.imagejpg;
                            string hry_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.CampaignPhotoPath, campaign.campaign_image_name + ImageType.imagejpg);
                            campaign.campaign_photo_image = GetImagePhoto(hry_photo_img_path, hry_photo_img_url);
                            
                            #endregion
                        }
                        else if (campaign.campaign_image_type == 3)
                        {
                            #region imgCampaignPhoto
                            string hry_photo_img_path = "/campaign_image/" + campaign.campaign_image_name + ImageType.image_gif;
                            string hry_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.CampaignPhotoPath, campaign.campaign_image_name + ImageType.image_gif);
                            campaign.campaign_photo_image = GetImagePhoto(hry_photo_img_path, hry_photo_img_url);
                            #endregion
                        }

                        #region imgCampaignGallery
                        campaign.campaign_gallery_image = new List<ImagePhoto>();
                        ImagePhoto imgPhoto;
                        for (int i = 1; i <= campaign.campaign_gallery_count; i++)
                        {
                            imgPhoto = new ImagePhoto();

                            string hry_gal_img_path = "/campaign_gallery/" + campaign.campaign_id + "_" + i + ImageType.image_first;
                            string hry_gal_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CampaignGalleryPath, campaign.campaign_id + "_" + i + "_" + campaign.campaign_gallery_no + ImageType.image_first);
                            campaign.campaign_gallery_image.Add(imgPhoto);
                        }
                        #endregion

                        #endregion

                        campaign.question = new List<Question>();
                        if (campaign.is_question)
                        {
                            campaign.question = GetQuestion(campaign.campaign_id);
                        }

                        campaignList.Add(campaign);
                    }
                }
                _conn.Close();
            }
            return campaignList;
        }

        public List<Question> GetQuestion(string pCampaignID)
        {
            List<Question> questionList = new List<Question>();

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_question";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    Question question;
                    foreach (DataRow row in table.Rows)
                    {
                        question = new Question();
                        question.loadDataQuestion(row);

                        questionList.Add(question);
                    }
                }
                _conn.Close();
            }
            return questionList;
        }
        #endregion

        #region JoinOnFeedTab

        public bool CheckGroup(string pCampaignID)
        {
            bool success = false;

            DataTable table = new DataTable();
           
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_check_group";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    success = Convert.ToBoolean(int.Parse(dr["check_group"].ToString()));
                }
                _conn.Close();
                return success;
                
            }
        }
        public bool CheckWinGroup(int pUserID, string pCampaignID)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_check_win_group";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    success = Convert.ToBoolean(int.Parse(dr["check_win_group"].ToString()));
                }
                _conn.Close();
                return success;

            }
        }
        public int CheckFollowAndJoin(int pCompanyID, int pUserID, string pCampaignID)
        {
            int statusJoin = 0;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_check_follow_and_join";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pCompanyID", pCompanyID);
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    statusJoin = int.Parse(dr["status_join"].ToString());
                }
                _conn.Close();
                return statusJoin;

            }
        }
        public DataTable CheckCampaignBeforeJoin(string pCampaignID, int pUserID)
        {
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_check_campaign_before_join";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pUserID", pUserID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
               
                _conn.Close();
                return table;

            }
        }
        public int GetCampaignDigit(string pCampaignID)
        {
            int resultDigit = 0;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_campaign_digit";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    //resultDigit = int.Parse(dr["result_digit"].ToString());
                    resultDigit = Convert.ToInt32(dr["result_digit"] == DBNull.Value ? 0 : dr["result_digit"]);
                }
                _conn.Close();
                return resultDigit;

            }
        }
        public bool CheckJoinNumberRepeated(string pJoinNumber, string pCampaignID)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_check_join_number";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pJoinNumber", pJoinNumber);
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    success = Convert.ToBoolean(int.Parse(dr["is_repeated"].ToString()));
                }
                _conn.Close();
                return success;

            }
        }

        public List<Join> InsertNewUserJoin(string pCampaignID, int pUserID, double pEnjoyNumber, float pLat, float pLng, string verify)
        {
            List<Join> joinList = null;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_insert_user_join";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pEnjoyNumber", pEnjoyNumber);
                cmd.Parameters.AddWithValue("@pLat", pLat);
                cmd.Parameters.AddWithValue("@pLng", pLng);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    joinList = new List<Join>();
                    Join join;
                    CampaignPrize campaignprize;
                    foreach (DataRow row in table.Rows)
                    {
                        join = new Join();
                        join.loadDataJoin(row, pEnjoyNumber);

                        //join.campaign_user_code_id

                        double accum_win_rate = 0;
                        int check_announce = 0;
                        double join_number_rate = Utility.GetRandomNumberDouble(0.0000001, 1);

                        UpdateCPUserCodeNumber(join.id, check_announce, join_number_rate);
                        UpdateVerifyForJoin(join.id, verify);

                        DataTable tableCheckCampaign = GetCampaignPrize(pCampaignID);
                        foreach (DataRow row2 in tableCheckCampaign.Rows)
                        {
                            campaignprize = new CampaignPrize();
                            campaignprize.loadDataPrize(row2);

                            //announce_type >> 1=สุ่มรหัสให้, 2=รอประกาศผล, 3=กำหนดตำแหน่ง
                            if (join.announce_type == 1)
                            {
                                //****
                                accum_win_rate = accum_win_rate + campaignprize.win_rate;

                                if (join_number_rate < accum_win_rate)
                                //if(true)
                                {
                                    bool isPrize = UpdateCurrentWin(join.campaign_id, join.id, campaignprize.prize_id);
                                    if (isPrize)
                                    {
                                        //int user_prize_id = InsertNewUserPrize(join.campaign_id, pUserID, join.campaign_user_code_id, campaignprize.prize_id);
                                        join.announce_text = row["win_text"].ToString();
                                        join.result = campaignprize.prize_order;
                                        break;
                                    }
                                    else
                                    {
                                        join.announce_text = row["lost_text"].ToString();
                                        join.result = 0;
                                    }
                                }
                                else
                                {
                                    join.announce_text = row["lost_text"].ToString();
                                    join.result = 0;
                                }

                            }

                            ///******** flow ผิด ยังไม่สามารถใช้งาน type 3 ได้
                            else if (join.announce_type == 3)
                            {
                                bool check_win_reward = CheckAnnounce(Convert.ToInt32(join.campaign_id), join.count_user);

                                UpdateCPUserCodeNumber(join.id, check_announce, 0);

                                if (check_win_reward)
                                {
                                    bool isPrize = UpdateCurrentWin(join.campaign_id, join.id, campaignprize.prize_id);
                                    if (isPrize)
                                    {
                                        //int user_prize_id = InsertNewUserPrize(join.campaign_id, pUserID, join.campaign_user_code_id, campaignprize.prize_id);
                                        join.announce_text = row["win_text"].ToString();
                                        join.result = campaignprize.prize_order;
                                        break;
                                    }
                                    else
                                    {
                                        join.announce_text = row["lost_text"].ToString();
                                        join.result = 0;
                                    }
                                }
                                else
                                {
                                    join.announce_text = row["lost_text"].ToString();
                                    join.result = 0;
                                }
                            }
                            else
                            {
                                UpdateCPUserCodeNumber(join.id, check_announce, 0);
                                join.announce_text = "รอลุ้นผลรางวัล ในวันที่ " + join.announce_date;
                                join.result = 0;
                                break;
                            }
                        }
                        joinList.Add(join);
                    }
                }
            }
                
            return joinList;
        }
        public Join InsertNewUserJoinNew(string pCampaignID, int pUserID, double pEnjoyNumber, float pLat, float pLng, string verify)
        {
            Join joinList = null;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_insert_user_join";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pEnjoyNumber", pEnjoyNumber);
                cmd.Parameters.AddWithValue("@pLat", pLat);
                cmd.Parameters.AddWithValue("@pLng", pLng);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    joinList = new Join();
                    Join join;
                    CampaignPrize campaignprize;
                    //foreach (DataRow row in table.Rows)
                    //{
                    DataRow row = table.Rows[0];
                        join = new Join();
                        join.loadDataJoin(row, pEnjoyNumber);

                        //join.campaign_user_code_id

                        double accum_win_rate = 0;
                        int check_announce = 0;
                        double join_number_rate = Utility.GetRandomNumberDouble(0.0000001, 1);

                        UpdateCPUserCodeNumber(join.id, check_announce, join_number_rate);
                        UpdateVerifyForJoin(join.id, verify);

                        DataTable tableCheckCampaign = GetCampaignPrize(pCampaignID);
                        //foreach (DataRow row2 in tableCheckCampaign.Rows)
                        //{
                        DataRow row2 = tableCheckCampaign.Rows[0];
                            campaignprize = new CampaignPrize();
                            campaignprize.loadDataPrize(row2);

                            //announce_type >> 1=สุ่มรหัสให้, 2=รอประกาศผล, 3=กำหนดตำแหน่ง
                            if (join.announce_type == 1)
                            {
                                //****
                                accum_win_rate = accum_win_rate + campaignprize.win_rate;

                                if (join_number_rate < accum_win_rate)
                                //if(true)
                                {
                                    bool isPrize = UpdateCurrentWin(join.campaign_id, join.id, campaignprize.prize_id);
                                    if (isPrize)
                                    {
                                        //int user_prize_id = InsertNewUserPrize(join.campaign_id, pUserID, join.campaign_user_code_id, campaignprize.prize_id);
                                        join.announce_text = row["win_text"].ToString();
                                        join.result = campaignprize.prize_order;
                                      
                                    }
                                    else
                                    {
                                        join.announce_text = row["lost_text"].ToString();
                                        join.result = 0;
                                    }
                                }
                                else
                                {
                                    join.announce_text = row["lost_text"].ToString();
                                    join.result = 0;
                                }

                            }

                            ///******** flow ผิด ยังไม่สามารถใช้งาน type 3 ได้
                            else if (join.announce_type == 3)
                            {
                                bool check_win_reward = CheckAnnounce(Convert.ToInt32(join.campaign_id), join.count_user);

                                UpdateCPUserCodeNumber(join.id, check_announce, 0);

                                if (check_win_reward)
                                {
                                    bool isPrize = UpdateCurrentWin(join.campaign_id, join.id, campaignprize.prize_id);
                                    if (isPrize)
                                    {
                                        //int user_prize_id = InsertNewUserPrize(join.campaign_id, pUserID, join.campaign_user_code_id, campaignprize.prize_id);
                                        join.announce_text = row["win_text"].ToString();
                                        join.result = campaignprize.prize_order;
                                       
                                    }
                                    else
                                    {
                                        join.announce_text = row["lost_text"].ToString();
                                        join.result = 0;
                                    }
                                }
                                else
                                {
                                    join.announce_text = row["lost_text"].ToString();
                                    join.result = 0;
                                }
                            }
                            else
                            {
                                UpdateCPUserCodeNumber(join.id, check_announce, 0);
                                join.announce_text = "รอลุ้นผลรางวัล ในวันที่ " + join.announce_date;
                                join.result = 0;
                                
                            }
                        //}
                        joinList=join;
                    //}
                }
            }

            return joinList;
        }
        public bool UpdateCPUserCodeNumber(int pCPUserCodeID, int pCheckAnnounce, double pJoinNumberRate)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_campaign_user_code_number";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCPUserCodeID", pCPUserCodeID);
                cmd.Parameters.AddWithValue("@pCheckAnnounce", pCheckAnnounce);
                cmd.Parameters.AddWithValue("@pJoinNumberRate", pJoinNumberRate);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    success = true;
                }
                _conn.Close();
                return success;

            }
        }

        public bool UpdateVerifyForJoin(int pCampaignUserCodeID, string pVerifyForJoin)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_verify_for_join";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignUserCodeID", pCampaignUserCodeID);
                cmd.Parameters.AddWithValue("@pVerifyForJoin", pVerifyForJoin);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    success = true;
                }
                _conn.Close();
                return success;

            }
        }

        public DataTable GetCampaignPrize(string pCampaignID)
        {
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_campaign_prize_realtime";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                _conn.Close();
                return table;

            }
        }

        public bool UpdateCurrentWin(string pCampaignID, int pCPUserCodeID, int pPrizeID)
        {
            bool success = false;

            DataTable table = new DataTable();
            
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_current_win";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pCPUserCodeID", pCPUserCodeID);
                cmd.Parameters.AddWithValue("@pPrizeID", pPrizeID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    success = Convert.ToBoolean(int.Parse(dr["isPrize"].ToString()));
                }
                _conn.Close();
                return success;

            }
        }
        public bool CheckAnnounce(int pCampaignID, int pCountUser)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_check_announce";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pCountUser", pCountUser);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    success = Convert.ToBoolean(dr["user_announce"]);
                }
                _conn.Close();
                return success;

            }
        }

        #endregion

        #region GetCampaignResult
        public CampaignResult CheckResultAward(int pUserID, string pCampaignID)
        {
            CampaignResult campaignResult = new CampaignResult();

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_check_result_award";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    campaignResult.loadDataCampaignResult(table.Rows[0]);
                }
                _conn.Close();
                return campaignResult;

            }
        }

        public DataTable GetImageResult(string pCampaignID, int pImgResultType)
        {
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_img_result";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pImgResultType", pImgResultType);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                _conn.Close();
                return table;

            }
        }

        public bool UpdateResultImage(int pUserID, string pCampaignID, string pResultImg, string pResultImgName)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_result_image";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pResultImg", pResultImg);
                cmd.Parameters.AddWithValue("@pResultImgName", pResultImgName);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    success = true;
                }
                _conn.Close();
                return success;

            }
        }

        public int InsertNewUserPrize(string pCampaignID, int pUserID, int pCampaignUserCodeID, int pPrizeID)
        {
            int userPrizeID = 0;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_insert_user_prize";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pCampaignUserCodeID", pCampaignUserCodeID);
                cmd.Parameters.AddWithValue("@pPrizeID", pPrizeID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    userPrizeID = int.Parse(dr["user_prize_id"].ToString());
                }
                _conn.Close();
                

            }
            return userPrizeID;
        }

        #endregion

        #region Coment
        public DataTable GetCount(string pCampaignID)
        {
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_count";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                _conn.Close();
                return table;

            }
        }
        public DataTable GetPerPageCampaignComment(string pCampaignID, int pPage, int pPerPage)
        {
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_perpage_campaign_comment";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pPage", pPage);
                cmd.Parameters.AddWithValue("@pPerPage", pPerPage);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                _conn.Close();
                return table;
            }
        }
        public int GetTotalCampaignComment(string pCampaignID)
        {
            int total = 0;
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "get_total_campaign_comment";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }

                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    total = int.Parse(dr["total"].ToString());
                }
                _conn.Close();
                return total;

            }
        }

        public DataTable InsertNewComment(string pCampaignID, int pUserID, string pComment)
        {
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_insert_new_comment";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pComment", pComment);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                _conn.Close();
                return table;
            }
        }
        #endregion

        #region PrizeResultTab
        public List<UserPrize> GetAllPrizeList(int pUserID)
        {
            List<UserPrize> userprizeList = new List<UserPrize>();
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_all_prize_list";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pUserID", pUserID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    UserPrize userprize;
                    foreach (DataRow row in table.Rows)
                    {
                        userprize = new UserPrize();
                        userprize.loadDataUserPrize(row);

                        #region imgShoplogo
                        string shop_logo_img_path = "/company_logo/" + userprize.company_image_name + ImageType.image_logo;
                        string shop_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CompanyLogoPath, "no-image" + ImageType.imagejpg);
                        userprize.company_logo_image = GetImagePhoto(shop_logo_img_path, shop_logo_img_url);
                        #endregion

                        userprizeList.Add(userprize);
                    }
                }
                _conn.Close();
                return userprizeList;

            }
        }

        public PrizeDetailModel GetPrizeDetail(int pUserPrizeID)
        {
            PrizeDetailModel prizeDetail = new PrizeDetailModel();

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_prize_detail";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pUserPrizeID", pUserPrizeID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    prizeDetail.loadDataPrizeDetail(table.Rows[0]);

                    #region imgCampaignPhoto
                    string hry_photo_img_path = "/campaign_image/" + prizeDetail.campaign_image_name + ImageType.imagejpg;
                    string hry_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                _appSettings.Value.CampaignPhotoPath, prizeDetail.campaign_image_name + ImageType.imagejpg);

                    prizeDetail.campaign_photo_image = GetImagePhoto(hry_photo_img_path, hry_photo_img_url);
                    #endregion

                    #region imgCampaignBackground
                    string hry_background_img_path = "/campaign_background/" + prizeDetail.bg_image_name + ImageType.imagejpg;
                    string hry_background_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CampaignBackgroundPath, prizeDetail.bg_image_name + ImageType.imagejpg);
                    prizeDetail.campaign_background_image = GetImagePhoto(hry_background_img_path, hry_background_img_url);
                    #endregion
                }
                _conn.Close();
                return prizeDetail;

            }
        }

        public bool DeleteUserPrize(int pUserID, int pUserPrizeID)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_delete_user_prize";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pUserPrizeID", pUserPrizeID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    success = true;
                }
               
                _conn.Close();
                return success;

            }
           
        }


        #endregion

        #region User
        public List<UserResult> GetAllResult(int pUserID, int pJoin, int pAnnouncetype, string deviceType)
        {
            List<UserResult> userResultList = new List<UserResult>();
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_all_result";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pJoin", pJoin);
                cmd.Parameters.AddWithValue("@pAnnouncetype", pAnnouncetype);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    UserResult userResult;
                    List<SmsResult> smsResultList;
                    foreach (DataRow row in table.Rows)
                    {
                        userResult = new UserResult();
                        userResult.loadDataUserResult(row);

                        smsResultList = new List<SmsResult>();

                        smsResultList = GetAllSmsResult(pUserID, userResult.campaign_id.ToString(), pJoin);
                        userResult.sms_result = smsResultList;

                        #region imgCompanylogo

                        string shop_logo_img_path = "/company_logo/" + userResult.company_image_name + ImageType.image_logo;
                        
                        string shop_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CompanyLogoPath, userResult.company_image_name + ImageType.image_logo);
                        userResult.company_logo_image = GetImagePhoto(shop_logo_img_path, shop_logo_img_url);
                        #endregion

                        //check campaign image type 20170315
                        //if (deviceType.ToLower() == "android")
                        //{
                        //    userResult.campaign_image_type = 1;
                        //}
                        ////////////////////////////////

                        if (userResult.campaign_image_type == 2)
                        {
                            #region imgCampaignVideo
                            string hry_video_path = "/campaign_video/" + userResult.campaign_id + ImageType.video_type;
                            //string cpn_video_url = string.Format(WebConfigurationManager.AppSettings["campaign_video_path"], WebConfigurationManager.AppSettings["resource_ip"], userResult.campaign_id + ImageType.video_type);
                            string hry_video_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CampaignVideoPath, userResult.campaign_id + ImageType.video_type);

                            userResult.campaign_photo_image = GetImagePhoto(hry_video_path, hry_video_url);
                            #endregion
                        }
                        else if (userResult.campaign_image_type == 3)
                        {
                            #region imgCampaignPhoto
                            string hry_photo_img_path = "/campaign_image/" + userResult.campaign_image_name + ImageType.image_gif;
                            string hry_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.CampaignPhotoPath, userResult.campaign_image_name + ImageType.image_gif);
                            userResult.campaign_photo_image = GetImagePhoto(hry_photo_img_path, hry_photo_img_url);
                            #endregion
                        }
                        else
                        {
                            if (userResult.campaign_type == 3)
                            {
                                #region imgCampaignDealGallery
                                string hry_gal_img_path = "/deal_photo/" + userResult.deal_id + "_" + 1 + ImageType.imagejpg;
                                //string cpn_gal_img_url = string.Format(WebConfigurationManager.AppSettings["deal_photo_path"], WebConfigurationManager.AppSettings["resource_ip"], userResult.deal_id + "_" + 1 + ImageType.imagejpg);
                                string hry_gal_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.DealPhotoPath, userResult.deal_id + "_" + 1 + ImageType.imagejpg);

                                userResult.campaign_photo_image = GetImagePhoto(hry_gal_img_path, hry_gal_img_url);
                                #endregion
                            }
                            else
                            {
                                //#region imgCampaignPhoto
                                //string cpn_photo_img_path = "/campaign_image/" + userResult.campaign_image_name + ImageType.imagejpg;
                                //string cpn_photo_img_url = string.Format(WebConfigurationManager.AppSettings["campaign_photo_path"], WebConfigurationManager.AppSettings["resource_ip"], userResult.campaign_image_name + ImageType.imagejpg);
                                //userResult.campaign_photo_image = GetImagePhoto(cpn_photo_img_path, cpn_photo_img_url);
                                //#endregion
                                #region imgCampaignPhoto
                                string hry_photo_img_path = "/campaign_image/" + userResult.campaign_image_name + ImageType.imagejpg;
                                string hry_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                            _appSettings.Value.CampaignPhotoPath, userResult.campaign_image_name + ImageType.imagejpg);
                                userResult.campaign_photo_image = GetImagePhoto(hry_photo_img_path, hry_photo_img_url);
                                #endregion
                            }
                        }

                        userResultList.Add(userResult);
                    }
                }
                _conn.Close();
                return userResultList;
            }
        }
        public List<SmsResult> GetAllSmsResult(int pUserID, string pCampaignID, int joinStatus)
        {
            List<SmsResult> smsResultList = new List<SmsResult>();

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_all_sms_result";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    SmsResult smsResult;

                    foreach (DataRow row in table.Rows)
                    {
                        smsResult = new SmsResult();
                        if (joinStatus == 1)
                        {
                            smsResult.sms_text = "Joined";
                        }
                        string url = string.Format("{0}/{1}", _appSettings.Value.BaseUrl,
                                         _appSettings.Value.Photo.SmsPhotoPath);

                        smsResult.loadDataSmsResult(row , url);

                        #region imgUserProfileRound
                        string user_r_img_path = "/user_profile_image/" + smsResult.image_name_profile + ImageType.image_round;
                        string user_r_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                _appSettings.Value.Photo.UserPhotoPath, smsResult.image_name_profile + ImageType.image_round);

                        smsResult.user_image_round = GetImagePhoto(user_r_img_path, user_r_img_url);
                        #endregion

                        #region imgUserProfileFull
                        string user_f_img_path = "/user_profile_image/" + smsResult.image_name_profile + ImageType.imagejpg;
                        //string user_f_img_url = string.Format(WebConfigurationManager.AppSettings["user_photo_path"], WebConfigurationManager.AppSettings["resource_ip"], smsResult.image_name_profile + ImageType.imagejpg);
                        string user_f_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                _appSettings.Value.Photo.UserPhotoPath, smsResult.image_name_profile + ImageType.imagejpg);
                        smsResult.user_image_full = GetImagePhoto(user_f_img_path, user_f_img_url);
                        #endregion

                        smsResultList.Add(smsResult);
                    }
                }
                _conn.Close();
                return smsResultList;
            }
        }
        public int CountPrizeBadge(int pUserID)
        {
            int prize_badge = 0;


            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_count_prize_badge";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pUserID", pUserID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    //prize_badge = int.Parse(dr["prize_badge"].ToString());
                    prize_badge = dr["prize_badge"] == DBNull.Value ? 0 : Convert.ToInt32(dr["prize_badge"].ToString());
                }

                _conn.Close();
                return prize_badge;

            }
        }
        #endregion

        #region Company
        public List<CompanyDetailModel> GetAllFollowCompany(int pFollow, int pUserID)
        {
            List<CompanyDetailModel> companyList = new List<CompanyDetailModel>();

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_all_follow_company";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pFollow", pFollow);
                cmd.Parameters.AddWithValue("@pUserID", pUserID);


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    CompanyDetailModel company;
                    foreach (DataRow row in table.Rows)
                    {
                        company = new CompanyDetailModel();
                        company.loadDataCompanyFollow(row);

                        #region imgCompanylogo
                        string company_logo_img_path = "/company_logo/" + company.company_image_name + ImageType.image_logo;

                        string company_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CompanyLogoPathVideo, company.company_image_name + ImageType.image_logo);
                        company.company_logo_image = GetImagePhoto(company_logo_img_path, company_logo_img_url);
                        #endregion

                        companyList.Add(company);
                    }
                }
                _conn.Close();
                return companyList;
            }
        }

        public List<FollowModel> GetAllCampaignCompanyPage(int pCompanyID, int pUserID, int pPage, int pPerPage)
        {
            List<FollowModel> followList = new List<FollowModel>();

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_all_campaign_company_page";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCompanyID", pCompanyID);
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pPage", pPage);
                cmd.Parameters.AddWithValue("@pPerPage", pPerPage);


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    FollowModel follow;
                    foreach (DataRow row in table.Rows)
                    {
                        follow = new FollowModel();
                        follow.loadDataFollow(row, pUserID);

                        #region imgCompanylogo
                        //string shop_logo_img_path = "/shop_logo/" + follow.shop_image_name + ImageType.image_logo;
                        //string shop_logo_img_url = string.Format(WebConfigurationManager.AppSettings["shop_logo_path"], WebConfigurationManager.AppSettings["resource_ip"], follow.shop_image_name + ImageType.image_logo);
                        //follow.shop_logo_image = GetImagePhoto(shop_logo_img_path, shop_logo_img_url);

                        string company_logo_img_path = "/company_logo/" + follow.company_image_name + ImageType.image_logo;

                        string company_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CompanyLogoPath, follow.company_image_name + ImageType.image_logo);
                        follow.company_logo_image = GetImagePhoto(company_logo_img_path, company_logo_img_url);
                        #endregion

                        #region imgCampaignGallery
                        follow.campaign_gallery_image = new List<ImagePhoto>();
                        ImagePhoto imgPhoto;
                        for (int i = 1; i <= follow.campaign_gallery_count; i++)
                        {
                            imgPhoto = new ImagePhoto();

                            string hry_gal_img_path = "/campaign_gallery/" + follow.campaign_id + "_" + i + ImageType.image_first;
                            string hry_gal_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CampaignGalleryPath, follow.campaign_id + "_" + i + "_" + follow.campaign_gallery_no + ImageType.image_first);
                            follow.campaign_gallery_image.Add(imgPhoto);
                        }
                        #endregion

                        //check campaign image type 20170315
                        //if (deviceType.ToLower() == "android")
                        //{
                        //    follow.campaign_image_type = 1;
                        //}
                        ///////////////////////

                        if (follow.campaign_image_type == 2)
                        {
                            #region imgCampaignPhoto
                            //string cpn_video_path = "/campaign_video/" + follow.campaign_id + ImageType.video_type;
                            //string cpn_video_url = string.Format(WebConfigurationManager.AppSettings["campaign_video_path"], WebConfigurationManager.AppSettings["resource_ip"], follow.campaign_id + ImageType.video_type);
                            //follow.campaign_photo_image = GetImagePhoto(cpn_video_path, cpn_video_url);

                            string hry_video_path = "/campaign_video/" + follow.campaign_id + ImageType.video_type;
                            string hry_video_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CampaignVideoPath, follow.campaign_id + ImageType.video_type);
                            follow.campaign_photo_image = GetImagePhoto(hry_video_path, hry_video_url);
                            #endregion
                        }
                        else if (follow.campaign_image_type == 1)
                        {
                            #region imgCampaignPhoto
                            //string cpn_photo_img_path = "/campaign_image/" + follow.campaign_image_name + ImageType.imagejpg;
                            //string cpn_photo_img_url = string.Format(WebConfigurationManager.AppSettings["campaign_photo_path"], WebConfigurationManager.AppSettings["resource_ip"], follow.campaign_image_name + ImageType.imagejpg);
                            //follow.campaign_photo_image = GetImagePhoto(cpn_photo_img_path, cpn_photo_img_url);

                            string hry_photo_img_path = "/campaign_image/" + follow.campaign_image_name + ImageType.imagejpg;
                            string hry_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.CampaignPhotoPath, follow.campaign_image_name + ImageType.imagejpg);
                            follow.campaign_photo_image = GetImagePhoto(hry_photo_img_path, hry_photo_img_url);
                            #endregion
                        }
                        else if (follow.campaign_image_type == 3)
                        {
                            #region imgCampaignPhoto
                            //string cpn_photo_img_path = "/campaign_image/" + follow.campaign_image_name + ImageType.image_gif;
                            //string cpn_photo_img_url = string.Format(WebConfigurationManager.AppSettings["campaign_photo_path"], WebConfigurationManager.AppSettings["resource_ip"], follow.campaign_image_name + ImageType.image_gif);
                            //follow.campaign_photo_image = GetImagePhoto(cpn_photo_img_path, cpn_photo_img_url);

                            string hry_photo_img_path = "/campaign_image/" + follow.campaign_image_name + ImageType.image_gif;
                            string hry_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.CampaignPhotoPath, follow.campaign_image_name + ImageType.image_gif);
                            follow.campaign_photo_image = GetImagePhoto(hry_photo_img_path, hry_photo_img_url);
                            #endregion
                        }

                        follow.question = new List<Question>();
                        if (follow.is_question)
                        {
                            follow.question = GetQuestion(follow.campaign_id);
                        }

                        followList.Add(follow);
                    }

                }
                _conn.Close();
                return followList;
            }
        }

        public int InsertNewNotificationFollowDetail(int pUserID, int pCompanyID)
        {
            int notificationID = 0;


            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_insert_new_notification_follow_detail";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pCompanyID", pCompanyID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    notificationID = dr["new_notification_id"]==DBNull.Value ?0:Convert.ToInt32(dr["new_notification_id"]);
                }
                _conn.Close();
            }

            return notificationID;
        }

        public int GetTotalCompanyFollowCampaign(int pCompanyID, int pUserID)
        {
            int total = 0;


            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "get_total_company_follow_campaign";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pCompanyID", pCompanyID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    total = dr["total"] == DBNull.Value ? 0 : Convert.ToInt32(dr["total"]);
                }
                _conn.Close();
            }

            return total;
        }

        public List<FollowModel> GetPerPageCompanyFollowCampaign(int pCompanyID, int pUserID, int pPage, int pPerPage)
        {

            List<FollowModel> followList = new List<FollowModel>();

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_perpage_company_follow_campaign";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCompanyID", pCompanyID);
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pPage", pPage);
                cmd.Parameters.AddWithValue("@pPerPage", pPerPage);


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    FollowModel follow;
                    foreach (DataRow row in table.Rows)
                    {
                        follow = new FollowModel();
                        follow.loadDataFollow(row, pUserID);

                        #region imgCompanylogo
                      
                        string company_logo_img_path = "/company_logo/" + follow.company_image_name + ImageType.image_logo;

                        string company_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CompanyLogoPath, follow.company_image_name + ImageType.image_logo);
                        follow.company_logo_image = GetImagePhoto(company_logo_img_path, company_logo_img_url);
                        #endregion

                        #region imgCampaignGallery
                        follow.campaign_gallery_image = new List<ImagePhoto>();
                        ImagePhoto imgPhoto;
                        for (int i = 1; i <= follow.campaign_gallery_count; i++)
                        {
                            imgPhoto = new ImagePhoto();

                            string hry_gal_img_path = "/campaign_gallery/" + follow.campaign_id + "_" + i + ImageType.image_first;
                            string hry_gal_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CampaignGalleryPath, follow.campaign_id + "_" + i + "_" + follow.campaign_gallery_no + ImageType.image_first);
                            follow.campaign_gallery_image.Add(imgPhoto);
                        }
                        #endregion

           

                        if (follow.campaign_image_type == 2)
                        {
                            #region imgCampaignPhoto
                            //string cpn_video_path = "/campaign_video/" + follow.campaign_id + ImageType.video_type;
                            //string cpn_video_url = string.Format(WebConfigurationManager.AppSettings["campaign_video_path"], WebConfigurationManager.AppSettings["resource_ip"], follow.campaign_id + ImageType.video_type);
                            //follow.campaign_photo_image = GetImagePhoto(cpn_video_path, cpn_video_url);

                            string hry_video_path = "/campaign_video/" + follow.campaign_id + ImageType.video_type;
                            string hry_video_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                    _appSettings.Value.CampaignVideoPath, follow.campaign_id + ImageType.video_type);
                            follow.campaign_photo_image = GetImagePhoto(hry_video_path, hry_video_url);
                            #endregion
                        }
                        else if (follow.campaign_image_type == 1)
                        {
                            #region imgCampaignPhoto
                            //string cpn_photo_img_path = "/campaign_image/" + follow.campaign_image_name + ImageType.imagejpg;
                            //string cpn_photo_img_url = string.Format(WebConfigurationManager.AppSettings["campaign_photo_path"], WebConfigurationManager.AppSettings["resource_ip"], follow.campaign_image_name + ImageType.imagejpg);
                            //follow.campaign_photo_image = GetImagePhoto(cpn_photo_img_path, cpn_photo_img_url);

                            string hry_photo_img_path = "/campaign_image/" + follow.campaign_image_name + ImageType.imagejpg;
                            string hry_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.CampaignPhotoPath, follow.campaign_image_name + ImageType.imagejpg);
                            follow.campaign_photo_image = GetImagePhoto(hry_photo_img_path, hry_photo_img_url);
                            #endregion
                        }
                        else if (follow.campaign_image_type == 3)
                        {
                            #region imgCampaignPhoto
                            //string cpn_photo_img_path = "/campaign_image/" + follow.campaign_image_name + ImageType.image_gif;
                            //string cpn_photo_img_url = string.Format(WebConfigurationManager.AppSettings["campaign_photo_path"], WebConfigurationManager.AppSettings["resource_ip"], follow.campaign_image_name + ImageType.image_gif);
                            //follow.campaign_photo_image = GetImagePhoto(cpn_photo_img_path, cpn_photo_img_url);

                            string hry_photo_img_path = "/campaign_image/" + follow.campaign_image_name + ImageType.image_gif;
                            string hry_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.CampaignPhotoPath, follow.campaign_image_name + ImageType.image_gif);
                            follow.campaign_photo_image = GetImagePhoto(hry_photo_img_path, hry_photo_img_url);
                            #endregion
                        }

                        follow.question = new List<Question>();
                        if (follow.is_question)
                        {
                            follow.question = GetQuestion(follow.campaign_id);
                        }

                        followList.Add(follow);
                    }

                }
                _conn.Close();
                return followList;
            }
        }

        public CompanyDetail GetCompanyDetail(int pCompanyID, int pUserID)
        {

            CompanyDetail shopDetail = new CompanyDetail();

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_get_company_detail";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCompanyID", pCompanyID);
                cmd.Parameters.AddWithValue("@pUserID", pUserID);


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];

                    shopDetail.loadDataFollowCompanyDetail(row);

                    #region imgCompanylogo

                    string company_logo_img_path = "/company_logo/" + shopDetail.company_image_name + ImageType.image_logo;

                    string company_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                _appSettings.Value.CompanyLogoPath, shopDetail.company_image_name + ImageType.image_logo);
                    shopDetail.company_logo_image = GetImagePhoto(company_logo_img_path, company_logo_img_url);
                    #endregion

                    #region imgCampaignGallery
                    shopDetail.company_gallery_image = new List<ImagePhoto>();
                    ImagePhoto imgPhoto;
                    for (int i = 1; i <= shopDetail.company_gallery_image.Count; i++)
                    {
                        imgPhoto = new ImagePhoto();

                        string hry_gal_img_path = "/company_gallery/" + shopDetail.company_id + "_" + i + ImageType.image_first;
                        string hry_gal_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                _appSettings.Value.CampaignGalleryPath, shopDetail.company_id + "_" + i + "_" + shopDetail.company_gallery_no + ImageType.image_first);
                        shopDetail.company_gallery_image.Add(imgPhoto);
                    }
                    #endregion

                    }
                }
                _conn.Close();
                return shopDetail;
        }

        #endregion

        #region Update
        public bool UpdateUserAddress(int pUserID, string pAddress, string pDistrict, string pAmphor, string pProvince, string pZipcode)
        {
            bool success = false;
            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_user_address";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pAddress", pAddress);
                cmd.Parameters.AddWithValue("@pDistrict", pDistrict);
                cmd.Parameters.AddWithValue("@pAmphor", pAmphor);
                cmd.Parameters.AddWithValue("@pProvince", pProvince);
                cmd.Parameters.AddWithValue("@pZipcode", pZipcode);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    success = true;
                }
                _conn.Close();
            }

            return success;
        }

        public UserInfoForEditModel UpdateUserInformation(int pUserID, string pFirstName, string pLastName, string pDisplayName, string pEmail, string pBirthday, string pGender, string pMobile, string pFFirstName, string pFLastName, string pFDisplayName, string pFEmail, string pFBirthday, string pFGender, string pFMobile, string pFacebookID, string pImageName, int pIsPhoto)
        {
            UserInfoForEditModel newUserInfo = null;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_user_profile2";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pFirstName", pFirstName);
                cmd.Parameters.AddWithValue("@pLastName", pLastName);
                cmd.Parameters.AddWithValue("@pDisplayName", pDisplayName);
                cmd.Parameters.AddWithValue("@pEmail", pEmail);

                DateTime paramBirthday = DateTime.Now;
                bool canPares = DateTime.TryParseExact(pBirthday, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out paramBirthday);
                if (canPares)
                {
                    cmd.Parameters.AddWithValue("@pBirthday", paramBirthday);
                }
                
                cmd.Parameters.AddWithValue("@pGender", pGender);
                cmd.Parameters.AddWithValue("@pMobile", pMobile);
                cmd.Parameters.AddWithValue("@pFFirstName", pFFirstName);
                cmd.Parameters.AddWithValue("@pFLastName", pFLastName);
                cmd.Parameters.AddWithValue("@pFDisplayName", pFDisplayName);
                cmd.Parameters.AddWithValue("@pFEmail", pFEmail);

                DateTime paramFBirthday = DateTime.Now;
                bool canPares2 = DateTime.TryParseExact(pBirthday, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.AssumeLocal, out paramFBirthday);
                if (canPares2)
                {
                    cmd.Parameters.AddWithValue("@pFBirthday", paramFBirthday);
                }

               
                cmd.Parameters.AddWithValue("@pFGender", pFGender);
                cmd.Parameters.AddWithValue("@pFMobile", pFMobile);
                cmd.Parameters.AddWithValue("@pFacebookID", pFacebookID);
                cmd.Parameters.AddWithValue("@pImageName", pImageName);
                cmd.Parameters.AddWithValue("@pIsPhoto", pIsPhoto);


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    newUserInfo = new UserInfoForEditModel();
                    newUserInfo.loadDataUserForEdit(table.Rows[0]);

                    #region imgUserProfileRound

                    string user_r_img_path = "/user_profile_image/" + newUserInfo.image_name_profile + ImageType.image_round;
                    string user_r_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                       _appSettings.Value.Photo.UserPhotoPath, newUserInfo.image_name_profile + ImageType.image_round);
                    newUserInfo.user_image_round = GetImagePhoto(user_r_img_path, user_r_img_url);
                 
                    #endregion

                    #region imgUserProfileRoundFull
                    string user_f_img_path = "/user_profile_image/" + newUserInfo.image_name_profile + ImageType.image_round_full;
                    string user_f_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                     _appSettings.Value.Photo.UserPhotoPath, newUserInfo.image_name_profile + ImageType.image_round_full);

                    newUserInfo.user_image_full = GetImagePhoto(user_f_img_path, user_f_img_url);
                    #endregion

                    #region imgUserProfile
                    string user_img_path = "/user_profile_image/" + newUserInfo.image_name_profile + ImageType.image_thumbnail;
                    string user_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                     _appSettings.Value.Photo.UserPhotoPath, newUserInfo.image_name_profile + ImageType.image_thumbnail);
                    newUserInfo.user_image = GetImagePhoto(user_img_path, user_img_url);
                    #endregion

                    #region imgUserProfileFull
                    string user_f_img_path2 = "/user_profile_image/" + newUserInfo.image_name_profile + ImageType.imagejpg;
                    string user_f_img_url2 = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                     _appSettings.Value.Photo.UserPhotoPath, newUserInfo.image_name_profile + ImageType.imagejpg);

                    newUserInfo.user_image_full = GetImagePhoto(user_f_img_path2, user_f_img_url2);
                    #endregion
                }
            }
            _conn.Close();
            return newUserInfo;
        }

        public bool VerifyDeviceToken(int pUserID, string pDeviceToken)
        {
            bool success = false;
            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_verify_device_token";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pDeviceToken", pDeviceToken);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    success = Convert.ToBoolean(row["is_valid"]);
                }
                _conn.Close();
            }

            return success;
        }

        public bool UpdateUserNotification(int pUserID, int pStatus)
        {
            bool success = false;
            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_user_notification";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pStatus", pStatus);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    success = Convert.ToBoolean(int.Parse(dr["is_notification"].ToString()));
                }
                _conn.Close();
            }

            return success;
        }
        public string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        public bool UpdateRePrizeOTP(int pUserID, string pOtp)
        {
            bool success = false;
            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_user_reprize_otp";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pOtp", pOtp);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    success = Convert.ToBoolean(int.Parse(dr["is_user"].ToString()));
                }
                _conn.Close();
            }

            return success;
        }

        public OldUserModel UpdateRestorePrize(int pUserID, string pFacebookID, string pOtpCode)
        {
            OldUserModel old_user = new OldUserModel();

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_update_restore_prize";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pFacebookID", pFacebookID);
                cmd.Parameters.AddWithValue("@pOtpCode", pOtpCode);
               
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    old_user.loadDataOldUser(table.Rows[0]);
                }
                
            }
            _conn.Close();
            return old_user;
        }

        public int InsertLogPush(string pPushName, string pMobileType, int pCampaignID)
        {
            int logID = 0;
            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "insert_log_push";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pPushName", pPushName);
                cmd.Parameters.AddWithValue("@pMobileType", pMobileType);
                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    logID = dr["log_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["log_id"]);
                }
                _conn.Close();
            }

            return logID;
        }

        public void createPushFile(string message, string type, int badge, string targetDeviceToken, string deviceType, List<KeyValuePair<string, string>> extendParameter, int round_number)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + round_number.ToString();
            string filePath = _appSettings.Value.PATH_PUSH_FILE.ToString();
            string delimeter = @"|##$%|";
            string equal = @"|==$%|";
            string separator = @"|::$%|";
            string text = targetDeviceToken + delimeter + message + delimeter + "1" + delimeter + type + delimeter + deviceType + delimeter;
            if (extendParameter != null && extendParameter.Count() > 0)
            {
                KeyValuePair<string, string> dict;
                int count = extendParameter.Count();
                int i = 0;

                do
                {
                    dict = extendParameter[i];
                    text += dict.Key + equal + dict.Value;

                    text += i++ < count ? separator : "";
                } while (i < count);
            }
            System.IO.File.WriteAllText(filePath + @"\\" + fileName, text);
        }

        public bool InsertFeedback(int pUserID, string pFeedback)
        {
            bool success = false;
            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_insert_new_feedback";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pFeedback", pFeedback);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    success = true;
                }
                _conn.Close();
            }

            return success;
        }

        public List<StoreConfigModel> GetStoreConfig(string pMarket)
        {
            List<StoreConfigModel> storeConfigList = new List<StoreConfigModel>();
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "get_store_config";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pMarket", pMarket);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    StoreConfigModel storeConfig;
                    foreach (DataRow row in table.Rows)
                    {
                        storeConfig = new StoreConfigModel();
                        storeConfig.loadDataStoreConfig(row);

                        storeConfigList.Add(storeConfig);
                    }
                }
                _conn.Close();
            }
            return storeConfigList;
        }
        #endregion

        #region User
        public bool CreatePassword(int userId, string passWord)
        {
            var result = false;
            var user = _context.HryUserProfile.Where(s => s.UserId == userId).FirstOrDefault();
            if (user != null )
            {
                user.Password = ComputeSha256Hash(passWord);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }

        public Register GetUser(int pUserID)
        {
            Register result = new Register();
            result = null;
            var user = _context.HryUserProfile.Where(s => s.UserId == pUserID).FirstOrDefault();
            if (user != null)
            {
                loadDataRegisterModel model = new loadDataRegisterModel() 
                {
                     display_birthday = user.DisplayBirthday.ToString(),
                     user_id = user.UserId,
                     display_show_name = user.DisplayShowName,
                    display_email = user.DisplayEmail,
                    display_fname = user.DisplayFname,
                    display_lname = user.DisplayLname,

                    display_gender = user.DisplayGender,
                     display_mobile = user.DisplayMobile,
                    user_lang = user.UserLang,
                   is_search_friend = user.IsSearchFriend == null ?false:Convert.ToBoolean(user.IsSearchFriend),
                    require_mobile_verify = user.RequireMobileVerify == null ? false : Convert.ToBoolean(user.RequireMobileVerify),
                    image_name_profile = user.ImageNameProfile,
                   
                };
                result = new Register();
                result.loadDataRegister(model);
                DateTime dt = DateTime.Now;
                string imgname = string.Format("{0}_{1:yyyyMMddHHmmss}", result.user_id, dt);
                UpdateUserProfile(result.user_id, imgname);

                string img = imgname;

                #region imgUserProfileRound
                string user_r_img_path = "/user_profile_image/" + img + ImageType.image_round;
                string user_r_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                   _appSettings.Value.Photo.UserPhotoPath, img + ImageType.image_round);
                result.user_image_round = GetImagePhoto(user_r_img_path, user_r_img_url);
                #endregion

                #region imgUserProfileFull
                string user_f_img_path = "/user_profile_image/" + img + ImageType.imagejpg;
                string user_f_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                 _appSettings.Value.Photo.UserPhotoPath, img + ImageType.imagejpg);

                result.user_image_full = GetImagePhoto(user_f_img_path, user_f_img_url);
                #endregion


            }
            return result;
        }

        public Register Login(LoginModel model)
        {
            Register result = new Register();
            result = null;
            var pass = ComputeSha256Hash(model.password);
            var user = _context.HryUserProfile.Where(s => s.DisplayMobile == model.mobile&& s.Password == pass).FirstOrDefault();
            if (user != null)
            {
                loadDataRegisterModel models = new loadDataRegisterModel()
                {
                    display_birthday = user.DisplayBirthday.ToString(),
                    user_id = user.UserId,
                    display_show_name = user.DisplayShowName,
                    display_email = user.DisplayEmail,
                    display_fname = user.DisplayFname,
                    display_lname = user.DisplayLname,

                    display_gender = user.DisplayGender,
                    display_mobile = user.DisplayMobile,
                    user_lang = user.UserLang,
                    is_search_friend = user.IsSearchFriend == null ? false : Convert.ToBoolean(user.IsSearchFriend),
                    require_mobile_verify = user.RequireMobileVerify == null ? false : Convert.ToBoolean(user.RequireMobileVerify),
                    image_name_profile = user.ImageNameProfile,

                };
                result = new Register();
                result.loadDataRegister(models);
                DateTime dt = DateTime.Now;
                string imgname = string.Format("{0}_{1:yyyyMMddHHmmss}", result.user_id, dt);
                UpdateUserProfile(result.user_id, imgname);

                string img = imgname;

                #region imgUserProfileRound
                string user_r_img_path = "/user_profile_image/" + img + ImageType.image_round;
                string user_r_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                   _appSettings.Value.Photo.UserPhotoPath, img + ImageType.image_round);
                result.user_image_round = GetImagePhoto(user_r_img_path, user_r_img_url);
                #endregion

                #region imgUserProfileFull
                string user_f_img_path = "/user_profile_image/" + img + ImageType.imagejpg;
                string user_f_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                 _appSettings.Value.Photo.UserPhotoPath, img + ImageType.imagejpg);

                result.user_image_full = GetImagePhoto(user_f_img_path, user_f_img_url);
                #endregion


            }
            return result;
        }

        public bool CheckMobile(string moBile)
        {
            var result = false;
            var user = _context.HryUserProfile.Where(s => s.DisplayMobile == moBile && s.MobileVerifyFlag ==1).FirstOrDefault();
            if (user != null)
            {
                result = true;
            }
            return result;
        }

        public HryUserProfile GetUserMobile(string mobile)
        {
            HryUserProfile hryUser = new HryUserProfile();
            hryUser = null;
            var user = _context.HryUserProfile.Where(s => s.DisplayMobile == mobile).FirstOrDefault();
            if (user != null)
            {
                hryUser = new HryUserProfile();
                hryUser = user;
            }
            return hryUser;
        }
        public bool CheckDateOtp( string mobile)
        {
            var date = DateTime.Now;
            var HryOtpDetail = _context.HryOtpDetail.Where(s => s.MobileNo == mobile && s.OtpType == 1 && s.OtpVerify == true && s.OtpSendDate.Value.Day == DateTime.Now.Day && s.OtpSendDate.Value.Month == DateTime.Now.Month && s.OtpSendDate.Value.Year == DateTime.Now.Year).ToList();
            if (HryOtpDetail.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckOtp(VerifyPasswordModel model)
        {
            var result = false;
            var data = _context.HryOtpDetail.Where(s => s.MobileNo == model.mobile && s.OtpType == 1 && s.OtpVerify == true && s.OtpNumber == model.verifycode && s.OtpSendDate.Value.Day == DateTime.Now.Day && s.OtpSendDate.Value.Month == DateTime.Now.Month && s.OtpSendDate.Value.Year == DateTime.Now.Year).FirstOrDefault();
            if (data != null)
            {
                result = true;
            }
            return result;
        }
        public bool InsertOtpDetail(int otpnumber, string detail , string mobile , out int number)
        {
            number = 0;
            var HryOtpDetail = _context.HryOtpDetail.Where(s => s.MobileNo == mobile && s.OtpType == 1 && s.OtpVerify == true).ToList();
            if (HryOtpDetail.Count > 0)
            {
                return false;
            }
            var expired = _context.HryOtpDetail.Where(s => s.MobileNo == mobile && s.OtpType == 1 && s.OtpVerify == false && s.OtpSendDate.Value.Day == DateTime.Now.Day && s.OtpSendDate.Value.Month == DateTime.Now.Month && s.OtpSendDate.Value.Year == DateTime.Now.Year).FirstOrDefault();
            if (expired != null)
            {
                number = (int)expired.OtpNumber;
                return true;
            }
            else
            {
                HryOtpDetail hryOtpDetail = new HryOtpDetail();
                hryOtpDetail.OtpNumber = otpnumber;
                hryOtpDetail.OtpDetail = detail;
                hryOtpDetail.MobileNo = mobile;
                hryOtpDetail.OtpVerify = false;
                hryOtpDetail.OtpSendDate = DateTime.Now;
                hryOtpDetail.OtpVerifyDate = null;
                hryOtpDetail.OtpType = 1;


                _context.HryOtpDetail.Add(hryOtpDetail);
                _context.SaveChanges();
                return true;
            }

        }

        public bool VerifyForgotPassword(VerifyPasswordModel model)
        {
            var result = false;
            var data = _context.HryOtpDetail.Where(s => s.MobileNo == model.mobile && s.OtpType ==1 && s.OtpVerify == false && s.OtpNumber== model.verifycode && s.OtpSendDate.Value.Day == DateTime.Now.Day && s.OtpSendDate.Value.Month == DateTime.Now.Month && s.OtpSendDate.Value.Year == DateTime.Now.Year).FirstOrDefault();
            if (data != null)
            {
                data.OtpVerify = true;
                data.OtpVerifyDate = DateTime.Now;
                _context.SaveChanges();
                result = true;
            }
            return result;
        }
        public string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        #endregion

        #region PageFeedResource
        public int InsertLogReceiveDataNew(string pServiceName, string pReceiveData, string pTimeStamp)
        {
            int id = 0;

            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "insert_log_receive_data";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pServiceName", pServiceName);
                cmd.Parameters.AddWithValue("@pReceiveData", pReceiveData);
                cmd.Parameters.AddWithValue("@pTimeStamp", pTimeStamp);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    id = dr["id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["id"]);
                }
                _conn.Close();
                return id;
            }
        }

        public bool InsertQuestionResult(int pUserID, int pQuestionID, int pCampaignUserCodeID, string pAnswer01, string pAnswer02, string pAnswer03, string pAnswer04, string pAnswer05, string pAnswer06, string pAnswer07, string pAnswer08, string pAnswer09, string pAnswer10, string pAnswerOption)
        {
            bool success = false;

            DataTable table = new DataTable();


            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _conn;
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                cmd.CommandText = "hry_insert_question_result";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", pUserID);
                cmd.Parameters.AddWithValue("@pQuestionID", pQuestionID);
                cmd.Parameters.AddWithValue("@pCampaignUserCodeID", pCampaignUserCodeID);
                cmd.Parameters.AddWithValue("@pAnswer01", pAnswer01);
                cmd.Parameters.AddWithValue("@pAnswer02", pAnswer02);
                cmd.Parameters.AddWithValue("@pAnswer03", pAnswer03);
                cmd.Parameters.AddWithValue("@pAnswer04", pAnswer04);
                cmd.Parameters.AddWithValue("@pAnswer05", pAnswer05);
                cmd.Parameters.AddWithValue("@pAnswer06", pAnswer06);
                cmd.Parameters.AddWithValue("@pAnswer07", pAnswer07);
                cmd.Parameters.AddWithValue("@pAnswer08", pAnswer08);
                cmd.Parameters.AddWithValue("@pAnswer09", pAnswer09);
                cmd.Parameters.AddWithValue("@pAnswer10", pAnswer10);
                cmd.Parameters.AddWithValue("@pAnswerOption", pAnswerOption);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                if (table != null && table.Rows.Count > 0)
                {
                    success = true;
                }
                _conn.Close();
                return success;
            }
        }
        #endregion
    }
}
