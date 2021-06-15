using Hooray.Core.Interfaces;
using Hooray.Infrastructure.DBContexts;
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
    public class IntroSplashScreenController : ControllerBase
    {
        private readonly devhoorayContext _context;
        private readonly IIntroSplashScreenServices _IntroSplashScreenServices;
        public IntroSplashScreenController( devhoorayContext context, IIntroSplashScreenServices IntroSplashScreenServices)
        {
            _IntroSplashScreenServices = IntroSplashScreenServices;
            _context = context;
        }

        /// <summary>
        /// รับข้อความแอพทั้งหมด
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tokenID"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("IntroSplashScreen")]
        [Route("GetDisplayTextUI")]
        public IActionResult GetDisplayTextUI([FromQuery]int uid, [FromQuery]string tokenID, [FromQuery]string lang)
        {
            try
            {
                var data = _IntroSplashScreenServices.GetDisplayTextUI(uid, tokenID , lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
