using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Hooray.Core.ViewModels.CampaignJoinViewModel;

namespace Hooray.Core.ViewModels
{
    public class JoinOnFeedTabModel
    {
    }
    public class HoorayJoin
    {
        public string message { get; set; }

        public bool device_token_status { get; set; }

        public bool status { get; set; }

        public bool status_login { get; set; }

        public List<Join> campaignjoin { get; set; }
    }
    //public class Join
    //{
    //    public int campaign_id { get; set; }
    //    public int campaign_user_code_id { get; set; }
    //    public string create_date { get; set; }
    //    public int announce_type { get; set; }
    //    public double join_number { get; set; }
    //    //public double win_rate { get; set; }
    //    public string announce_text { get; set; }
    //    public int result { get; set; }
    //    public string announce_date { get; set; }
    //    public int count_user { get; set; }
    //    public int total_join { get; set; }
    //    public int current_join { get; set; }
    //    public int prize_receive_type { get; set; }
    //    public bool require_prize_otp { get; set; }

    //    public void loadDataJoin(DataRow dr, double pEnjoyNumber)
    //    {
    //        join_number = pEnjoyNumber;
    //        campaign_user_code_id = int.Parse(dr["campaign_user_code_id"].ToString());
    //        campaign_id = int.Parse(dr["campaign_id"].ToString());
    //        create_date = Utility.convertToDateServiceFormatString(dr["create_date"].ToString());
    //        announce_type = int.Parse(dr["announce_type"].ToString());
    //        count_user = int.Parse(dr["count_user"].ToString());
    //        announce_date = Utility.convertToDateTimeServiceFormatString(dr["announce_date"].ToString());
    //        total_join = int.Parse(dr["total_join"].ToString());
    //        current_join = int.Parse(dr["current_join"].ToString());
    //        prize_receive_type = int.Parse(dr["prize_receive_type"].ToString());
    //        require_prize_otp = Convert.ToBoolean(dr["require_prize_otp"]);
    //    }
    //}
    public class CampaignPrize
    {
        public int prize_id { get; set; }
        public string campaign_id { get; set; }
        public string prize_name { get; set; }
        public string prize_detail { get; set; }
        public double prize_amount { get; set; }
        public int prize_qty { get; set; }
        public int prize_qty_usage { get; set; }
        public int prize_order { get; set; }
        public double win_rate { get; set; }
        // public int prize_receive_type { get; set; }

        public void loadDataPrize(DataRow dr)
        {
            //prize_id = int.Parse(dr["prize_id"].ToString());
            prize_id = dr["prize_id"] != DBNull.Value ? Convert.ToInt32(dr["prize_id"]) : 0;
            campaign_id = dr["campaign_id"].ToString();
            prize_name = dr["prize_name"].ToString();
            prize_detail = dr["prize_detail"].ToString();
            //prize_amount = double.Parse(dr["prize_amount"].ToString());
            prize_amount = dr["prize_amount"] != DBNull.Value ? Convert.ToDouble(dr["prize_amount"]) : 0.0;
            //prize_qty = int.Parse(dr["prize_qty"].ToString());
            prize_qty = dr["prize_qty"] != DBNull.Value ? Convert.ToInt32(dr["prize_qty"]) : 0;
            //prize_qty_usage = int.Parse(dr["prize_qty_usage"].ToString());
            prize_qty_usage = dr["prize_qty_usage"] != DBNull.Value ? Convert.ToInt32(dr["prize_qty_usage"]) : 0;
            //prize_order = int.Parse(dr["prize_order"].ToString());
            prize_order = dr["prize_order"] != DBNull.Value ? Convert.ToInt32(dr["prize_order"]) : 0;
            //win_rate = double.Parse(dr["win_rate"].ToString());
            win_rate = dr["win_rate"] != DBNull.Value ? Convert.ToDouble(dr["win_rate"]) : 0.0;
            // prize_receive_type = int.Parse(dr["prize_receive_type"].ToString());
        }
    }

    public class HoorayResultAward
    {
        public string message { get; set; }

        public bool device_token_status { get; set; }

        public bool status { get; set; }

        public bool status_login { get; set; }

        public CampaignResult campaign_result { get; set; }
    }
    public class CampaignResult
    {
        public int result_type { get; set; }
        public int check_announce { get; set; }
        public int prize_id { get; set; }
        public int user_prize_id { get; set; }
        public string announce_date { get; set; }
        public string announce_text { get; set; }
        public string device_type { get; set; }
        public string img_url { get; set; }
        public string img_path { get; set; }
        public bool is_share_reward { get; set; }
        public string result_image { get; set; }
        public string result_image_name { get; set; }
        public bool require_address_after_win { get; set; }
        public bool address_prize_complete { get; set; }
        public int verify_otp { get; set; }
        public bool require_prize_otp { get; set; }
        public string share_result_fb_message { get; set; }
        public string share_result_fb_message_lose { get; set; }
        public int special_prize_count { get; set; }
        public int id { get; set; }
        public int prize_count { get; set; }
        public int prize_order { get; set; }
        public string prize_remark_caption { get; set; }
        public bool random_result_on_check { get; set; }
        public double win_rate { get; set; }
        public int image_type_for_result { get; set; }
        public bool external_link_after_check { get; set; }
        public string text_popup_after_check { get; set; }
        public string link_after_check { get; set; }
        //public int coin_qty { get; set; }

        public void loadDataCampaignResult(DataRow dr)
        {
            //result_type = int.Parse(dr["resultType"].ToString()); //1=win,2=lost,3=NA
            result_type = Convert.ToInt32(dr["resultType"] == DBNull.Value ? 0 : dr["resultType"]);
            announce_date = Utility.convertToDateTimeServiceFormatString(dr["announce_date"].ToString());
            //check_announce = int.Parse(dr["checkAnnounce"].ToString());
            check_announce = Convert.ToInt32(dr["checkAnnounce"] == DBNull.Value ? 0 : dr["checkAnnounce"]);
            //prize_id = int.Parse(dr["prize_id"].ToString());
            prize_id = Convert.ToInt32(dr["prize_id"] == DBNull.Value ? 0 : dr["prize_id"]);

            //user_prize_id = int.Parse(dr["user_prize_id"].ToString());
            user_prize_id = Convert.ToInt32(dr["user_prize_id"] == DBNull.Value ? 0 : dr["user_prize_id"]);
            device_type = dr["device_type"].ToString();
            is_share_reward = Convert.ToBoolean(dr["is_share_reward"]);
            result_image = dr["resultImage"].ToString();
            //verify_otp = int.Parse(dr["verify_otp"].ToString());
            verify_otp = Convert.ToInt32(dr["verify_otp"] == DBNull.Value ? 0 : dr["verify_otp"]);
            share_result_fb_message = dr["share_result_fb_message"].ToString();
            share_result_fb_message_lose = dr["share_result_fb_message_lose"].ToString();
            //special_prize_count = int.Parse(dr["special_prize_count"].ToString());
            special_prize_count = Convert.ToInt32(dr["special_prize_count"] == DBNull.Value ? 0 : dr["special_prize_count"]);
            //id = int.Parse(dr["id"].ToString());
            id = Convert.ToInt32(dr["id"] == DBNull.Value ? 0 : dr["id"]);
            result_image_name = dr["resultImageName"].ToString();

            if (result_type == 1)
            {
                announce_text = dr["win_text"].ToString();
            }
            else if (result_type == 2)
            {
                announce_text = dr["lost_text"].ToString();
            }
            else if (result_type == 3)
            {
                announce_text = "ยังไม่ถึงวันประกาศผล";
            }
            else
            {
                announce_text = "คุณยังไม่ได้ เข้าร่วมกิจกรรม";
            }
            require_address_after_win = Convert.ToBoolean(dr["require_address_after_win"]);
            address_prize_complete = Convert.ToBoolean(dr["address_prize_complete"]);
            require_prize_otp = Convert.ToBoolean(dr["require_prize_otp"]);
            //prize_count = int.Parse(dr["prize_count"].ToString());
            prize_count = Convert.ToInt32(dr["prize_count"] == DBNull.Value ? 0 : dr["prize_count"]);
            //prize_order = int.Parse(dr["prize_order"].ToString());
            prize_order = Convert.ToInt32(dr["prize_order"] == DBNull.Value ? 0 : dr["prize_order"]);
            prize_remark_caption = dr["prize_remark_caption"].ToString();
            random_result_on_check = Convert.ToBoolean(dr["random_result_on_check"]);
            //win_rate = double.Parse(dr["win_rate"].ToString());
            win_rate = Convert.ToDouble(dr["win_rate"] == DBNull.Value ? 0.0 : dr["win_rate"]);
            //image_type_for_result = int.Parse(dr["image_type_for_result"].ToString());
            image_type_for_result = Convert.ToInt32(dr["image_type_for_result"] == DBNull.Value ? 0 : dr["image_type_for_result"]);
            external_link_after_check = Convert.ToBoolean(dr["external_link_after_check"]);
            text_popup_after_check = dr["text_popup_after_check"].ToString();
            link_after_check = dr["link_after_check"].ToString();
            //coin_qty = int.Parse(dr["coin_qty"].ToString());
        }
    }

}
