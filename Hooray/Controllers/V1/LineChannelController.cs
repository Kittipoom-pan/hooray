using Hooray.Core.Interfaces;
using Hooray.Core.ModelRequests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Hooray.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[action]")]
    public class LineChannelController : Controller
    {
        private readonly ILineSendMessageService _lineSendMessageService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LineChannelController(ILineSendMessageService lineSendMessageService, IHttpContextAccessor httpContextAccessor)
        {
            _lineSendMessageService = lineSendMessageService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("line")]
        [Route("message/send")]
        public async Task<IActionResult> SendLineMessage(LineSendMessage lineSendMessageRequest)
        {
            try
            {
                string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

                var (statusCode, responseLine) = await _lineSendMessageService.SendMessageLine(lineSendMessageRequest, token);
                if (statusCode == 401)
                {
                    return Unauthorized(responseLine);
                }else if(statusCode == 400)
                {
                    return BadRequest(responseLine);
                }
                else
                {
                    return Json(responseLine);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}
