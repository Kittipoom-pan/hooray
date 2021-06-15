using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooray.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[action]")]
    public class PageFeedResourceController : ControllerBase
    {
        private readonly IPageFeedResource _pageFeedResource;
        public PageFeedResourceController(IPageFeedResource IPageFeedResource)
        {
            _pageFeedResource = IPageFeedResource;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("pagefeed-pesource")]
        [Route("addquestion-result")]
        public  IActionResult AddQuestionResult([FromBody] AddQuestionResultModel model)
        {
            try
            {
                var data = _pageFeedResource.AddQuestionResult(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
