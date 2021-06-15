namespace Hooray.Core.ViewModels
{
    public class StatusCodeResponseViewModel
    {
        public int status_code { get; set; }
        public Error error { get; set; }
        public StatusCodeResponseViewModel(int statusCode, Error error)
        {
            status_code = statusCode;
            this.error = error;
        }
    }
    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
        public Error(string message, string code)
        {
            this.code = code;
            this.message = message;
        }
    }
}
