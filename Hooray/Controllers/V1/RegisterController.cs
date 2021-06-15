using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
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
    public class RegisterController : ControllerBase
    {
        private readonly devhoorayContext _context;
        private readonly IRegisterServices _IregisterServices;
        
        public RegisterController(devhoorayContext context , IRegisterServices IregisterServices)
        {
            _context = context;
            _IregisterServices = IregisterServices;
        }

        /// <summary>
        /// ลงทะเบียนสมัครแคมเปญใหม่
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("Register")]
        [Route("RegisterCampaignApplicationNew")]
        public async Task<IActionResult> RegisterCampaignApplicationNew([FromBody]RegisterNewModel model)
        {
            try
            {
                var data = await _IregisterServices.RegisterCampaignApplicationNew(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
