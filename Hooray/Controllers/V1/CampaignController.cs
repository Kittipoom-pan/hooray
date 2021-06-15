using Hooray.Configuration;
using Hooray.Core.Entities;
using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using Hooray.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hooray.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[action]")]
    public class CampaignController : Controller
    {
        private readonly ICampaignDetailRepository _campaignDetailRepository;
        private readonly ICampaignActionRepository _campaignActionRepository;
        private readonly ICampaignService _campaignService;
        private readonly devhoorayContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUriService _uriService;
        private readonly IRedeemService _redeemService;
        public CampaignController(ICampaignDetailRepository campaignDetailRepository,ICampaignActionRepository campaignActionRepository,
            ICampaignService campaignService, devhoorayContext context, IHttpContextAccessor httpContextAccessor, IUriService uriService
            , IRedeemService redeemService)
        {
            _campaignDetailRepository = campaignDetailRepository;
            _campaignActionRepository = campaignActionRepository;
            _campaignService = campaignService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _uriService = uriService;
            _redeemService = redeemService;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("campaign")]
        [Route("{campaign_id}/detail/{user_id}")]
        public async Task<IActionResult> GetCampaignDetail(string campaign_id, string user_id, string lat, string lng, string event_name, int event_type)
        {
            await _campaignActionRepository.AddCampaignAction(campaign_id, user_id, lat, lng, event_name, event_type);
            var medias = _context.HryMedia.ToList();
            var result = await _context.HryCampaign
                                .Include(q => q.Photos)
                                .Include(c => c.Company)
                                .Where(q => q.CampaignId == campaign_id.ToString())
                                .Select(p => new
                                {
                                    company_name = p.Company.CompanyName,
                                    campaign_desc = p.CampaignDesc,
                                    campaign_short_desc = p.CampaignShortDesc,
                                    company_id = p.CompanyId,
                                    photo_companys = p.Photos.Where(q => q.Objects == "company"),
                                    photo_campaign = p.Photos.Where(p => p.Objects == "campaign").OrderBy(q => q.DisplayOrder).ToList()
                                })
                                .SingleOrDefaultAsync();

            if (result == null )
                return NotFound(new ResponseViewModel("Campaign not found"));

            var campaignViewModel = new CampaignViewModel()
            {
                company_name = result.company_name,
                campaign_desc = result.campaign_desc,
                campaign_short_desc = result.campaign_short_desc,
                company_id = Convert.ToInt32(result.company_id),
                photo_campaign = result.photo_campaign,
                photo_company = result.photo_companys.FirstOrDefault()
            };

            return Ok(campaignViewModel);
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("campaign")]
        [Route("{campaign_id}/join-user/{user_id}")]
        public async Task<IActionResult> JoinUserCampaign(string campaign_id, string user_id, string lat, string lng, int code_type, string display_name)
        {
            try
            {
                //var (canJoin,path) = await _campaignDetailRepository.CheckJoinUserCampaign(campaign_id, user_id);
                //var model = new UserJoinCampaignViewModel()
                //{
                //    user_join = canJoin,
                //    photo_result = path,
                //};
                var result = await _redeemService.ClientDeal(campaign_id, user_id, display_name);
                
                if (result == null) return NotFound(new { data = result });

                await _campaignActionRepository.AddUserJoinCampaign(campaign_id, user_id, lat, lng, code_type);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpGet]
        //[MapToApiVersion("1.0")]
        //[ActionName("campaign")]
        //[Route("{campaign_id}/result/{user_id}")]
        //public async Task<IActionResult> GetCampaignResult(int campaign_id, int user_id)
        //{
        //    try
        //    {
        //        var photo_result = await _campaignDetailRepository.GetUserCampaignResult(campaign_id, user_id);

        //        if (photo_result == null) return NotFound("Photo not found");

        //        return Ok(photo_result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpPost]
        [MapToApiVersion("1.0")]
        [ActionName("campaign")]
        [Route("{campaign_id}/confirm-scratch/{user_id}")]
        public async Task<IActionResult> ConfirmScratch(string campaign_id, string display_name, string user_id, string lat, string lng, string event_name, int event_type)
        {
            try
            {
                var user = new HryUserProfile();
                var result = new object();
                //int statusCode == null;
                //(var someString, var someInt)
                DealCodeRedeemConfig codeRedeem = new DealCodeRedeemConfig(_context);

                var deal = (from c in _context.HryCampaign
                              join cd in _context.HryCampaignDeal
                              on c.CampaignId equals cd.CampaignId
                              where cd.CampaignId == campaign_id
                              select new
                              {
                                  deal_id = cd.DealId
                              }).FirstOrDefault();

                var data = _context.HryUserProfile.Where(t => t.LineUserId == user_id.ToString()).FirstOrDefault();

                if (data == null)
                {
                    user = new HryUserProfile()
                    {
                        LineUserId = user_id,
                        DisplayFname = display_name,
                        CreateDate = DateTime.Now,
                        RegisterType = "Line"
                    };

                    _context.HryUserProfile.Add(user);
                    _context.SaveChanges();

                    (int statusCode, object response) = await codeRedeem.ClientCodeRedeem(deal.deal_id, user.DisplayFname, user.UserId);

                    if (statusCode == 400) return BadRequest(response);

                    await _campaignActionRepository.AddCampaignAction(campaign_id, user_id, lat, lng, event_name, event_type);

                    return Ok(response);
                }
                else
                {
                    (int statusCode, object response) = await codeRedeem.ClientCodeRedeem(deal.deal_id, data.DisplayFname, data.UserId);

                    if (statusCode == 400) return BadRequest(response);

                    await _campaignActionRepository.AddCampaignAction(campaign_id, user_id, lat, lng, event_name, event_type);

                    return Ok(response);
                }

                //var propertyInfo = result.GetType().GetProperty("status_code");
                //int statusCode = (int)(propertyInfo.GetValue(result, null));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("getallcampaign")]
        [Route("{user_id}")]
        public async Task<IActionResult> GetAllCampaign(int user_id, string lang)
        {
            try
            {
                var campaign = await _campaignService.GetAllCampaign(user_id, lang);
                return Json(new ResponseViewModel(campaign));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("getallfeedlist")]
        [Route("feed/{user_id}/{lang}")]
        public async Task<IActionResult> GetAllFeedList(int user_id, string lang,[FromQuery]PaginationFilter pageFilter)
        {
            try
            {
                var route = Request.Path.Value;
                var validFilter = new Core.ViewModels.PaginationFilter(pageFilter.page_number, pageFilter.page_size);
                var pagedData = await _context.HryCampaign
                               .Skip((validFilter.page_number - 1) * validFilter.page_size)
                               .Take(validFilter.page_size)
                               .ToListAsync();
                var startup_badge = new StartupBadgeViewModel();
                var totalRecords = await _context.HryCampaign.CountAsync();
                var pagedReponse = PaginationHelper.CreatePagedReponse<HryCampaign>(pagedData, validFilter, totalRecords, _uriService, route, "dd", true, true,true, startup_badge);
                
                return Ok(pagedReponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("getallfeedcampaignlist")]
        [Route("{user_id}/{lang}")]
        public async Task<IActionResult> GetAllFeedCampaign2(int user_id, string lang, int company_id, [FromQuery] PaginationFilter pageFilter)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["token"];
                var route = Request.Path.Value;
                var response = await _campaignService.GetAllFeedCampaign2(user_id, lang, company_id, pageFilter, token, route);

                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [MapToApiVersion("1.0")]
        [ActionName("campaign")]
        [Route("{campaign_id}/updatelikecampaign")]
        public async Task<IActionResult> UpdateLikeCampaign(string user_id, string lang, int like_type, string campaign_id)
        {
            try
            {
                var response = await _campaignService.UpdateLikeCampaign(user_id, lang, like_type, campaign_id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
