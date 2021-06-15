using System;
using System.Data;

namespace Hooray.Core.ViewModels
{
    public class CheckCampaignViewModel
    {
            public bool required_facebook { get; set; }
            public bool required_share_feed { get; set; }
            public bool status_like_fanpage { get; set; }
            public bool status_share_feed { get; set; }
            public int valid { get; set; }
            public int join_expire { get; set; }
            public void loadDataCheckCampaign(DataRow dr)
            {
                required_facebook = Convert.ToBoolean(dr["required_facebook"]);
                required_share_feed = Convert.ToBoolean(dr["required_share_feed"]);
                status_like_fanpage = Convert.ToBoolean(dr["status_like_fanpage"]);
                status_share_feed = Convert.ToBoolean(dr["status_share_feed"]);
                valid = int.Parse(dr["valid"].ToString());
                join_expire = int.Parse(dr["join_expire"].ToString());
            }
        
    }
}
