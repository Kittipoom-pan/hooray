using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hooray.Infrastructure.DBContexts
{
    public partial class HryTagTopic
    {
        public int Id { get; set; }
        public string CampaignId { get; set; }
        public string TagTopicName { get; set; }
        public string TagTopicNameEn { get; set; }
        public string TagTopicNameTh { get; set; }
        public string TagTopicNameCh { get; set; }
        public string Image { get; set; }
        public string TagTopicDescription { get; set; }
        public string TagTopicDescriptionEn { get; set; }
        public string TagTopicDescriptionTh { get; set; }
        public string TagTopicDescriptionCh { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedById { get; set; }
    }
}
