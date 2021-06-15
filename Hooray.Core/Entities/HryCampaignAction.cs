using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hooray.Core.Entities
{
    public partial class HryCampaignAction
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public int? EventType { get; set; }
        public string DeviceInfo { get; set; }
        public string DeviceOs { get; set; }
        public string UserId { get; set; }
        //public int? CampaignId { get; set; }
        public string CampaignId { get; set; }
        public string ShopName { get; set; }
        public string ImageName { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
