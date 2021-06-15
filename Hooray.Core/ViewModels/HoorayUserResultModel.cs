using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class HoorayUserResultModel
    {
        //public int prize_badge { get; set; }

        public List<UserResult> user_result { get; set; }
    }
    public class UserResult
    {
        public string campaign_id { get; set; }
        public string campaign_name { get; set; }
        public string campaign_desc { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public bool join_status { get; set; }
        public bool check_announce { get; set; }
        public int img_height { get; set; }
        public int img_width { get; set; }
        public int campaign_image_type { get; set; }
        public string announce_date { get; set; }
        public int announce_type { get; set; }
        public string check_reward_expire_date { get; set; }
        public bool flag_reward_expire_date { get; set; }
        public string company_image_name { get; set; }
        public string campaign_image_name { get; set; }
        public bool flag_announce { get; set; }
        public string random_result_date { get; set; }

        public int campaign_type { get; set; }
        public string deal_buy_detail { get; set; }
        public int deal_id { get; set; }
        public int deal_gallery_count { get; set; }
        public string deal_time_expire { get; set; }

        public void loadDataUserResult(DataRow dr)
        {
            campaign_id = dr["campaign_id"].ToString();
            campaign_name = dr["campaign_name"].ToString();
            campaign_desc = dr["campaign_desc"].ToString();
            //shop_id = int.Parse(dr["shop_id"].ToString());
            company_id =  dr["company_id"] == DBNull.Value ?0:Convert.ToInt32(dr["company_id"]);
            company_name = dr["company_name"].ToString();
            //join_status = int.Parse(dr["join_status"].ToString());
            join_status = dr["join_status"] == DBNull.Value ? false : Convert.ToBoolean(dr["join_status"]);
            //check_announce = int.Parse(dr["check_announce"].ToString());
            check_announce = dr["check_announce"] == DBNull.Value ? false : Convert.ToBoolean(dr["check_announce"]);
            //img_height = int.Parse(dr["img_height"].ToString());
            img_height = dr["img_height"] == DBNull.Value ? 0 : Convert.ToInt32(dr["img_height"]);
            //img_width = int.Parse(dr["img_width"].ToString());
            img_width = dr["img_width"] == DBNull.Value ? 0 : Convert.ToInt32(dr["img_width"]);
            //campaign_image_type = int.Parse(dr["campaign_image_type"].ToString());
            campaign_image_type = dr["campaign_image_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["campaign_image_type"]);
            announce_date = Utility.convertToDateTimeServiceFormatString(dr["announce_date"].ToString());
            //announce_type = int.Parse(dr["announce_type"].ToString());
            announce_type = dr["announce_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["announce_type"]);
            check_reward_expire_date = Utility.convertToDateTimeServiceFormatString(dr["check_reward_expire_date"].ToString());
            //flag_reward_expire_date = int.Parse(dr["flag_reward_expire_date"].ToString());
            flag_reward_expire_date = dr["flag_reward_expire_date"] == DBNull.Value ? false : Convert.ToBoolean(dr["flag_reward_expire_date"]);
            company_image_name = dr["company_image_name"].ToString();
            campaign_image_name = dr["campaign_image_name"].ToString();
            random_result_date = dr["random_result_date"].ToString();

            if ((DateTime.Now > Convert.ToDateTime(dr["announce_date"].ToString())) && (random_result_date != ""))
            {
                flag_announce = true;
            }
            else
            {
                flag_announce = false;
            }

            //campaign_type = int.Parse(dr["campaign_type"].ToString());
            campaign_type = dr["campaign_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["campaign_type"]);
            deal_buy_detail = dr["deal_buy_detail"].ToString();
            //deal_id = int.Parse(dr["deal_id"].ToString());
            deal_id = dr["deal_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["deal_id"]);
            //deal_gallery_count = int.Parse(dr["deal_gallery_count"].ToString());
            deal_gallery_count = dr["deal_gallery_count"] == DBNull.Value ? 0 : Convert.ToInt32(dr["deal_gallery_count"]);
            deal_time_expire = dr["deal_time_expire"].ToString();
        }
        public List<SmsResult> sms_result { get; set; }
        public ImagePhoto company_logo_image { get; set; }
        public ImagePhoto campaign_photo_image { get; set; }
    }
    public class SmsResult
    {
        public string user_id { get; set; }
        public int sms_type { get; set; }
        public string display_name { get; set; }
        public string sms_text { get; set; }
        public string create_date { get; set; }
        public string image_name_profile { get; set; }

        public void loadDataSmsResult(DataRow dr , string url)
        {
            create_date = Utility.convertToDateServiceFormatString(dr["create_date"].ToString());
            user_id = dr["user_id"].ToString();
            display_name = dr["display_show_name"].ToString();
            image_name_profile = dr["image_name_profile"].ToString();

            #region sms
            string smsText = dr["sms_text"].ToString();
            string smsImage = string.Format(url, dr["sms_image"].ToString());
            if (smsText == "")
            {
                sms_text = smsImage;
                sms_type = 1;
            }
            else
            {
                sms_text = smsText;
                sms_type = 2;
            }
            #endregion
        }

        public ImagePhoto user_image_round { get; set; }
        public ImagePhoto user_image_full { get; set; }
    }
}
