using Hooray.Core.AppsettingModels;
using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.Helpers;
using Hooray.Infrastructure.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hooray.Core.Services
{
    public class CampaignService : ICampaignService
    {
        private string clear = "";
        private int messagecode = 0;
        private readonly IOptions<UrlDownloadApp> _appSettings;
        private readonly ICampaignDetailRepository _campaignDetailRepository;
        private readonly IMySQLManagerRepository _mySQLManagerRepository;
        private IMySQLManager _msg;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<Resource> _option;
        private readonly IUriService _uriService;

        private readonly ILogger _logger;
        public CampaignService(ICampaignDetailRepository campaignDetailRepository, 
        IOptions<UrlDownloadApp> appSettings, IOptions<Resource> option,
        IMySQLManagerRepository mySQLManagerRepository, IUriService uriService,
        IMySQLManager msg, IHttpContextAccessor httpContextAccessor, ILogger<CampaignService> logger)
        {
            _logger = logger;
            _mySQLManagerRepository = mySQLManagerRepository;
            _appSettings = appSettings;
            _campaignDetailRepository = campaignDetailRepository;
            _msg = msg;
            _httpContextAccessor = httpContextAccessor;
            _option = option;
            _uriService = uriService;
        }

        public async Task<AllCampaignViewModel> GetAllCampaign(int user_id, string lang)
        {
            string host = _httpContextAccessor.HttpContext.Request.Host.Value;

            AllCampaignViewModel obj = new AllCampaignViewModel();
            obj.message = string.Empty;
            obj.status = true;
            obj.status_login = true;
            obj.device_token_status = true;
            obj.campaign = new List<Campaign>();
            obj.startup_badge = new StartupBadgeViewModel();
            messagecode = 0;

            try
            {
                if (user_id.ToString() != "0")
                {
                    if (user_id.ToString() == "0")
                    {
                        obj.status_login = false;
                    }

                    //UserInfoViewModel ui = new UserInfoViewModel();
                    var (userInfo, userDeviceType) = await _campaignDetailRepository.GetUserInformation(user_id);
                    string deviceType = userDeviceType;

                    obj.campaign = _campaignDetailRepository.GetAllCampaign(user_id, userDeviceType);
                    obj.url_android = _appSettings.Value.UrlAndroid;
                    obj.url_ios = _appSettings.Value.UrlIos;
                    obj.startup_badge = await _msg.GetStartupBadge(user_id);
                }
                else
                {
                    obj.status = false;
                    obj.device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                    obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("GetAllCampaign -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetAllCampaign");
            }

            return obj;
        }

        public async Task<PagedResponse<List<Campaign>>> GetAllFeedCampaign2(int user_id, string lang, int company_id, PaginationFilter pageFilter, string token, string route)
        {
            BaseResponse<List<Campaign>> obj = new BaseResponse<List<Campaign>>();

            int total = 0;
            var campaign = new List<Campaign>();
            obj.message = string.Empty;
            obj.status = true;
            obj.status_login = true;
            obj.device_token_status = true;
            messagecode = 0;
            var validFilter = new PaginationFilter(pageFilter.page_number, pageFilter.page_size);
            var startup_badge = new StartupBadgeViewModel();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Campaign>(campaign, validFilter, total, _uriService, route, obj.message, obj.device_token_status,
                    obj.status, obj.status_login, startup_badge);
            try
            {
                if (user_id.ToString() == "0" || clear == "")
                {
                    if (user_id.ToString() == "0")
                    {
                        obj.status_login = false;
                    }

                    //check campaign image type 20170315
                    UserInfo ui = new UserInfo();
                    ui = _mySQLManagerRepository.GetUserInformation2(user_id);
                    string deviceType = ui.device_type;

                    int perPage = (pageFilter.page_size == 0) ? int.Parse(_option.Value.PerPageShop) : pageFilter.page_size;
                    int pageNumber = (pageFilter.page_number == 0) ? 1 : pageFilter.page_number;
                    if (company_id ==0)
                    {
                        campaign = _campaignDetailRepository.GetAllFeedCampaign2(user_id, pageNumber, perPage, deviceType);
                        total = await _campaignDetailRepository.GetTotalCampaign2(user_id);
                        
                    }
                    else
                    {
                        campaign = _campaignDetailRepository.GetAllFeedCampaign3(user_id, company_id, pageNumber, perPage, deviceType);
                        total = await _campaignDetailRepository.GetTotalCampaign3(user_id, company_id);

                    }

                    pagedReponse = PaginationHelper.CreatePagedReponse<Campaign>(campaign, validFilter, total, _uriService, route, obj.message, obj.device_token_status,
                                obj.status, obj.status_login, startup_badge);
                }
                else
                {
                    obj.status = false;
                    obj.device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                    obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                    pagedReponse = PaginationHelper.CreatePagedReponse<Campaign>(campaign, validFilter, total, _uriService, route, obj.message, obj.device_token_status,
                                obj.status, obj.status_login, startup_badge);
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                pagedReponse = PaginationHelper.CreatePagedReponse<Campaign>(campaign, validFilter, total, _uriService, route, obj.message, obj.device_token_status,
                               obj.status, obj.status_login, startup_badge);
                _logger.LogError(ex, string.Format("GetAllFeedCampaign2 -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetAllCampaign");
            }

            return pagedReponse;
        }

        public async Task<HoorayUpdateLike> UpdateLikeCampaign(string user_id, string lang, int like_type, string campaign_id)
        {
            HoorayUpdateLike obj = new HoorayUpdateLike();
            obj.message = string.Empty;
            obj.status_login = true;
            obj.update_like = true;
            obj.status_like = false;
            obj.device_token_status = true;
            obj.campaign_id = 0;
            messagecode = 0;

            try
            {
                if (clear == "")
                {
                    int statusLike = await _campaignDetailRepository.InsertCampaignLike(campaign_id, user_id, like_type);
                    obj.status_like = Convert.ToBoolean(statusLike);

                    if (statusLike == 0)
                    {
                        int campaignLikeCount = await _campaignDetailRepository.GetLikeCampaign(campaign_id, like_type);
                        if (campaignLikeCount == 0)
                        {
                            campaignLikeCount = 1;
                        }
                        else
                        {
                            campaignLikeCount = campaignLikeCount + 1;
                        }

                        bool success = await _campaignDetailRepository.UpdateLikeCampaign(campaign_id, campaignLikeCount, like_type);
                        if (success)
                        {
                            obj.update_like = true;
                            obj.status_like = true;
                            obj.campaign_id = Convert.ToInt32(campaign_id);
                            messagecode = 301039; //Like Campaign Success
                        }
                    }
                    else
                    {
                        obj.update_like = false;
                        obj.campaign_id = Convert.ToInt32(campaign_id);
                        messagecode = 301047; //You can not like again
                    }
                }
                else
                {
                    obj.update_like = false;
                    obj.status_login = false;
                    obj.device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                }
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
            }
            catch (Exception ex)
            {
                obj.update_like = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");

                _logger.LogError(ex, string.Format("UpdateLikeCampaign -- {0}", obj.message));
            }
            return obj;
        }
    }
}
