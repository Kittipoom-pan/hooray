using Hooray.Core.AppsettingModels;
using Hooray.Core.Helpers;
using Hooray.Core.Interfaces;
using Hooray.Core.RequestModels;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Hooray.Core.Services
{
    public class RedeemService : IRedeemService
    {
        private readonly IRedeemRepository _redeemRepository;
        private readonly IOptions<Resource> _appSetting;
        private readonly ICampaignDetailRepository _campaignDetailRepository;
        public RedeemService(IRedeemRepository redeemRepository, IOptions<Resource> appSetting, ICampaignDetailRepository campaignDetailRepository)
        {
            _redeemRepository = redeemRepository;
            _appSetting = appSetting;
            _campaignDetailRepository = campaignDetailRepository;
        }
        public async Task<object> ClientDeal(string campaignId, string userlineId, string displayName)
        {
            try
            {
                string url = "";
                if (string.IsNullOrEmpty(campaignId) || campaignId == null)
                {
                    url = string.Format("https://privilege-api.dev.fysvc.com/api/v1/client-deal");
                }
                else
                {
                    int dealId = await _redeemRepository.GetDeal(campaignId);
                    url = string.Format("https://privilege-api.dev.fysvc.com/api/v1/client-deal/{0}", dealId);
                }
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers.Add("x-api-key:" + _appSetting.Value.ApiKey);
                httpWebRequest.Headers.Add("x-api-key:" + "BgaPWiaxWjLxvsOxILkpirMqlhqPvCOqfGHoyoYwcIRXkDbdQsUMBjpzCZBONyES");
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ClientDealViewModel model = JsonConvert.DeserializeObject<ClientDealViewModel>(result.ToString());

                    model.user_join = await _campaignDetailRepository.CheckJoinUserCampaign(campaignId, "");

                    model.deal_code_redeem = await _redeemRepository.GetUserByLineId(campaignId , userlineId, displayName);

                    //model.deal_code_redeem = await _redeemRepository.GetDeal(campaignId);

                    return model;
                    //await _lineMessageRepository.AddSendMessageLog(id);
                    //return (JsonConvert.DeserializeObject(result));
                }
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;

                using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return (JsonConvert.DeserializeObject(result));
                }
            }
        }

        public async Task<object> ClientUserHistoryRedeem(string userlineId, string displayName)
        {
            try
            {
                HryUserProfile user = new HryUserProfile();
                string url = "";
                if (userlineId == null || string.IsNullOrEmpty(userlineId))
                {
                    url = string.Format("https://privilege-api.dev.fysvc.com/api/v1/client-user/history/redeem");
                }
                else
                {
                    user = await _redeemRepository.GetUser(userlineId, displayName);
                    url = string.Format("https://privilege-api.dev.fysvc.com/api/v1/client-user/history/redeem/{0}", user.UserId);
                }

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                //httpWebRequest.Headers.Add("x-api-key:" + _appSetting.Value.ApiKey);
                httpWebRequest.Headers.Add("x-api-key:" + "BgaPWiaxWjLxvsOxILkpirMqlhqPvCOqfGHoyoYwcIRXkDbdQsUMBjpzCZBONyES");
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    int userId = user.UserId;
                    return (JsonConvert.DeserializeObject(result));
                }

            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;

                using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return (JsonConvert.DeserializeObject(result));
                }
            }
        }

        public async Task<object> DealCodeRedeem(UserLineRequest model)
        {
            Uri url = new Uri("https://privilege-api.dev.fysvc.com/api/v1/client-deal/redeem");

            var user = _redeemRepository.GetUser(model.userLineId, model.displayName);
            ////int dealId = _redeemRepository.GetDeal(model.campaignId);
            //if (user.Result.DisplayFname == null)
            //{
            //    user.Result.DisplayFname = "No first name";
            //};

            var dealCodeRedeem = new DealCodeRedeemRequest(2389, user.Result.DisplayFname, user.Result.UserId);

            //var dealCodeRedeem = new DealCodeRedeemRequest(2389, "New", 291);

            WebHelper webHelper = new WebHelper();

            IDictionary<string, string> header = new Dictionary<string, string>();
            header.Add("x-api-key", "BgaPWiaxWjLxvsOxILkpirMqlhqPvCOqfGHoyoYwcIRXkDbdQsUMBjpzCZBONyES");
            string param = JsonConvert.SerializeObject(dealCodeRedeem);

            //webHelper.Post(url, param, null, null, error =>
            //{
            //    //LogManager.ServiceLog.WriteCustomLog("AddEvent TreasureData", error);
            //});
            var result = webHelper.PostReturnValue(url, param, header);

            return result;
        }
    }
}
