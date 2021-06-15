using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.Entities
{
     public class HryOtpDetail
    {
        public int Id { get; set; }
        public int? OtpNumber { get; set; }
        public DateTime? OtpSendDate { get; set; }
        public DateTime? OtpVerifyDate { get; set; }
        public string OtpDetail { get; set; }
        public string MobileNo { get; set; }
        public bool? OtpVerify { get; set; }
        public int? OtpType { get; set; }
    }
}
