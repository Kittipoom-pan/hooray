using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hooray.Infrastructure.DBContexts
{
    public partial class HryUserJoin
    {
        public int Id { get; set; }
        public string CampaignId { get; set; }
        //public int? CampaignId { get; set; }
        public string UserId { get; set; }
        public int? CodeType { get; set; }
        public string SmsText { get; set; }
        public string AnswerForJoin { get; set; }
        public byte? IsRead { get; set; }
        public byte? IsVerifyPrize { get; set; }
        public string SmsImage { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public double? RawRandomNumber { get; set; }
        public DateTime? RawRandomDate { get; set; }
        public int? IsWin { get; set; }
        public int? PrizeId { get; set; }
        public string ResultImage { get; set; }
        public string ResultImageName { get; set; }
        public byte? CheckAnnounce { get; set; }
        public DateTime? CheckAnnounceDate { get; set; }
        public int? RegisterFacebookId { get; set; }
        public int? RegisterFeyverId { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public byte? HasWinInCampaign { get; set; }
        public byte? HasShared { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
