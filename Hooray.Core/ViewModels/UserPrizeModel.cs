using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class UserPrize
    {
        public int user_prize_id { get; set; }
        public int campaign_id { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string prize_description { get; set; }
        public string create_date { get; set; }
        public string accept_date { get; set; }
        public bool status_accept { get; set; }
        public int prize_receive_type { get; set; }
        public string prize_receive_name { get; set; }
        public int prize_accept_type { get; set; }
        public bool verify_otp { get; set; }
        public int count_otp { get; set; }
        public bool require_prize_otp { get; set; }
        public bool require_address_after_win { get; set; }
        public bool address_prize_complete { get; set; }
        public string address { get; set; }
        public string district { get; set; }
        public string amphor { get; set; }
        public string province { get; set; }
        public string zipcode { get; set; }
        public string company_image_name { get; set; }
        public string expire_date { get; set; }
        public bool flag_expire_date { get; set; }
        public string prize_name { get; set; }
        public string prize_remark_caption { get; set; }

        public void loadDataUserPrize(DataRow dr)
        {
            user_prize_id = dr["user_prize_id"]== DBNull.Value?0:Convert.ToInt32(dr["user_prize_id"]);
           
            campaign_id = dr["campaign_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["campaign_id"]);
            
            company_id = dr["company_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["company_id"]);
            company_name = dr["company_name"].ToString();
            prize_description = dr["prize_description"].ToString();
            create_date = Utility.convertToDateServiceFormatString(dr["create_date"].ToString());
            status_accept = Convert.ToBoolean(dr["status_accept"]);
          
            prize_receive_type = dr["prize_receive_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["prize_receive_type"]);
            prize_receive_name = dr["prize_receive_name"].ToString();
            accept_date = Utility.convertToDateTimeServiceFormatString(dr["accept_date"].ToString());
            
            prize_accept_type = dr["prize_accept_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["prize_accept_type"]);
            
            verify_otp = dr["verify_otp"] == DBNull.Value ? false : Convert.ToBoolean(dr["verify_otp"]);
            
            count_otp = dr["count_otp"] == DBNull.Value ? 0 : Convert.ToInt32(dr["count_otp"]);
            require_prize_otp = Convert.ToBoolean(dr["require_prize_otp"]);
            require_address_after_win = Convert.ToBoolean(dr["require_address_after_win"]);
            address_prize_complete = Convert.ToBoolean(dr["address_prize_complete"]);
            address = dr["address"].ToString();
            district = dr["district"].ToString();
            amphor = dr["amphor"].ToString();
            province = dr["province"].ToString();
            zipcode = dr["zipcode"].ToString();
            company_image_name = dr["company_image_name"].ToString();
            expire_date = Utility.convertToDateTimeServiceFormatString(dr["expire_date"].ToString());
            
            flag_expire_date = dr["flag_expire_date"] == DBNull.Value ? false : Convert.ToBoolean(dr["flag_expire_date"]);
            prize_name = dr["prize_name"].ToString();
            prize_remark_caption = dr["prize_remark_caption"].ToString();
        }
        public ImagePhoto company_logo_image { get; set; }
    }

    public class HoorayDeleteUserPrize
    {
        public bool status_delete { get; set; }
    }
}
