using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class GetOtpReprizeModel
    {
        public int uid { get; set; }
        public string tokenID { get; set; }
        public string lang { get; set; }
    }
    public class StatusOTPModel
    {
        public string otp { get; set; }
    }
}
