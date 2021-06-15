using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class UserInfoForEditModel
    {
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public string birthday { get; set; }
        public string gender { get; set; }
        public string mobile { get; set; }
        public bool status_mobile { get; set; }
        public bool mobile_verify_status { get; set; }
        public string device_id { get; set; }
        public string image_name_profile { get; set; }
        public int old_user_id { get; set; }

        public void loadDataUserForEdit(DataRow dr)
        {
            user_id = dr["user_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["user_id"]);
            first_name = dr["display_fname"].ToString();
            last_name = dr["display_lname"].ToString();
            display_name = dr["display_show_name"].ToString();
            email = dr["display_email"].ToString();
            birthday = Utility.convertToDateServiceFormatString(dr["display_birthday"].ToString());
            gender = dr["display_gender"].ToString();
            mobile = dr["display_mobile"].ToString();
            status_mobile = dr["status_mobile"] == DBNull.Value ? false : Convert.ToBoolean(dr["status_mobile"]);
            device_id = dr["device_id"].ToString();
            image_name_profile = dr["image_name_profile"].ToString();
            old_user_id = dr["old_userid"] == DBNull.Value ? 0 : Convert.ToInt32(dr["old_userid"]);
        }

        public ImagePhoto user_image_round { get; set; }
        public ImagePhoto user_image_round_full { get; set; }
        public ImagePhoto user_image { get; set; }
        public ImagePhoto user_image_full { get; set; }
    }
}
