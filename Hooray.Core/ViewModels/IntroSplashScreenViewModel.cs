using Hooray.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace Hooray.Infrastructure.ViewModels
{
    class IntroSplashScreenViewModel
    {
    }
    public class HoorayMessageUI
    {
        [DataMember(Order = 0)]
        public string message { get; set; }

        [DataMember(Order = 1)]
        public bool status { get; set; }

        [DataMember(Order = 2)]
        public bool device_token_status { get; set; }

        [DataMember(Order = 3)]
        public List<MessageUI> messageUI { get; set; }
    }
    public class MessageUI
    {
        public int message_code { get; set; }
        public string message_text { get; set; }
        public void loadDataMessageUI(SystemMessage dr)
        {
            message_code = (int)dr.MessageCode;
            message_text = dr.MessageText;
        }
    }
}
