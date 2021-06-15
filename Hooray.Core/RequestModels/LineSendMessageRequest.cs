using System.Collections.Generic;

namespace Hooray.Core.ModelRequests
{
    public class LineSendMessageRequest
    {
        public string to { get; set; }
        //public string message { get; set; }
        public List<Messages> messages { get; set; }
        public LineSendMessageRequest(string user_id, List<Messages> messages)
        {
            this.to = user_id;
            this.messages = messages;
        }
    }

    public class Messages
    {
        public string type { get; set; }
        public string text { get; set; }

        public Messages(string message, string type)
        {
            this.type = type;
            this.text = message;
        }
    }

    public class LineSendMessage
    {
        public string userId { get; set; }
        public string message { get; set; }
        public int companyId { get; set; }
        public string token { get; set; }
    }
}
