namespace Hooray.Core.RequestModels
{
    public class AddUserFollowRequest
    {
        public int user_id { get; set; }
        public int company_id { get; set; }
        public string campaign_id { get; set; }
        public int join { get; set; }
        public int unfollow { get; set; }
        //public string token { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
        public string lang { get; set; }
    }
}
