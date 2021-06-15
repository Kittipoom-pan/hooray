using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class CompanyDetailModel
    {

        public int campaign_user_follow_id { get; set; }
        public int company_id { get; set; }
        public int is_new { get; set; }
        public string company_name { get; set; }
        public string company_information { get; set; }
        public string follow_date { get; set; }
        public string company_image_name { get; set; }

        public void loadDataCompanyFollow(DataRow dr)
        {
            campaign_user_follow_id = dr["campaign_user_follow_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["campaign_user_follow_id"]);
            //campaign_user_follow_id = int.Parse(dr["campaign_user_follow_id"].ToString());
            //company_id = int.Parse(dr["company_id"].ToString());
            company_id = dr["company_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["company_id"]);
            //is_new = int.Parse(dr["is_new"].ToString());
            is_new = dr["company_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["company_id"]);
            company_name = dr["company_name"].ToString();
            company_information = dr["company_information"].ToString();
            follow_date = Utility.convertToDateTimeServiceFormatString(dr["follow_date"].ToString());
            company_image_name = dr["company_image_name"].ToString();
        }
        public ImagePhoto company_logo_image { get; set; }
    }

    public class CompanyDetail
    {
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string company_information { get; set; }
        public string company_address { get; set; }
        public string company_tel { get; set; }
        public bool campaign_user_follow { get; set; }
        public int company_gallery_count { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string company_image_name { get; set; }
        public int company_gallery_no { get; set; }

        public void loadDataFollowCompanyDetail(DataRow dr)
        {
            company_id = int.Parse(dr["company_id"].ToString());
            company_name = dr["company_name"].ToString();
            company_information = dr["company_information"].ToString();
            company_address = dr["company_address"].ToString();
            company_tel = dr["company_tel"].ToString();
            campaign_user_follow = Convert.ToBoolean(int.Parse(dr["count_user_follow"].ToString()));
            company_gallery_count = int.Parse(dr["company_gallery_count"].ToString());
            latitude = double.Parse(dr["latitude"].ToString());
            longitude = double.Parse(dr["longitude"].ToString());
            company_image_name = dr["company_image_name"].ToString();
            company_gallery_no = int.Parse(dr["company_gallery_no"].ToString());
        }
        public ImagePhoto company_logo_image { get; set; }
        public List<ImagePhoto> company_gallery_image { get; set; }
    }
}
