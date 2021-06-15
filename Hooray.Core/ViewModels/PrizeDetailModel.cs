using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class PrizeDetailModel
    {
        public string campaign_id { get; set; }
        public string campaign_name { get; set; }
        public string campaign_detail { get; set; }
        public string prize_name { get; set; }
        public string prize_detail { get; set; }
        public int id { get; set; }
        public int prize_receive_type { get; set; }
        public string prize_receive_name { get; set; }
        public string expire_date { get; set; }
        public string prize_confirm_code { get; set; }
        public int prize_accept_type { get; set; }
        public string prize_accept_condition { get; set; }
        public int prize_accept_expire_day { get; set; }
        public string accept_date { get; set; }
        public string sms_text { get; set; }
        public bool address_prize_complete { get; set; }
        public string address { get; set; }
        public string district { get; set; }
        public string amphor { get; set; }
        public string province { get; set; }
        public string zipcode { get; set; }
        public string prize_redeem_code_expire { get; set; }
        public int prize_redeem_code_age { get; set; }
        public bool is_expire { get; set; }
        public string img_height { get; set; }
        public string img_width { get; set; }
        public string bg_image_name { get; set; }
        public string campaign_image_name { get; set; }
        public string prize_description { get; set; }
        public int prize_redeem_code_display_type { get; set; }
        public bool status_accept { get; set; }
        public void loadDataPrizeDetail(DataRow dr)
        {
            campaign_id = dr["campaign_id"].ToString();
            campaign_name = dr["campaign_name"].ToString();
            campaign_detail = dr["campaign_desc"].ToString();
            prize_name = dr["prize_name"].ToString();
            prize_detail = dr["prize_detail"].ToString();
            id = dr["id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["id"]);
            //prize_receive_type = int.Parse(dr["prize_receive_type"].ToString());
            prize_receive_type = dr["prize_receive_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["prize_receive_type"]);
            prize_receive_name = dr["prize_receive_name"].ToString();
            expire_date = Utility.convertToDateServiceFormatString(dr["expire_date"].ToString());
            prize_confirm_code = dr["prize_confirm_code"].ToString();
            //prize_accept_type = int.Parse(dr["prize_accept_type"].ToString());
            prize_accept_type = dr["prize_accept_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["prize_accept_type"]);
            prize_accept_condition = dr["prize_accept_condition"].ToString();
            //prize_accept_expire_day = int.Parse(dr["prize_accept_expire_day"].ToString());
            prize_accept_expire_day = dr["prize_accept_expire_day"] == DBNull.Value ? 0 : Convert.ToInt32(dr["prize_accept_expire_day"]);
            accept_date = Utility.convertToDateServiceFormatString(dr["accept_date"].ToString());
            sms_text = dr["sms_text"].ToString();
            address_prize_complete = Convert.ToBoolean(dr["address_prize_complete"]);
            address = dr["address"].ToString();
            district = dr["district"].ToString();
            amphor = dr["amphor"].ToString();
            province = dr["province"].ToString();
            zipcode = dr["zipcode"].ToString();
            prize_redeem_code_expire = ((dr["prize_redeem_code_expire"] == null) || (dr["prize_redeem_code_expire"].ToString() == "") ? "-" : Utility.convertToDateTimeServiceFormatString(dr["prize_redeem_code_expire"].ToString()));
            //prize_redeem_code_age = int.Parse(dr["prize_redeem_code_age"].ToString());
            prize_redeem_code_age = dr["prize_redeem_code_age"] == DBNull.Value ? 0 : Convert.ToInt32(dr["prize_redeem_code_age"]);
            img_height = dr["img_height"].ToString();
            img_width = dr["img_width"].ToString();
            bg_image_name = dr["bg_image_name"].ToString();
            campaign_image_name = dr["campaign_image_name"].ToString();
            prize_description = dr["prize_description"].ToString();
            //prize_redeem_code_display_type = int.Parse(dr["prize_redeem_code_display_type"].ToString());
            prize_redeem_code_display_type = dr["prize_redeem_code_display_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["prize_redeem_code_display_type"]);
            status_accept = Convert.ToBoolean(dr["status_accept"]);

            if (prize_confirm_code != "")
            {
                is_expire = DateTime.Now > Convert.ToDateTime(dr["prize_redeem_code_expire"].ToString());
            }
            else
            {
                if (prize_confirm_code != "")
                {
                    is_expire = DateTime.Now.Date > Convert.ToDateTime(dr["prize_redeem_code_expire"].ToString());
                }
                else
                {
                    is_expire = DateTime.Now > Convert.ToDateTime(dr["expire_date"].ToString());
                }
            }
        }
        public ImagePhoto campaign_photo_image { get; set; }
        public ImagePhoto campaign_background_image { get; set; }
    }
}
