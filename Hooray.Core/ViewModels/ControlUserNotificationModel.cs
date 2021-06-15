using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class ControlUserNotificationModel
    {
        public int uid { get; set; }
        public int status { get; set; }
        public string tokenID { get; set; }
        public string lang { get; set; }
    }
    public class StatusPushModel
    {
        public bool status_push { get; set; }
    }

}
