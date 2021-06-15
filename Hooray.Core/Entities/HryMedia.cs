using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Hooray.Core.Entities
{
    public partial class HryMedia
    {
        public long Id { get; set; }
        public string Objects { get; set; }
        public string ObjectId { get; set; }
        public string Path { get; set; }
        public int? DisplayOrder { get; set; }
        public string MediaType { get; set; }
        //[JsonIgnore]
        //[NotMapped]
        //public int ObjectIdInt { get { return Convert.ToInt32(ObjectId); } }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual HryCampaign HryCampaign { get; set; }
        //[JsonIgnore]
        //[IgnoreDataMember]
        //public virtual HryCompany HryCompany { get; set; }
    }
}
