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
    public class SendOTPController : ControllerBase
    {
        private readonly ISendOTPServices _ISendOTPServices;
        public SendOTPController(ISendOTPServices ISendOTPServices)
        {
            _ISendOTPServices = ISendOTPServices;
        }
        /// <summary>
        /// ตรวจสอบบัญชี
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="verifyCode"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("SendOTP")]
        [Route("VerifyAccount")]
        public IActionResult VerifyAccount([FromQuery]string deviceID, [FromQuery]string verifyCode, [FromQuery]string lang)
        {
            try
            {
                var data = _ISendOTPServices.VerifyAccount( deviceID,  verifyCode, lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
