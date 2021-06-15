using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class DeleteUserPrizeModel
    {
       public int uid { get; set; }
       public List<string> upid { get; set; }
       public string lang { get; set; }
    }
}
