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
    public class ChangeController : ControllerBase
    {

        private readonly IChangeServices _changeServices;
        public ChangeController(IChangeServices changeServices)
        {
            _changeServices = changeServices;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("change")]
        [Route("create-account")]
        public async Task<IActionResult> CreateAccount([FromBody]CreateAccountModel model)
        {
            try
            {
                var data = await _changeServices.CreateAccount(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("change")]
        [Route("create-password")]
        public async Task<IActionResult> CreatePassword([FromBody] CreatePasswordModel model)
        {
            try
            {
                var data = await _changeServices.CreatePassword(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("change")]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var data = await _changeServices.Login(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("change")]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            try
            {
                var data = await _changeServices.ForgotPassword(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("change")]
        [Route("verify-forgotpassword")]
        public async Task<IActionResult> VerifyForgotPassword([FromBody] VerifyPasswordModel model)
        {
            try
            {
                var data = await _changeServices.VerifyForgotPassword(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
