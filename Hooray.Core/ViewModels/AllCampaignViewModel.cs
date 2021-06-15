using System;
using System.Collections.Generic;
using System.Data;

namespace Hooray.Core.ViewModels
{
    public class AllCampaignViewModel
    {
        public string message { get; set; }
        public bool device_token_status { get; set; }
        public bool status { get; set; }
        public bool status_login { get; set; }

        public List<Campaign> campaign { get; set; }
        public string url_android { get; set; }
        public string url_ios { get; set; }
        public StartupBadgeViewModel startup_badge { get; set; }

    }
    public class Campaign
    {
        public string campaign_id { get; set; }
        public int company_id { get; set; }
        public string campaign_name { get; set; }
        public int campaign_type { get; set; }
        public int campaign_image_type { get; set; }
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
        public int total_join { get; set; }
        public int current_join { get; set; }
        public bool is_question { get; set; }
        public bool require_address { get; set; }
        public bool? required_facebook { get; set; }
        public string facebook_page_id { get; set; }
        public bool address_completed { get; set; }
        public bool is_show_join { get; set; }
        public bool require_address_after_win { get; set; }
        public bool is_follow { get; set; }
        public string company_image_name { get; set; }
        public bool is_start_join { get; set; }
        public bool is_expire_join { get; set; }
        public string campaign_image_name { get; set; }
        public int status_mobile_verify { get; set; }
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
      
        public bool required_watch_video { get; set; }
        //public int skip_video_per_sec { get; set; }
        public string video_url { get; set; }

        public void loadDataCampaign(DataRow dr)
        {
            campaign_id =dr["campaign_id"].ToString();
            company_id = int.Parse(dr["company_id"].ToString());
            campaign_name = dr["campaign_name"].ToString();
            campaign_type = int.Parse(dr["campaign_type"].ToString());
            campaign_image_type = dr["campaign_image_type"] != DBNull.Value ? Convert.ToInt32(dr["campaign_image_type"]) : 0;
            sms_number = dr["sms_number"].ToString();
            campaign_desc = dr["campaign_desc"].ToString();
            campaign_short_desc = dr["campaign_short_desc"].ToString();
            campaign_result_desc = dr["campaign_result_desc"].ToString();
            campaign_result_announce = dr["campaign_result_announce"].ToString();
            join_start_date = Utility.convertToDateTimeServiceFormatString(dr["join_start_date"].ToString());
            join_end_date = Utility.convertToDateTimeServiceFormatString(dr["join_end_date"].ToString());
            campaign_like_count = int.Parse(dr["like_count"].ToString());
            campaign_comment_count = int.Parse(dr["comment_count"].ToString());
            create_date = Utility.convertToDateServiceFormatString(dr["created_date"].ToString());
            company_name = dr["company_name"].ToString();
            user_campaign_like = Convert.ToBoolean(int.Parse(dr["user_campaign_like"].ToString()));
            user_campaign_join = Convert.ToBoolean(int.Parse(dr["user_campaign_join"].ToString()));
            status_follow = Convert.ToBoolean(int.Parse(dr["status_follow"].ToString()));
            img_height = dr["img_height"] != DBNull.Value ? Convert.ToInt32(dr["img_height"]) : 0;
            img_width = dr["img_width"] != DBNull.Value ? Convert.ToInt32(dr["img_width"]) : 0;
            total_join = int.Parse(dr["total_join"].ToString());
            current_join = int.Parse(dr["current_join"].ToString());
            is_question = Convert.ToBoolean(dr["is_question"]);
            require_address = Convert.ToBoolean(dr["require_address"]);
            required_facebook = dr["required_facebook"] != DBNull.Value ? Convert.ToBoolean(dr["required_facebook"]) : false;
            facebook_page_id = dr["facebook_page_id"].ToString();
            address_completed = Convert.ToBoolean(int.Parse(dr["address_completed"].ToString()));
            is_show_join = dr["is_show_join"] != DBNull.Value ? Convert.ToBoolean(dr["is_show_join"]) : false;
            require_address_after_win = Convert.ToBoolean(dr["require_address_after_win"]);
            is_follow = Convert.ToBoolean(dr["is_follow"]);
            company_image_name = dr["company_image_name"].ToString();
            var join_end_date_db = dr.IsNull("join_end_date") ? (DateTime?)null : Convert.ToDateTime(dr["join_end_date"].ToString());
            if (join_end_date_db == null)
            {
                is_expire_join = false;
            }
            else if (DateTime.Now > Convert.ToDateTime(join_end_date_db))
            {
                is_expire_join = true;
            }
            else
            {
                is_expire_join = false;
            }
            var join_start_date_db = dr.IsNull("join_start_date") ? (DateTime?)null : Convert.ToDateTime(dr["join_start_date"].ToString());
            if (join_start_date_db == null)
            {
                is_start_join = false;
            }
            else if (DateTime.Now >= Convert.ToDateTime(join_start_date_db))
            {
                is_start_join = true;
            }
            else
            {
                is_start_join = false;
            }
            campaign_image_name = dr["campaign_image_name"].ToString();
            status_mobile_verify = int.Parse(dr["status_mobile_verify"].ToString());
            campaign_gallery_count = dr["campaign_gallery_count"] != DBNull.Value ? Convert.ToInt32(dr["campaign_gallery_count"]) : 0;
            campaign_gallery_no = dr["campaign_gallery_no"] != DBNull.Value ? Convert.ToInt32(dr["campaign_gallery_no"]) : 0;
            required_share_feed = dr["required_share_feed"] != DBNull.Value ? Convert.ToBoolean(dr["required_share_feed"]) : false;
            is_join_verify_code = dr["is_join_verify_code"] != DBNull.Value ? Convert.ToBoolean(dr["is_join_verify_code"]) : false;
            question_for_verify = dr["question_text"].ToString();
            
            required_watch_video = Convert.ToBoolean(int.Parse(dr["required_watch_video"].ToString()));
            //skip_video_per_sec = int.Parse(dr["skip_video_per_sec"].ToString());
        }
        public ImagePhoto company_logo_image { get; set; }
        public ImagePhoto campaign_photo_image { get; set; }

        public List<Question> question { get; set; }
        public List<ImagePhoto> campaign_gallery_image { get; set; }
    }

    //public class startup_badge
    //{
    //    public int friend_badge_count { get; set; }
    //    public int chat_badge_count { get; set; }
    //    public int gift_badge_count { get; set; }
    //    public int result_badge_count { get; set; }
    //    public int follow_badge_count { get; set; }

    //    public void loadDataStartupBadge(DataRow dr)
    //    {
    //        friend_badge_count = int.Parse(dr["un_new_friends"].ToString());
    //        chat_badge_count = int.Parse(dr["un_new_chat"].ToString());
    //        gift_badge_count = int.Parse(dr["un_new_gift"].ToString());
    //        result_badge_count = int.Parse(dr["un_new_result"].ToString());
    //        follow_badge_count = int.Parse(dr["un_new_follow"].ToString());
    //    }
    //}

    public class HoorayAllFeed
    {
        public string message { get; set; }

        public bool device_token_status { get; set; }

        public bool status { get; set; }

        public bool status_login { get; set; }

        public List<CampaignNew> campaign { get; set; }

        public string url_android { get; set; }

        public string url_ios { get; set; }

        public StartupBadge startup_badge { get; set; }

    }
    public class CampaignNew
    {
        public string campaign_id { get; set; }
        public int company_id { get; set; }
        public string campaign_name { get; set; }
        public int campaign_type { get; set; }
        public int campaign_image_type { get; set; }
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
        public int status_mobile_verify { get; set; }
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
        public string video_url { get; set; }

        public void loadDataCampaign(DataRow dr)
        {
            campaign_id = dr["campaign_id"].ToString();
            company_id = int.Parse(dr["company_id"].ToString());
            campaign_name = dr["campaign_name"].ToString();
            campaign_type = int.Parse(dr["campaign_type"].ToString());
            campaign_image_type = int.Parse(dr["campaign_image_type"].ToString());
            sms_number = dr["sms_number"].ToString();
            campaign_desc = dr["campaign_desc"].ToString();
            campaign_short_desc = dr["campaign_short_desc"].ToString();
            campaign_result_desc = dr["campaign_result_desc"].ToString();
            campaign_result_announce = dr["campaign_result_announce"].ToString();
            join_start_date = Utility.convertToDateTimeServiceFormatString(dr["join_start_date"].ToString());
            join_end_date = Utility.convertToDateTimeServiceFormatString(dr["join_end_date"].ToString());
            campaign_like_count = int.Parse(dr["like_count"].ToString());
            campaign_comment_count = int.Parse(dr["comment_count"].ToString());
            create_date = Utility.convertToDateServiceFormatString(dr["create_date"].ToString());
            company_name = dr["company_name"].ToString();
            user_campaign_like = Convert.ToBoolean(int.Parse(dr["user_campaign_like"].ToString()));
            user_campaign_join = Convert.ToBoolean(int.Parse(dr["user_campaign_join"].ToString()));
            status_follow = Convert.ToBoolean(int.Parse(dr["status_follow"].ToString()));
            img_height = int.Parse(dr["img_height"].ToString());
            img_width = int.Parse(dr["img_width"].ToString());
            total_join = int.Parse(dr["total_join"].ToString());
            current_join = int.Parse(dr["current_join"].ToString());
            is_question = Convert.ToBoolean(dr["is_question"]);
            require_address = Convert.ToBoolean(dr["require_address"]);
            required_facebook = Convert.ToBoolean(dr["required_facebook"]);
            facebook_page_id = dr["facebook_page_id"].ToString();
            address_completed = Convert.ToBoolean(int.Parse(dr["address_completed"].ToString()));
            is_show_join = Convert.ToBoolean(dr["is_show_join"]);
            require_address_after_win = Convert.ToBoolean(dr["require_address_after_win"]);
            is_follow = Convert.ToBoolean(dr["is_follow"]);
            company_image_name = dr["company_image_name"].ToString();
            is_expire_join = DateTime.Now > Convert.ToDateTime(dr["join_end_date"].ToString());
            is_start_join = DateTime.Now >= Convert.ToDateTime(dr["join_start_date"].ToString());
            campaign_image_name = dr["campaign_image_name"].ToString();
            status_mobile_verify = int.Parse(dr["status_mobile_verify"].ToString());
            campaign_gallery_count = int.Parse(dr["campaign_gallery_count"].ToString());
            campaign_gallery_no = int.Parse(dr["campaign_gallery_no"].ToString());
            required_share_feed = Convert.ToBoolean(dr["required_share_feed"]);
            is_join_verify_code = Convert.ToBoolean(dr["is_join_verify_code"]);
            question_for_verify = dr["question_text"].ToString();
            flag_used_app = Convert.ToBoolean(dr["flag_used_app"]);
            android_package = dr["android_package"].ToString();
            android_playstore_url = dr["android_playstore_url"].ToString();
            ios_package = dr["ios_package"].ToString();
            ios_package_url = dr["ios_package_url"].ToString();
            app_name = dr["app_name"].ToString();
            company_url = dr["company_url"].ToString();
            //HR-18 Watch video before join campaign
            required_watch_video = Convert.ToBoolean(int.Parse(dr["required_watch_video"].ToString()));
            skip_video_per_sec = int.Parse(dr["skip_video_per_sec"].ToString());
            video_url = dr["video_url"].ToString();
        }
        public ImagePhoto company_logo_image { get; set; }
        public ImagePhoto campaign_photo_image { get; set; }

        public List<Question> question { get; set; }
        public List<ImagePhoto> campaign_gallery_image { get; set; }
    }
    public class Question
    {
        public int question_id { get; set; }
        public string campaign_id { get; set; }
        public int question_order { get; set; }
        public string question_order_text { get; set; }
        public string question_text { get; set; }
        public bool answer_required { get; set; }
        public int answer_type { get; set; }
        public bool require_option { get; set; }
        public string require_option_text { get; set; }
        //public List<string> choice { get; set; }
        public string choice_01 { get; set; }
        public string choice_02 { get; set; }
        public string choice_03 { get; set; }
        public string choice_04 { get; set; }
        public string choice_05 { get; set; }
        public string choice_06 { get; set; }
        public string choice_07 { get; set; }
        public string choice_08 { get; set; }
        public string choice_09 { get; set; }
        public string choice_10 { get; set; }

        public void loadDataQuestion(DataRow dr)
        {
            //question_id = int.Parse(dr["question_id"].ToString());
            question_id = dr["question_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["question_id"]); 
            //campaign_id = int.Parse(dr["campaign_id"].ToString());
            campaign_id = dr["campaign_id"] == DBNull.Value ? "" : dr["campaign_id"].ToString();
            //question_order = int.Parse(dr["question_order"].ToString());
            question_order = dr["question_order"] == DBNull.Value ? 0 : Convert.ToInt32(dr["question_order"]);
            question_order_text = dr["question_order_text"].ToString();
            question_text = dr["question_text"].ToString();
            //answer_required = Convert.ToBoolean(dr["answer_required"]);
            answer_required = dr["answer_required"] == DBNull.Value ? false : Convert.ToBoolean(dr["answer_required"]);
            //answer_type = int.Parse(dr["answer_type"].ToString());
            answer_type = dr["answer_type"] == DBNull.Value ? 0 : Convert.ToInt32(dr["answer_type"]);
            //require_option = Convert.ToBoolean(dr["require_option"]);
            require_option = dr["require_option"] == DBNull.Value ? false : Convert.ToBoolean(dr["require_option"]);
            require_option_text = dr["require_option_text"].ToString();
            choice_01 = dr["choice_01"].ToString();
            choice_02 = dr["choice_02"].ToString();
            choice_03 = dr["choice_03"].ToString();
            choice_04 = dr["choice_04"].ToString();
            choice_05 = dr["choice_05"].ToString();
            choice_06 = dr["choice_06"].ToString();
            choice_07 = dr["choice_07"].ToString();
            choice_08 = dr["choice_08"].ToString();
            choice_09 = dr["choice_09"].ToString();
            choice_10 = dr["choice_10"].ToString();
        }
    }
}
