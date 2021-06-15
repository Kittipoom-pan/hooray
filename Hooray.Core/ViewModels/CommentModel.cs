using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class CommentModel
    {
       
        public int comment_id { get; set; }
        public string campaign_id { get; set; }
        public int user_id { get; set; }
        public string display_name { get; set; }
        public string comment_text { get; set; }
        public string comment_time { get; set; }
        public bool is_image { get; set; }
        public string image_name_profile { get; set; }

        //public void loadDataComment(DataRow dr)
        //{
        //    comment_time = Utility.convertToDateTimeServiceFormatString(dr["comment_time"].ToString());
        //    comment_id = int.Parse(dr["comment_id"].ToString());
        //    user_id = int.Parse(dr["user_id"].ToString());
        //    campaign_id = int.Parse(dr["campaign_id"].ToString());
        //    display_name = dr["display_show_name"].ToString();
        //    is_image = int.Parse(dr["is_image"].ToString());
        //    image_name_profile = dr["image_name_profile"].ToString();

        //    if (is_image == 1)
        //    {
        //        comment_text = string.Format(WebConfigurationManager.AppSettings["comment_photo_path"], WebConfigurationManager.AppSettings["resource_ip"], dr["comment_image"].ToString());
        //    }
        //    else
        //    {
        //        comment_text = System.Web.HttpUtility.UrlDecode(dr["comment_text"].ToString());
        //    }
        //}
        public ImagePhoto user_image_round { get; set; }
        public ImagePhoto user_image_full { get; set; }
    }
}
