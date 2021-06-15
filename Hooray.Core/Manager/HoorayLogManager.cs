
using Hooray.Core.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Infrastructure.Manager
{
    public class HoorayLogManager
    {
        private static Log _log = null;

        public static Log FeyverLog
        {
            get
            {
                if (_log == null)
                {
                    _log = new Log();
                }

                return _log;
            }
        }
    }

}
