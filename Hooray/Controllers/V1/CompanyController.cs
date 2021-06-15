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
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("company")]
        [Route("getFollow-company")]
        public async Task<IActionResult> GetFollowCompany(int uid, int follow,string lang , [FromQuery] PaginationFilter pageFilter)
        {
            try
            {
                var route = Request.Path.Value;
                var userFollow = await _companyService.GetFollowCompany(uid, follow, lang, pageFilter, route);
                return Ok(userFollow);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("company")]
        [Route("getallcampaign-followcompanypage")]
        public  IActionResult GetAllCampaignFollowCompanyPage(int uid, int sid, string lang, [FromQuery] PaginationFilter pageFilter)
        {
            try
            {
                var route = Request.Path.Value;
                var userFollow = _companyService.GetAllCampaignFollowCompanyPage(uid, sid, lang, pageFilter, route);

                return Ok(userFollow);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("company")]
        [Route("getall-companyfollowcampaignlist")]
        public IActionResult GetAllShopFollowCampaignList(string uid, string sid,  string lang ,[FromQuery] PaginationFilter pageFilter)
        {
            try
            {
                var route = Request.Path.Value;
                var userFollow = _companyService.GetAllCompanyFollowCampaignList(uid, sid, lang, pageFilter, route);

                return Ok(userFollow);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("company")]
        [Route("getfollow-companydetail")]
        public IActionResult GetFollowCompanyDetail(int uid, int sid, string lang)
        {
            try
            {
                var userFollow = _companyService.GetFollowCompanyDetail(uid, sid, lang);

                return Ok(userFollow);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
