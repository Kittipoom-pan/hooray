using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class AddQuestionResultModel
    {
        public int uid { get; set; }
        public string qtid { get; set; }
        public int cucid { get; set; }
        public string answer { get; set; }
        public string answeroption { get; set; }
        public string tokenID { get; set; }
        public string lang { get; set; }
    }
}
