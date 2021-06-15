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
    public class HambergerMenuController : ControllerBase
    {
        private readonly IHambergerMenuServices _IHambergerMenuServices;
        public HambergerMenuController(IHambergerMenuServices IHambergerMenuServices)
        {
            _IHambergerMenuServices = IHambergerMenuServices;
        }

        /// <summary>
        /// แก้ไขที่อยู่
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("hambergermenu")]
        [Route("update-address")]
        public IActionResult UpdateAddress([FromBody]UpdateAddressModel model)
        {
            try
            {
                var data = _IHambergerMenuServices.UpdateAddress(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// แก้ไขข้อมูลผู้ใช้
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("hambergermenu")]
        [Route("update-userinformation")]
        public IActionResult UpdateUserInformation([FromBody]UpdateUserInformationModel model)
        {
            try
            {
                var data = _IHambergerMenuServices.UpdateUserInformation(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("hambergermenu")]
        [Route("control-usernotification")]
        public IActionResult ControlUserNotification([FromBody] ControlUserNotificationModel model)
        {
            try
            {
                var data = _IHambergerMenuServices.ControlUserNotification(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("hambergermenu")]
        [Route("getotp-reprize")]
        public IActionResult GetOtpReprize([FromBody] GetOtpReprizeModel model)
        {
            try
            {
                var data = _IHambergerMenuServices.GetOtpReprize(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("hambergermenu")]
        [Route("restore-prize")]
        public IActionResult RestorePrize([FromBody] RestorePrizeModel model)
        {
            try
            {
                var data = _IHambergerMenuServices.RestorePrize(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("hambergermenu")]
        [Route("add-feedback")]
        public IActionResult AddFeedback([FromBody] AddFeedbackModel model)
        {
            try
            {
                var data = _IHambergerMenuServices.AddFeedback(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("hambergermenu")]
        [Route("getmore-list")]
        public IActionResult GetMoreDetail(string market, string lang)
        {
            try
            {
                var data = _IHambergerMenuServices.GetMoreDetail(market, lang);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
