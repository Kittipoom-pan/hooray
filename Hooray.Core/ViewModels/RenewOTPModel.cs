using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class RenewOTPModel
    {
    }
    [DataContract]
    public class ResetVerify
    {
        [DataMember(Order = 0)]
        public string message { get; set; }

        [DataMember(Order = 2)]
        public bool status { get; set; }

    }
}
