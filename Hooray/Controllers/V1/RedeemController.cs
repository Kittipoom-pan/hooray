using Hooray.Configuration;
using Hooray.Core.Interfaces;
using Hooray.Core.RequestModels;
using Hooray.Infrastructure.DBContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Hooray.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[action]")]
    public class RedeemController : Controller
    {
        private readonly devhoorayContext _context;
        private readonly IRedeemService _redeemService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RedeemController(devhoorayContext context, IRedeemService redeemService, IHttpContextAccessor httpContextAccessor)
        {
            _redeemService = redeemService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("client-deal")]
        public async Task<IActionResult> ClientDeal(string campaignId, string userId, string displayName)
        {
            try
            {
                var result = await _redeemService.ClientDeal(campaignId, userId, displayName);

                if (result == null) return NotFound(new { data = result });

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("client-user")]
        [Route("history/redeem")]
        public async Task<IActionResult> ClientUserHistoryRedeem(string userId, string displayName)
        {
            try
            {
                var result = await _redeemService.ClientUserHistoryRedeem(userId, displayName);

                if (result == null) return NotFound(new { data = result });

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //[HttpPost]
        //[MapToApiVersion("1.0")]
        //[ActionName("deal-code")]
        //[Route("redeem")]
        //public async Task<IActionResult> DealCodeRedeem([FromForm] UserLineRequest redeem)
        //{
        //    try
        //    {
        //        DealCodeRedeemConfig codeRedeem = new DealCodeRedeemConfig(_context);

        //        var result = await codeRedeem.ClientCodeRedeem(redeem);

        //        if (result == null) return NotFound(new { data = result });

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ActionName("unique-id")]
        public async Task<IActionResult> GetUniqueId(int campaign_id)
        {
            try
            {
                return Ok(Guid.NewGuid().ToString("N"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
