using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hooray.Core.Entities
{
    public class SystemMessage
    {
        public int Id { get; set; }
        public int? MessageCode { get; set; }
        public Int16? MessageType { get; set; }
        public string MessageText { get; set; }
        public string MessageDesc { get; set; }
        public string MessageLang { get; set; }
        public string UiNameIos { get; set; }
        public string UiNameAndroid { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
