using System;

namespace Hooray.Core.ViewModels
{
    public class ResponseViewModel
    {
        public Object data { get; set; }

        public ResponseViewModel(Object data)
        {
            this.data = data;
        }
    }
}
