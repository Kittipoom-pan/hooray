using Hooray.Core.Interfaces;
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
    public class JoinOnFeedTabController : ControllerBase
    {
        private readonly IJoinOnFeedTabServices _joinon;
        public JoinOnFeedTabController(IJoinOnFeedTabServices joinOnFeedTab)
        {
            _joinon = joinOnFeedTab;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("joinonfeedtab")]
        [Route("addjoincampaign")]
        public IActionResult AddJoinCampaign([FromQuery]int uid, [FromQuery] string cpid, [FromQuery] int sid, [FromQuery] string tokenID, [FromQuery] float lat, [FromQuery] float lng, [FromQuery] string lang)
        {
            try
            {
                var data = _joinon.AddJoinCampaign( uid,  cpid,  sid,  tokenID,  lat,  lng,  lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("joinonfeedtab")]
        [Route("getcampaignresult")]
        public IActionResult GetCampaignResult([FromQuery] int uid, [FromQuery] string cpid, [FromQuery] string tokenID, [FromQuery] string lang)
        {
            try
            {
                var data = _joinon.GetCampaignResult( uid,  cpid,  tokenID,  lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
