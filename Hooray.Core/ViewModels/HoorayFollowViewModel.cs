using System;
using System.Collections.Generic;
using System.Data;
using static Hooray.Core.ViewModels.CampaignJoinViewModel;

namespace Hooray.Core.ViewModels
{
    //public class HoorayFollowViewModel
    //{
    //    public string message { get; set; }
    //    public bool device_token_status { get; set; }
    //    public bool status { get; set; }
    //    public bool status_login { get; set; }
    //    public bool status_unfollow { get; set; }
    //    public List<CompanyFollow> company_follow { get; set; }
    //    public List<Join> campaignjoin { get; set; }
    //}

    public class CompanyFollow
    {
        public int campaign_user_follow_id { get; set; }
        public int company_id { get; set; }
        public int is_new { get; set; }
        public string company_name { get; set; }
        public string company_information { get; set; }
        public string follow_date { get; set; }
        public string company_image_name { get; set; }
        public void loadDataShopFollow(DataRow dr)
        {
            campaign_user_follow_id = int.Parse(dr["campaign_user_follow_id"].ToString());
            company_id = int.Parse(dr["company_id"].ToString());
            //is_new = Convert.ToBoolean(dr["is_new"] != DBNull.Value);
            is_new = dr["is_new"] != DBNull.Value ? Convert.ToInt32(dr["is_new"]) : 0;
            company_name = dr["company_name"].ToString();
            company_information = dr["company_information"].ToString();
            follow_date = Utility.convertToDateTimeServiceFormatString(dr["follow_date"].ToString());
            //company_image_name = dr["shop_image_name"].ToString();

        }
        public ImagePhoto shop_logo_image { get; set; }
    }
    public class HoorayFollow
    {
        public bool status_unfollow { get; set; }
        public CompanyFollow company_follow { get; set; }
        //public List<CompanyFollow> company_follow { get; set; }
        public Join campaignjoin { get; set; }
        //public List<Join> campaignjoin { get; set; }
    }
}
