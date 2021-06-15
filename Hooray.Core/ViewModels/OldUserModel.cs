using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class OldUserModel
    {
        public int user_id { get; set; }
        public string token_id { get; set; }
        public string device_type { get; set; }
        public bool status_prize { get; set; }
        public void loadDataOldUser(DataRow dr)
        {
            user_id = dr["oldUserID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["oldUserID"]);
            token_id = dr["token_id"].ToString();
            device_type = dr["device_type"].ToString();
            status_prize = dr["statusPrize"] == DBNull.Value ? false : Convert.ToBoolean(dr["statusPrize"]);
        }
    }
}
