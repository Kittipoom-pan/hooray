using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Hooray.Core.ViewModels
{
    public class RegisterModel
    {
        public Register register { get; set; } = new Register();


        //public List<Sms> sms_user { get; set; } = new List<Sms>();

    }

    public class SignUpMessage
    {

        public string message { get; set; }

        public bool device_token_status { get; set; }


        public bool status { get; set; }


        public Register register { get; set; }


        public List<Sms> sms_user { get; set; }
    }
    public class Register
    {

        public int user_id { get; set; }

        public string display_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string birthday { get; set; }
        public string gender { get; set; }
        public string mobile { get; set; }
        public string userlang { get; set; }
        public bool is_search_friend { get; set; }
        public int tab_menu { get; set; }
        public bool require_mobile_verify { get; set; }
        public string image_name_profile { get; set; }

        public void loadDataRegister(DataRow dr)
        {
            birthday = Utility.convertToDateServiceFormatString(dr["display_birthday"].ToString());

            user_id = int.Parse(dr["user_id"].ToString());
            display_name = dr["display_show_name"].ToString();
            email = dr["display_email"].ToString();
            first_name = dr["display_fname"].ToString();
            last_name = dr["display_lname"].ToString();
            gender = dr["display_gender"].ToString();
            mobile = dr["display_mobile"].ToString();
            userlang = dr["user_lang"].ToString();
            is_search_friend = Convert.ToBoolean(dr["is_search_friend"]);
            tab_menu = /*int.Parse(WebConfigurationManager.AppSettings["tabMenu"])*/ 3;
            require_mobile_verify = Convert.ToBoolean(dr["require_mobile_verify"]);
            image_name_profile = dr["image_name_profile"].ToString();
            //old_user_id = int.Parse(dr["old_userid"].ToString());
        }
        public void loadDataRegister(loadDataRegisterModel model)
        {
            birthday = model.display_birthday;
            user_id = model.user_id;
            display_name = model.display_show_name;
            email = model.display_email;
            first_name = model.display_fname;

            last_name = model.display_lname;
            gender = model.display_gender;
            mobile = model.display_mobile;
            userlang = model.user_lang;
            is_search_friend = model.is_search_friend;
            tab_menu =  3;
            require_mobile_verify = model.require_mobile_verify;
            image_name_profile = model.image_name_profile;
            //old_user_id = model.old_userid;
        }
        public ImagePhoto user_image_round { get; set; }
        public ImagePhoto user_image_full { get; set; }
        //public StartupBadge startup_badge { get; set; }
    }
    public class ImagePhoto
    {
        public string image_url { get; set; }
        public double image_update_date { get; set; }
        public string image_path { get; set; }
        public void loadDataImagePhoto(DataRow dr)
        {
            image_url = dr["image_url"].ToString();
            image_path = dr["image_path"].ToString();
            //image_update_date = double.Parse(dr["image_update_timestamp"].ToString());
            image_update_date = dr["image_update_timestamp"]==DBNull.Value?0.0:Convert.ToDouble(dr["image_update_timestamp"]);
        }
    }
    public class StartupBadge
    {
        public int friend_badge_count { get; set; }
        public int chat_badge_count { get; set; }
        public int gift_badge_count { get; set; }
        public int result_badge_count { get; set; }
        public int follow_badge_count { get; set; }

        public void loadDataStartupBadge(DataRow dr)
        {
            friend_badge_count = int.Parse(dr["un_new_friends"].ToString());
            chat_badge_count = int.Parse(dr["un_new_chat"].ToString());
            gift_badge_count = int.Parse(dr["un_new_gift"].ToString());
            result_badge_count = int.Parse(dr["un_new_result"].ToString());
            follow_badge_count = int.Parse(dr["un_new_follow"].ToString());
        }
    }
    public class Sms
    {
        //public int campaign_id { get; set; }
        public string campaign_id { get; set; }
        public string sms_text { get; set; }
        public string sms_image { get; set; }
        public string create_date { get; set; }
        //public void loadDataSms(DataRow dr)
        //{
        //    campaign_id = int.Parse(dr["campaign_id"].ToString());
        //    sms_text = dr["sms_text"].ToString();
        //    sms_image = string.Format("http://{0}/MobileServices/HoorayDev/sms_image/{1}", "203.151.213.133", dr["sms_image"].ToString());
        //    create_date = Utility.convertToDateServiceFormatString(dr["create_date"].ToString());
        //}
    }
   
}
