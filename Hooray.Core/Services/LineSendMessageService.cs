using Hooray.Core.AppsettingModels;
using Hooray.Core.Interfaces;
using Hooray.Core.ModelRequests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Hooray.Core.Services
{
    public class LineSendMessageService : ILineSendMessageService
    {
        private readonly IOptions<LineMessages> _appSettings;
        private readonly IOptions<Resource> _resource;
        private readonly ILineMessageRepository _lineMessageRepository;
        private int messagecode = 0;
        private readonly ILogger _logger;
        public LineSendMessageService(IOptions<LineMessages> appSettings, ILineMessageRepository lineMessageRepository, 
            ILogger<LineSendMessageService> logger, IOptions<Resource> resource)
        {
            _logger = logger;
            _appSettings = appSettings;
            _lineMessageRepository = lineMessageRepository;
            _resource = resource;
        }
        public async Task<(int,object)> SendMessageLine(LineSendMessage model, string token)
        {
            try
            {
                string messages = _appSettings.Value.MessageBody.Text;
                string type = _appSettings.Value.MessageBody.Type;

                //await _lineMessageRepository.AddLineChannel(token, model.companyId);
                
                //string lineChannelToken = await _lineMessageRepository.GetTokenLine(_resource.Value.Environment);
                string lineChannelToken = await _lineMessageRepository.GetTokenLine("development");
                string url = "https://api.line.me/v2/bot/message/push";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers.Add("Authorization:" + String.Format("Bearer {0}", token));
                httpWebRequest.Headers.Add("Authorization:" + lineChannelToken);
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    Messages msg = new Messages(model.message.ToString(), type);

                    List<Messages> list = new List<Messages>();

                    list.Add(msg);

                    LineSendMessageRequest request = new LineSendMessageRequest(model.userId, list);

                    request.messages = list;

                    string json = JsonConvert.SerializeObject(request);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    await _lineMessageRepository.AddSendMessageLog(model.userId);
                    return (200,JsonConvert.DeserializeObject(result));
                }

            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;

                using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return ((int)response.StatusCode, JsonConvert.DeserializeObject(result));
                }
            }
        }
    }
}
