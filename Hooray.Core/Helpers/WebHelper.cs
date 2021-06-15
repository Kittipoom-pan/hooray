using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hooray.Core.Helpers
{
    public class WebHelper
    {
        public void Post(Uri url, string value, IDictionary<string, string> header = null, Action<string> OnComplete = null, Action<string> OnError = null, object p = null)
        {
            var request = HttpWebRequest.Create(url);
            var byteData = Encoding.ASCII.GetBytes(value);
            request.ContentType = "application/json";
            request.Method = "POST";

            if (header != null)
            {
                foreach (KeyValuePair<string, string> entry in header)
                {
                    request.Headers.Add(entry.Key, entry.Value);
                }
            }

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(byteData, 0, byteData.Length);
                }

                request.BeginGetResponse(new AsyncCallback((IAsyncResult) =>
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    OnComplete?.Invoke(responseString);
                }), null);

            }

            catch (WebException ex)
            {
                OnError?.Invoke(ex.ToString() + " " + url.AbsolutePath);
            }
        }

        public async Task<object> PostReturnValue(Uri url, string value, IDictionary<string, string> header = null)
        {
            var request = HttpWebRequest.Create(url);
            var byteData = Encoding.ASCII.GetBytes(value);
            request.ContentType = "application/json";
            request.Method = "POST";

            if (header != null)
            {
                foreach (KeyValuePair<string, string> entry in header)
                {
                    request.Headers.Add(entry.Key, entry.Value);
                }
            }

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(byteData, 0, byteData.Length);
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return (JsonConvert.DeserializeObject(result));
                }
            }

            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;

                using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return (JsonConvert.DeserializeObject(result));
                }
            }
        }

        public void Get(Uri url, Func<string, Task> OnComplete = null, Func<string, Task> OnError = null)
        {
            var request = HttpWebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                request.BeginGetResponse(new AsyncCallback((IAsyncResult) =>
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    OnComplete?.Invoke(responseString);
                }), null);
            }
            catch (WebException ex)
            {
                OnError?.Invoke(ex.ToString() + " " + url.AbsolutePath);
            }
        }
    }
}
