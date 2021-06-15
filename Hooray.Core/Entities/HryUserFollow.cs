using System;

namespace Hooray.Core.Entities
{
    public partial class HryUserFollow
    {
        public int CampaignUserFollowId { get; set; }
        public int CompanyId { get; set; }
        //public int UserId { get; set; }
        public string UserId { get; set; }
        public byte? IsNew { get; set; }
        public DateTime? FollowDate { get; set; }
        public DateTime? UnFollowDate { get; set; }
    }
}
