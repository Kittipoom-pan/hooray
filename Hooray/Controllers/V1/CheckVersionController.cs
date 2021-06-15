using Hooray.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hooray.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[action]")]

    public class CheckVersionController : Controller
    {
        private readonly ICheckVersionService _checkVersionService;
        public CheckVersionController(ICheckVersionService checkVersionService)
        {
            _checkVersionService = checkVersionService;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("checkversion")]
        public async Task<IActionResult> CheckVersion(string versionapp, string devicetype, string lang)
        {
            try
            {
                var checkVersion = await _checkVersionService.CheckVersion(versionapp, devicetype, lang);

                return Ok(checkVersion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
