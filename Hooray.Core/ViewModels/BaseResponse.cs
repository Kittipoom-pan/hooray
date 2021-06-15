namespace Hooray.Core.ViewModels
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
        }
        public BaseResponse(string message)
        {
            this.message = message;
            //data = data;
            //device_token_status = null;
            //status = data;
            //status_login = data;
            //startup_badge = data;
        }
        public T data { get; set; }
        public string message { get; set; }
        public bool device_token_status { get; set; }
        public bool status { get; set; }
        public bool status_login { get; set; }
        public StartupBadgeViewModel startup_badge { get; set; }
    }
}
