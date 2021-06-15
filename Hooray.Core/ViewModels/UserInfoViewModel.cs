using System;
using System.Data;

namespace Hooray.Core.ViewModels
{
    public class UserInfoViewModel
    {
        public int user_id { get; set; }
        public string display_name { get; set; }
        public string device_token { get; set; }
        public string device_type { get; set; }
        public bool is_online { get; set; }
        public string device_id { get; set; }
        public string image_name_profile { get; set; }
        public void loadDataUserInfo(DataRow dr)
        {
            user_id = int.Parse(dr["user_id"].ToString());
            display_name = dr["display_show_name"].ToString();
            device_type = dr["device_type"].ToString();
            is_online = Convert.ToBoolean(dr["is_online"]);
            device_token = dr["token_id"].ToString();
            device_id = dr["device_id"].ToString();
            image_name_profile = dr["image_name_profile"].ToString();
        }
        //public ImagePhoto user_image { get; set; }
        //public ImagePhoto user_image_full { get; set; }
    }
}
