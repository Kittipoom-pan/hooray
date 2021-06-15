using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class CreateAccountModel
    {
       public string mobile { get; set; }
        public string deviceID { get; set; }
        public string tokenID { get; set; }
        public string lang { get; set; }
        public string versionApp { get; set; }
        public string versionIOS { get; set; }
        public string versionAndroid { get; set; }
    }
}
