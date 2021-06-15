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
    public class PrizeResultTabController : ControllerBase
    {
        private readonly IPrizeResultTabServices _IPrizeResultTabServices;
        public PrizeResultTabController(IPrizeResultTabServices IPrizeResultTabServices)
        {
            _IPrizeResultTabServices = IPrizeResultTabServices;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("prizeresulttab")]
        [Route("getprizelist")]
        public IActionResult GetPrizeList(int uid, string lang)
        {
            try
            {
                var data =  _IPrizeResultTabServices.GetPrizeList(uid, lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("prizeresulttab")]
        [Route("getprizedetail")]
        public IActionResult GetPrizeDetail(int uid, int upid, string lang)
        {
            try
            {
                var data = _IPrizeResultTabServices.GetPrizeDetail(uid, upid, lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [MapToApiVersion("1.0")]
        [ActionName("prizeresulttab")]
        [Route("delete-userprize")]
        public IActionResult DeleteUserPrize([FromBody]DeleteUserPrizeModel model)
        {
            try
            {
                var data = _IPrizeResultTabServices.DeleteUserPrize(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
