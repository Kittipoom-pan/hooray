using Hooray.Core.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Hooray.Infrastructure.DBContexts
{
    public partial class HryCompany
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyInformation { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContactInfo { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string ContactInfo { get; set; }
        public byte? ApprovalRequired { get; set; }
        public string LoginEmail { get; set; }
        public string LoginPassword { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual HryCampaign HryCampaign { get; set; }
        //[NotMapped]
        //public ICollection<HryMedia> Photos { get; set; }
    }
}
