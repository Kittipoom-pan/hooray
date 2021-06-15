using Hooray.Core.Interfaces;
using Hooray.Core.RequestModels;
using Hooray.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hooray.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[action]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("adduser-follow")]
        public async Task<IActionResult> AddUserFollow(AddUserFollowRequest model)
        {
            try
            {
                //string token = Request.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault();
                var userFollow = await _userService.AddUserFollow(model);

                return Ok(userFollow);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("user")]
        [Route("getall-userresult")]
        public async Task<IActionResult> GetAllUserResult(int uid, int join, int announcetype, string lang, [FromQuery] PaginationFilter pageFilter)
        {
            try
            {
                var route = Request.Path.Value;
                var userFollow = await _userService.GetAllUserResult(uid, join, announcetype, lang, pageFilter, route);

                return Ok(userFollow);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
