using System;

namespace Hooray.Core.Entities
{
    public class ClientDealRedeemLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DealId { get; set; }
        public string DisplayName { get; set; }
        public string Param { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
