using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class AddFeedbackModel
    {
       public int uid { get; set; }
        public string feedback { get; set; }
        public string tokenID { get; set; }
        public string lang { get; set; }
    }
}
