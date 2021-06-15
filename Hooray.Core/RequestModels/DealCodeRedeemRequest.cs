namespace Hooray.Core.RequestModels
{
    public class DealCodeRedeemRequest
    {
        public int deal_id { get; set; }
        public string requested_by { get; set; }
        public int requested_by_id { get; set; }
        public DealCodeRedeemRequest(int dealId, string displayName, int userId)
        {
            deal_id = dealId;
            requested_by = displayName;
            requested_by_id = userId;
        }
    }
}
