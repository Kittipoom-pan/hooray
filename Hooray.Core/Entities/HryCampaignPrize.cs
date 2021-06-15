using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hooray.Infrastructure.DBContexts
{
    public partial class HryCampaignPrize
    {
        public int PrizeId { get; set; }
        //public int? CampaignId { get; set; }
        public string CampaignId { get; set; }
        public int? MasterRewardNo { get; set; }
        public byte? IsSubReward { get; set; }
        public string PrizeName { get; set; }
        public string PrizeDetail { get; set; }
        public int? PrizeOrder { get; set; }
        public float? PrizeAmount { get; set; }
        public int? PrizeQty { get; set; }
        public int? PrizeQtyUsage { get; set; }
        public int? PrizeDigit { get; set; }
        public string ShareResultFbMessageWin { get; set; }
        public string ShareResultFbMessageLose { get; set; }
        public byte? LimitQtyByPercent { get; set; }
        public float? WinRate { get; set; }
        public int? PrizeReceiveType { get; set; }
        public int? PrizeAcceptType { get; set; }
        public string PrizeAcceptCondition { get; set; }
        public int? PrizeAcceptExpireDay { get; set; }
        public DateTime? PrizeAcceptExpireDate { get; set; }
        public int? PrizeAcceptExpireType { get; set; }
        public int? PrizeRedeemCodeAge { get; set; }
        public int? PrizeRedeemCodeDisplayType { get; set; }
        public string PrizeRemarkCaption { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? UpdateAnnounceDate { get; set; }
    }
}
