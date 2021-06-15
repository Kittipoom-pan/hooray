using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hooray.Infrastructure.DBContexts
{
    public partial class HryCompanyBranch
    {
        public int BranchId { get; set; }
        //public int? CampaignId { get; set; }
        public string CampaignId { get; set; }
        public string BranchName { get; set; }
        public string BranchDisplayname { get; set; }
        public string BranchDesc { get; set; }
        public string BranchAddress { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public int? BranchOrder { get; set; }
    }
}
