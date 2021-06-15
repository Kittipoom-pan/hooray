using System;
using System.Text;

namespace Hooray.Core.Core
{
    public class Utility
    {
        static string WS_DATETIME_FORMAT = "dd/MM/yyyy HH:mm";
        static string WS_DATE_FORMAT = "dd/MM/yyyy";

        public static string convertToDateServiceFormatString(string input)
        {
            DateTime result;

            bool isPass = DateTime.TryParse(input, out result);

            if (!isPass)
                result = DateTime.Now;

            return result.ToString(WS_DATE_FORMAT);
        }

        public static string convertToDateTimeServiceFormatString(string input)
        {
            DateTime result;

            bool isPass = DateTime.TryParse(input, out result);

            if (!isPass)
                result = DateTime.Now;

            return result.ToString(WS_DATETIME_FORMAT);
        }

        public static string EscapeStringValue(string value)
        {
            const char BACK_SLASH = '\\';
            const char SLASH = '/';
            const char DBL_QUOTE = '"';

            var output = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                switch (c)
                {
                    case SLASH:
                        output.AppendFormat("{0}{1}", BACK_SLASH, SLASH);
                        break;

                    case BACK_SLASH:
                        output.AppendFormat("{0}{0}", BACK_SLASH);
                        break;

                    case DBL_QUOTE:
                        output.AppendFormat("{0}{1}", BACK_SLASH, DBL_QUOTE);
                        break;

                    default:
                        output.Append(c);
                        break;
                }
            }

            return output.ToString();
        }

        public static double GetRandomNumberDouble(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
