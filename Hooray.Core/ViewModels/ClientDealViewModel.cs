using System.Collections.Generic;

namespace Hooray.Core.ViewModels
{
    public class ClientDealViewModel
    {

        public int id { get; set; }
        public int campaign_id { get; set; }
        public string deal_image { get; set; }
        public string deal_name { get; set; }
        public string deal_image_th { get; set; }
        public int point { get; set; }
        public string deal_detail_promote { get; set; }
        public string deal_detail_promote_th { get; set; }
        public string deal_detail_short { get; set; }
        public string deal_detail_short_th { get; set; }
        public string deal_detail_condition { get; set; }
        public string deal_detail_condition_th { get; set; }
        public UserJoin user_join { get; set; }
        public DealCodeRedeem deal_code_redeem { get; set; }
    }
    public class UserJoin
    {
        public string photo_result { get; set; }
        public bool user_join { get; set; }
        public void loadData(bool userJoin, string photoResult)
        {
            photo_result = photoResult;
            user_join = userJoin;
        }
    }
    public class DealCodeRedeem
    {
        public int deal_id { get; set; }
        public string display_name { get; set; }
        public int user_id { get; set; }
        public void loadData(int dealId, string displayName, int userId)
        {
            deal_id = dealId;
            display_name = displayName;
            user_id = userId;
        }
    }
}
