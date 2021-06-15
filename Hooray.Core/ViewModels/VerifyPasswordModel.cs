using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class VerifyPasswordModel
    {
        public string mobile { get; set; }
        public int verifycode { get; set; }
        public string lang { get; set; }
    }
}
