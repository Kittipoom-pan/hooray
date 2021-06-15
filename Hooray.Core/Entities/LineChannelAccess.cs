using System;

namespace Hooray.Core.Entities
{
    public partial class LineChannelAccess
    {
        public int Id { get; set; }
        public string ChannelAccessToken { get; set; }
        public string Environment { get; set; }
        public int? CompanyId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
