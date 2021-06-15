using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class UpdateAddressModel
    {
        public int uid { get; set; }
        public string address { get; set; }
        public string district { get; set; }
        public string amphor { get; set; }
        public string province { get; set; }
        public string zipcode { get; set; }
        public string lang { get; set; }
    }
}
