using System;
using System.Data;

namespace Hooray.Core.ViewModels
{
    public class CampaignPrizeViewModel
    {
        public int prize_id { get; set; }
        public int campaign_id { get; set; }
        public string prize_name { get; set; }
        public string prize_detail { get; set; }
        public double prize_amount { get; set; }
        public int prize_qty { get; set; }
        public int prize_qty_usage { get; set; }
        public int prize_order { get; set; }
        public double win_rate { get; set; }

        public void loadDataPrize(DataRow dr)
        {
            prize_id = dr["prize_id"] != DBNull.Value ? Convert.ToInt32(dr["prize_id"]) : 0;
            campaign_id = dr["campaign_id"] != DBNull.Value ? Convert.ToInt32(dr["campaign_id"]) : 0;
            prize_name = dr["prize_name"].ToString();
            prize_detail = dr["prize_detail"].ToString();
            prize_amount = dr["prize_amount"] != DBNull.Value ? double.Parse(dr["prize_amount"].ToString()) : 0.0;
            prize_qty = dr["prize_qty"] != DBNull.Value ? Convert.ToInt32(dr["prize_qty"]) : 0;
            prize_qty_usage = dr["prize_qty_usage"] != DBNull.Value ? Convert.ToInt32(dr["prize_qty_usage"]) : 0;
            prize_order = dr["prize_order"] != DBNull.Value ? Convert.ToInt32(dr["prize_order"]) : 0;
            win_rate = dr["win_rate"] != DBNull.Value ? double.Parse(dr["win_rate"].ToString()) : 0.0;
        }
    }
}
