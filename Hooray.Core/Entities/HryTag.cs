using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Hooray.Infrastructure.DBContexts
{
    public partial class HryTag
    {
        public int Id { get; set; }
        public int? TagTopicId { get; set; }
        public string TagName { get; set; }
        public string TagNameEn { get; set; }
        public string TagNameTh { get; set; }
        public string TagNameCh { get; set; }
        public string Image { get; set; }
        public string ImageHover { get; set; }
        public string TagDescription { get; set; }
        public string TagDescriptionEn { get; set; }
        public string TagDescriptionTh { get; set; }
        public string TagDescriptionCh { get; set; }
        public int? TagWeight { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedById { get; set; }
    }
}
