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
    public class RenewOTPController : ControllerBase
    {
        private readonly IRenewOTPServices _renew;
        public RenewOTPController(IRenewOTPServices renewOTP)
        {
            _renew = renewOTP;
        }
        /// <summary>
        /// รีเซ็ตยืนยันบัญชี
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("RenewOTP")]
        [Route("ResetVerifyAccount")]
        public IActionResult ResetVerifyAccount([FromQuery]string deviceId, [FromQuery]string lang)
        {
            try
            {
                var data = _renew.ResetVerifyAccount( deviceId, lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
