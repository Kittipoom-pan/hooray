using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class loadDataRegisterModel
    {
        public string display_birthday { get; set; }
        public int user_id  { get; set; }
        public string display_show_name { get; set; }
        public string display_email { get; set; }
        public string display_fname { get; set; }
        public string display_lname { get; set; }
        public string display_gender { get; set; }
        public string display_mobile { get; set; }
        public string user_lang { get; set; }
        public bool is_search_friend { get; set; }
        public bool require_mobile_verify { get; set; }
        public string image_name_profile { get; set; }
        //public int old_userid { get; set; }
    }
}
