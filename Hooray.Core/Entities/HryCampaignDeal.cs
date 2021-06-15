using System;
using System.ComponentModel.DataAnnotations;

namespace Hooray.Core.Entities
{
    public class HryCampaignDeal
    {
        [Key]
        public int DealId { get; set; }
        public string CampaignId { get; set; }
        public string DealBuyDetail { get; set; }
        public string DealUsedDetail { get; set; }
        //public byte? IsNew { get; set; }
        public DateTime? DealOnJoinExpireDate { get; set; }
        public int DealOnResultHour { get; set; }
        public int DealGalleryCount { get; set; }
        public int LimitBuyDeal { get; set; }
        public int UsedBuyDeal { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
