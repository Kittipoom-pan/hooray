using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hooray.Core.Services
{
    public class CompanyService : ICompanyService
    {
        private IMySQLManagerRepository _sql;
        private IMySQLManager _msg;
        private int messagecode = 0;
        private string clear = "";
        private readonly ILogger _logger;
        private readonly IUriService _uriService;
        public CompanyService(IMySQLManagerRepository mySQLManagerRepository, IMySQLManager mySQLManager, ILogger<CompanyService> logger, IUriService uriService)
        {
            _logger = logger;
            _sql = mySQLManagerRepository;
            _msg = mySQLManager;
            _uriService = uriService;
        }
        public async Task<PagedResponse<List<CompanyDetailModel>>> GetFollowCompany(int uid, int follow, string lang , PaginationFilter pageFilter , string route)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            var message = string.Empty;
            var status = true;
            var status_login = true;
            var device_token_status = true;
            var startup_badge = new StartupBadgeViewModel();
            List<CompanyDetailModel> data = new List<CompanyDetailModel>();
            int totalRecords = 0;
            messagecode = 0;
            var validFilter = new PaginationFilter(pageFilter.page_number, pageFilter.page_size);
            PagedResponse<List<CompanyDetailModel>> pagedReponse = null;

            try
            {
                if (uid.ToString() == "0" || clear == "")
                {
                    if (uid.ToString() == "0")
                    {
                        status_login = false;
                    }
                    List<CompanyDetailModel> models = _sql.GetAllFollowCompany(follow, uid);
                    totalRecords = models.Count;
                    //data = models.Skip(pageFilter.page_number * pageFilter.page_size).Take(pageFilter.page_size).ToList();
                    data = models.Skip((pageFilter.page_number - 1) * pageFilter.page_size).Take(pageFilter.page_size).ToList();


                   
                    //follow_flag = follow;
                    startup_badge = await _msg.GetStartupBadge(uid);
                }
                else
                {
                    status = false;
                    status_login = false;
                    device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                    message = _msg.GetMessageLang(lang, messagecode, "", "");
                }
                //pagedReponse = PaginationHelper.CreatePagedReponse<CompanyDetailModel>(data, validFilter, totalRecords, _uriService, route, message, device_token_status, status, status_login, startup_badge);
            }
            catch (Exception ex)
            {
                status = false;
                messagecode = 302001; //Sorry,internal server error
                message = _msg.GetMessageLang(lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("GetFollowCompany -- {0}", message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetAllShop");
            }
            finally
            {
            }
            pagedReponse = PaginationHelper.CreatePagedReponse<CompanyDetailModel>(data, validFilter, totalRecords, _uriService, route, message, device_token_status, status, status_login, startup_badge);
            return pagedReponse;
        }

        public PagedResponse<List<FollowModel>> GetAllCampaignFollowCompanyPage(int uid, int sid, string lang, PaginationFilter pageFilter, string route)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            var message = string.Empty;
            var status = true;
            var status_login = true;
            var device_token_status = true;
            var startup_badge = new StartupBadgeViewModel();
            List<FollowModel> data = new List<FollowModel>();
            int totalRecords = 0;
            messagecode = 0;
            var validFilter = new PaginationFilter(pageFilter.page_number, pageFilter.page_size);
            PagedResponse<List<FollowModel>> pagedReponse = null;

            try
            {
                if (uid.ToString() == "0" || clear == "")
                {
                    if (uid.ToString() == "0")
                    {
                        status_login = false;
                    }
                    //check campaign image type 20170315
                    UserInfo ui = new UserInfo();
                    ui = _sql.GetUserInformation2(uid);
                    string deviceType = ui.device_type;
                    /////////////////////////////

                    data = _sql.GetAllCampaignCompanyPage(sid, uid, pageFilter.page_number, pageFilter.page_size);
                    _sql.InsertNewNotificationFollowDetail(uid, sid);
                }
                else
                {
                   status = false;
                   device_token_status = true;
                   messagecode = 302002; //Sorry,Your device token invalid.
                   message = _msg.GetMessageLang(lang, messagecode, "", "");
                }
            }
            catch (Exception ex)
            {
                device_token_status = true;
                status = false;
                messagecode = 302001; //Sorry,internal server error
                message = _msg.GetMessageLang(lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("GetAllCampaignFollowCompanyPage -- {0}", message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetAllCampaign");
            }
            finally
            {
            }
            pagedReponse = PaginationHelper.CreatePagedReponse<FollowModel>(data, validFilter, totalRecords, _uriService, route, message, device_token_status, status, status_login, startup_badge);
            return pagedReponse;
        }

        public PagedResponse<List<FollowModel>> GetAllCompanyFollowCampaignList(string uid, string sid, string lang, PaginationFilter pageFilter, string route)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            var message = string.Empty;
            var status = true;
            var status_login = true;
            var device_token_status = true;
            var startup_badge = new StartupBadgeViewModel();
            List<FollowModel> data = new List<FollowModel>();
            int totalRecords = 0;
            messagecode = 0;
            var validFilter = new PaginationFilter(pageFilter.page_number, pageFilter.page_size);
            PagedResponse<List<FollowModel>> pagedReponse = null;

            try
            {
                if (uid.ToString() == "0")
                {
                    status_login = false;
                }
                
                data = _sql.GetPerPageCompanyFollowCampaign(int.Parse(sid), int.Parse(uid), validFilter.page_number, validFilter.page_size);
                totalRecords = _sql.GetTotalCompanyFollowCampaign(int.Parse(sid), int.Parse(uid));
                _sql.InsertNewNotificationFollowDetail(int.Parse(uid), int.Parse(sid));
            }
            catch (Exception ex)
            {
                device_token_status = true;
                status = false;
                messagecode = 302001; //Sorry,internal server error
                message = _msg.GetMessageLang(lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("GetAllShopFollowCampaignList -- {0}", message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetAllShopFollowCampaignList");
            }
            finally
            {
            }

            pagedReponse = PaginationHelper.CreatePagedReponse<FollowModel>(data, validFilter, totalRecords, _uriService, route, message, device_token_status, status, status_login, startup_badge);
            return pagedReponse;
        }
        public BaseResponse<CompanyDetail> GetFollowCompanyDetail(int uid, int sid, string lang)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<CompanyDetail> obj = new BaseResponse<CompanyDetail>();
            obj.message = string.Empty;
            obj.status = true;
            obj.status_login = true;
            obj.device_token_status = true;
            obj.data = new CompanyDetail();
             messagecode = 0;

            try
            {
                if (uid.ToString() == "0" || clear == "")
                {
                    if (uid.ToString() == "0")
                    {
                        obj.status_login = false;
                    }
                    obj.data = _sql.GetCompanyDetail(sid, uid);
                }
                else
                {
                    obj.status = false;
                    obj.status_login = false;
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
                _logger.LogError(ex, string.Format("GetCompanyDetail -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetShopDetail");
            }
            finally
            {
                _sql.InsertEventLog(uid, 0, "Company detail", "", 8, "", "", 0, 0, "", "");
            }

            return obj;
        }

    }
   
}
