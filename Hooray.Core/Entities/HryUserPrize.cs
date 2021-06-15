using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hooray.Infrastructure.DBContexts
{
    public partial class HryUserPrize
    {
        public int UserPrizeId { get; set; }
        public int? PrizeId { get; set; }
        //public int? CampaignId { get; set; }
        public string CampaignId { get; set; }
        //public int? UserId { get; set; }
        public string UserId { get; set; }
        public int? CampaignUserCodeId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string Amphor { get; set; }
        public string Province { get; set; }
        public string Zipcode { get; set; }
        public string PrizeRemark { get; set; }
        public byte? AddressPrizeComplete { get; set; }
        public DateTime? AcceptDate { get; set; }
        public byte? IsCancel { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string PrizeRedeemCode { get; set; }
        public DateTime? PrizeRedeemCodeExpire { get; set; }
        public DateTime? CancelDate { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
