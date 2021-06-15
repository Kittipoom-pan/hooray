using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class StoreConfigModel
    {
        public string store_name { get; set; }
        public string desc_short { get; set; }
        public string description { get; set; }
        public string ios_app_url { get; set; }
        public string android_app_url { get; set; }
        public string icon_app_url { get; set; }
        public string package_name { get; set; }
        public void loadDataStoreConfig(DataRow dr)
        {
            store_name = dr["name"].ToString();
            desc_short = dr["desc_short"].ToString();
            description = dr["description"].ToString();
            ios_app_url = dr["ios_app_url"].ToString();
            android_app_url = dr["android_app_url"].ToString();
            icon_app_url = dr["icon_app_url"].ToString();
            package_name = dr["package_name"].ToString();
        }
    }
}
