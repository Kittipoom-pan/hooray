using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class FollowModel
    {
        public string campaign_id { get; set; }
        public int company_id { get; set; }
        public string campaign_name { get; set; }
        public int campaign_type { get; set; }
        public string sms_number { get; set; }
        public string campaign_desc { get; set; }
        public string campaign_short_desc { get; set; }
        public string campaign_result_desc { get; set; }
        public string campaign_result_announce { get; set; }
        public int campaign_like_count { get; set; }
        public int campaign_comment_count { get; set; }
        public string join_start_date { get; set; }
        public string join_end_date { get; set; }
        public string create_date { get; set; }
        public string company_name { get; set; }
        public bool user_campaign_like { get; set; }
        public bool user_campaign_join { get; set; }
        public bool status_follow { get; set; }
        public int img_width { get; set; }
        public int img_height { get; set; }
        public int is_new { get; set; }
        public int campaign_image_type { get; set; }
        public int total_join { get; set; }
        public int current_join { get; set; }

        public bool is_question { get; set; }
        public bool require_address { get; set; }
        public bool required_facebook { get; set; }
        public string facebook_page_id { get; set; }
        public bool address_completed { get; set; }
        public bool is_show_join { get; set; }
        public bool require_address_after_win { get; set; }
        public bool is_follow { get; set; }
        public string company_image_name { get; set; }
        public bool is_start_join { get; set; }
        public bool is_expire_join { get; set; }
        public string campaign_image_name { get; set; }
        public int campaign_gallery_count { get; set; }
        public int campaign_gallery_no { get; set; }
        public bool required_share_feed { get; set; }
        public bool is_join_verify_code { get; set; }
        public string question_for_verify { get; set; }
        public bool flag_used_app { get; set; }
        public string android_package { get; set; }
        public string android_playstore_url { get; set; }
        public string ios_package { get; set; }
        public string ios_package_url { get; set; }
        public string app_name { get; set; }
        public string company_url { get; set; }
        //HR-18 Watch video before join campaign
        public bool required_watch_video { get; set; }
        public int skip_video_per_sec { get; set; }



        public void loadDataFollow(DataRow dr, int uid)
        {
            campaign_id = dr["campaign_id"] == DBNull.Value ? "" : dr["campaign_id"].ToString();
            company_id = dr["company_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["company_id"]);
            campaign_name = dr["campaign_name"].ToString();
            campaign_type = dr["campaign_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["campaign_type"]);
            //campaign_image_type = int.Parse(dr["campaign_image_type"].ToString());
            campaign_image_type = dr["campaign_image_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["campaign_image_type"]);
            sms_number = dr["sms_number"].ToString();
            campaign_desc = dr["campaign_desc"].ToString();
            campaign_short_desc = dr["campaign_short_desc"].ToString();
            campaign_result_desc = dr["campaign_result_desc"].ToString();
            campaign_result_announce = dr["campaign_result_announce"].ToString();
            join_start_date = Utility.convertToDateTimeServiceFormatString(dr["join_start_date"].ToString());
            join_end_date = Utility.convertToDateTimeServiceFormatString(dr["join_end_date"].ToString());
            //campaign_like_count = int.Parse(dr["like_count"].ToString());
            campaign_like_count = dr["like_count"] == DBNull.Value ? 0 : Convert.ToInt32(dr["like_count"]);
            //campaign_comment_count = int.Parse(dr["comment_count"].ToString());
            campaign_comment_count = dr["comment_count"] == DBNull.Value ? 0 : Convert.ToInt32(dr["comment_count"]);
            create_date = Utility.convertToDateServiceFormatString(dr["create_date"].ToString());
            company_name = dr["company_name"].ToString();
            //user_campaign_like = Convert.ToBoolean(int.Parse(dr["user_campaign_like"].ToString()));
            user_campaign_like = dr["user_campaign_like"] == DBNull.Value ? false : Convert.ToBoolean(dr["user_campaign_like"]);
            //user_campaign_join = Convert.ToBoolean(int.Parse(dr["user_campaign_join"].ToString()));
            user_campaign_join = dr["user_campaign_join"] == DBNull.Value ? false : Convert.ToBoolean(dr["user_campaign_join"]);
            //status_follow = Convert.ToBoolean(int.Parse(dr["status_follow"].ToString()));
            status_follow = dr["status_follow"] == DBNull.Value ? false : Convert.ToBoolean(dr["status_follow"]);
            //img_height = int.Parse(dr["img_height"].ToString());
            img_height = dr["img_height"] == DBNull.Value ? 0 : Convert.ToInt32(dr["img_height"]);
            //img_width = int.Parse(dr["img_width"].ToString());
            img_width = dr["img_width"] == DBNull.Value ? 0 : Convert.ToInt32(dr["img_width"]);
            //is_new = int.Parse(dr["is_new"].ToString());
            is_new = dr["is_new"] == DBNull.Value ? 0 : Convert.ToInt32(dr["is_new"]);
            //total_join = int.Parse(dr["total_join"].ToString());
            total_join = dr["total_join"] == DBNull.Value ? 0 : Convert.ToInt32(dr["total_join"]);
            //current_join = int.Parse(dr["current_join"].ToString());
            current_join = dr["current_join"] == DBNull.Value ? 0 : Convert.ToInt32(dr["current_join"]);

            //is_question = Convert.ToBoolean(dr["is_question"]);
            is_question = dr["is_question"] == DBNull.Value ? false : Convert.ToBoolean(dr["is_question"]);
            //require_address = Convert.ToBoolean(dr["require_address"]);
            require_address = dr["require_address"] == DBNull.Value ? false : Convert.ToBoolean(dr["require_address"]);
            //required_facebook = Convert.ToBoolean(dr["required_facebook"]);
            required_facebook = dr["required_facebook"] == DBNull.Value ? false : Convert.ToBoolean(dr["required_facebook"]);
            facebook_page_id = dr["facebook_page_id"].ToString();
            //address_completed = Convert.ToBoolean(int.Parse(dr["address_completed"].ToString()));
            address_completed = dr["address_completed"] == DBNull.Value ? false : Convert.ToBoolean(dr["address_completed"]);
            //is_show_join = Convert.ToBoolean(dr["is_show_join"]);
            is_show_join = dr["is_show_join"] == DBNull.Value ? false : Convert.ToBoolean(dr["is_show_join"]);
            //require_address_after_win = Convert.ToBoolean(dr["require_address_after_win"]);
            require_address_after_win = dr["require_address_after_win"] == DBNull.Value ? false : Convert.ToBoolean(dr["require_address_after_win"]);
            //is_follow = Convert.ToBoolean(dr["is_follow"]);
            is_follow = dr["is_follow"] == DBNull.Value ? false : Convert.ToBoolean(dr["is_follow"]);
            company_image_name = dr["company_image_name"].ToString();
            is_expire_join = DateTime.Now > Convert.ToDateTime(dr["join_end_date"].ToString());
            is_start_join = DateTime.Now >= Convert.ToDateTime(dr["join_start_date"].ToString());
            campaign_image_name = dr["campaign_image_name"].ToString();
            //campaign_gallery_count = int.Parse(dr["campaign_gallery_count"].ToString());
            campaign_gallery_count = dr["campaign_gallery_count"] == DBNull.Value ? 0 : Convert.ToInt32(dr["campaign_gallery_count"]);
            //campaign_gallery_no = int.Parse(dr["campaign_gallery_no"].ToString());
            campaign_gallery_no = dr["campaign_gallery_no"] == DBNull.Value ? 0 : Convert.ToInt32(dr["campaign_gallery_no"]);
            //required_share_feed = Convert.ToBoolean(dr["required_share_feed"]);
            required_share_feed = dr["required_share_feed"] == DBNull.Value ? false : Convert.ToBoolean(dr["required_share_feed"]);
            //is_join_verify_code = Convert.ToBoolean(dr["is_join_verify_code"]);
            is_join_verify_code = dr["is_join_verify_code"] == DBNull.Value ? false : Convert.ToBoolean(dr["is_join_verify_code"]);
            question_for_verify = dr["question_text"].ToString();
            //flag_used_app = Convert.ToBoolean(dr["flag_used_app"]);
            flag_used_app = dr["flag_used_app"] == DBNull.Value ? false : Convert.ToBoolean(dr["flag_used_app"]);
            android_package = dr["android_package"].ToString();
            android_playstore_url = dr["android_playstore_url"].ToString();
            ios_package = dr["ios_package"].ToString();
            ios_package_url = dr["ios_package_url"].ToString();
            app_name = dr["app_name"].ToString();
            company_url = dr["company_url"].ToString();
            //HR-18 Watch video before join campaign
            //required_watch_video = Convert.ToBoolean(int.Parse(dr["required_watch_video"].ToString()));
            required_watch_video = dr["required_watch_video"] == DBNull.Value ? false : Convert.ToBoolean(dr["required_watch_video"]);
            //skip_video_per_sec = int.Parse(dr["skip_video_per_sec"].ToString());
            skip_video_per_sec = dr["skip_video_per_sec"] == DBNull.Value ? 0 : Convert.ToInt32(dr["skip_video_per_sec"]);
        }
        public ImagePhoto company_logo_image { get; set; }
        public ImagePhoto campaign_photo_image { get; set; }
        public List<Question> question { get; set; }
        public List<ImagePhoto> campaign_gallery_image { get; set; }
    }
}
