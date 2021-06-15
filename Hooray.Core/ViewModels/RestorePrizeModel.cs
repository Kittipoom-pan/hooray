using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class RestorePrizeModel
    {
        public  int uid { get; set; }
        public string fbid { get; set; }
        public string otpcode { get; set; }
        public string tokenID { get; set; }
        public string lang { get; set; }
    }
}
