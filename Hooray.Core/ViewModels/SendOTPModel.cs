using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class VerifyAccountMessage
    {
        public string message { get; set; }

        public bool device_token_status { get; set; }

        public bool status { get; set; }

        public bool verify_mobile_status { get; set; }

        public string mobile_number { get; set; }
    }
    public class VerifyModel
    {
        public bool verify_mobile_status { get; set; }

        public string mobile_number { get; set; }
    }
}
