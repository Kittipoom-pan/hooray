using System;
using System.Data;

namespace Hooray.Core.ViewModels
{
    public class CampaignJoinViewModel
    {
        public class Join
        {
            public string campaign_id { get; set; }
            public int id { get; set; }
            public string create_date { get; set; }
            public int announce_type { get; set; }
            public string join_number { get; set; }
            public string announce_text { get; set; }
            public int result { get; set; }
            public string announce_date { get; set; }
            public int count_user { get; set; }
            public int total_join { get; set; }
            public int current_join { get; set; }
            public int prize_receive_type { get; set; }
            public bool require_prize_otp { get; set; }

            public void loadDataJoin(DataRow dr, double pEnjoyNumber)
            {
                join_number = pEnjoyNumber.ToString();
                id = int.Parse(dr["id"].ToString());
                campaign_id = dr["campaign_id"].ToString();
                create_date = Utility.convertToDateServiceFormatString(dr["create_date"].ToString());
                //announce_type = int.Parse(dr["announce_type"].ToString());
                announce_type = dr["announce_type"] != DBNull.Value ? Convert.ToInt32(dr["announce_type"]) : 0; 
                //count_user = int.Parse(dr["count_user"].ToString());
                count_user = dr["count_user"] != DBNull.Value ? Convert.ToInt32(dr["count_user"]) : 0;
                announce_date = Utility.convertToDateTimeServiceFormatString(dr["announce_date"].ToString());
                //total_join = int.Parse(dr["total_join"].ToString());
                total_join =  dr["total_join"] != DBNull.Value ? Convert.ToInt32(dr["total_join"]) : 0;
                //current_join = int.Parse(dr["current_join"].ToString());
                current_join = dr["current_join"] != DBNull.Value ? Convert.ToInt32(dr["current_join"]) : 0;
                //prize_receive_type = int.Parse(dr["prize_receive_type"].ToString());
                prize_receive_type = dr["prize_receive_type"] != DBNull.Value ? Convert.ToInt32(dr["prize_receive_type"]) : 0;
                //require_prize_otp = Convert.ToBoolean(dr["require_prize_otp"]);
                require_prize_otp = dr["require_prize_otp"] != DBNull.Value ? Convert.ToBoolean(dr["require_prize_otp"]) : false;
            }
        }
    }
}
