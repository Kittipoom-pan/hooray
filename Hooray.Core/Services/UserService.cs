using Hooray.Core.Interfaces;
using Hooray.Core.RequestModels;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.Helpers;
using Hooray.Infrastructure.Manager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Hooray.Core.ViewModels.CampaignJoinViewModel;

namespace Hooray.Core.Services
{
    public class UserService : IUserService
    {
        private int messagecode = 0;
        private string clear = "";
        private readonly IUserRepository _userRepository;
        private readonly IMySQLManager _mySQLManager;
        private readonly IMySQLManagerRepository _mySQLManagerRepository;
        private readonly ILogger _logger;
        private readonly IUriService _uriService;
        public UserService(IUserRepository userRepository, IMySQLManager mySQLManager, IMySQLManagerRepository mySQLManagerRepository, ILogger<UserService> logger, IUriService uriService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mySQLManager = mySQLManager;
            _mySQLManagerRepository = mySQLManagerRepository;
            _uriService = uriService;
        }
        public async Task<BaseResponse<HoorayFollow>> AddUserFollow(AddUserFollowRequest model)
        {
            BaseResponse<HoorayFollow> obj = new BaseResponse<HoorayFollow>();
            obj.message = string.Empty;
            obj.status = true;
            obj.status_login = true;
            obj.data = new HoorayFollow();
            obj.data.status_unfollow = false;
            obj.device_token_status = true;
            obj.data.company_follow = new CompanyFollow();
            obj.data.campaignjoin = new Join();
            messagecode = 0;

            try
            {
                if (clear == "")
                {
                    //if (model.unfollow == 0) 
                    if (model.unfollow == 1)
                    {
                        obj.data.company_follow = await _userRepository.InsertAndGetNewUserFollow(model.company_id, model.user_id);
                        if (obj.data.company_follow == null)
                        {
                            obj.data.company_follow = new CompanyFollow();
                            obj.status = false;
                        }
                        else
                        {
                            int notificationID = await _userRepository.InsertNewNotificationFollow(model.user_id, model.campaign_id);
                            messagecode = 301036; //Follow Success.
                        }

                        if (model.join == 1)
                        {
                            //CheckGroupJoin
                            bool isJoin = false;
                            bool is_group = await _mySQLManager.CheckGroup(model.campaign_id);

                            if (is_group)
                            {
                                bool is_win = await _mySQLManager.CheckWinGroup(model.user_id, model.campaign_id);
                                if (is_win)
                                {
                                    isJoin = false;
                                    obj.status = false;
                                    messagecode = 301053; //You 've won already this campaign.
                                }
                                else
                                {
                                    isJoin = true;
                                }
                            }
                            else
                            {
                                isJoin = true;
                            }

                            if (isJoin)
                            {
                                int statusJoin = await _mySQLManager.CheckFollowAndJoin(model.company_id, model.user_id, model.campaign_id);
                                if ((statusJoin == 1) || (statusJoin == 4))
                                {
                                    var checkCampaign = _mySQLManager.CheckCampaignBeforeJoin(model.campaign_id, model.user_id);

                                    if ((checkCampaign.Result.required_facebook) && (!checkCampaign.Result.status_like_fanpage))
                                    {
                                        obj.status = false;
                                        messagecode = 301049; //กรุณา Like Fanpage ของกิจกรรมนี้ก่อนเข้าร่วม
                                    }
                                    else if ((checkCampaign.Result.required_share_feed) && (!checkCampaign.Result.status_share_feed))
                                    {
                                        obj.status = false;
                                        messagecode = 301055; //กรุณา share campaign นี้ก่อนเข้าร่วม 
                                    }
                                    else
                                    {
                                        if (checkCampaign.Result.join_expire == 0)
                                        {
                                            obj.status = false;
                                            messagecode = 301052; //Campaign นี้ หมดเวลาร่วมสนุกแล้ว
                                            obj.message = _mySQLManager.GetMessageLang(model.lang, messagecode, "", "");
                                        }
                                        else if (checkCampaign.Result.valid == 0)
                                        {
                                            obj.status = false;
                                            messagecode = 301051; //Campaign นี้ แจกครบหมดแล้ว
                                        }
                                        else
                                        {
                                            int resultDigit = await _mySQLManager.GetCampaignDigit(model.campaign_id);
                                            double min;
                                            int mins;
                                            int maxs;
                                            double join_number;
                                            if (resultDigit == 1)
                                            {
                                                mins = 1;
                                                maxs = 9;
                                            }
                                            else
                                            {
                                                //min = Math.Pow(10, resultDigit - 1);
                                                min = Math.Pow(10, 2 - 1);
                                                mins = Convert.ToInt32(min);
                                                maxs = (mins * 10) - 1;
                                            }
                                            Random rand = new Random();
                                            join_number = rand.Next(mins, maxs);

                                            obj.data.campaignjoin = await _mySQLManager.InsertNewUserJoin(model.campaign_id, model.user_id, join_number, model.lat, model.lng, "0");
                                            if (obj.data.campaignjoin == null)
                                            {
                                                obj.data.campaignjoin = new Join();
                                                obj.status = false;
                                            }
                                            else
                                            {
                                                messagecode = 301037; //Join And Follow Success.
                                            }
                                        }
                                    }

                                }
                                else if (statusJoin == 2) //ประกาศผลไปแล้ว
                                {
                                    obj.status = false;
                                    messagecode = 301045; //This campaign has already announced the result.
                                }
                            }
                        }
                    }
                    else
                    {
                        obj.data.status_unfollow = await _userRepository.UpdateUnFollow(model.company_id, model.user_id);
                        if (obj.data.status_unfollow)
                        {
                            messagecode = 301038; //Unfollow Success.
                        }
                    }
                }
                else
                {
                    obj.status = false;
                    obj.status_login = false;
                    obj.device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                }

                obj.message = _mySQLManager.GetMessageLang(model.lang, messagecode, "", "");
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _mySQLManager.GetMessageLang(model.lang, messagecode, "", "");

                _logger.LogError(ex, string.Format("AddUserFollow -- {0}", obj.message));

                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "AddUserFollow");
            }

            return obj;
        }

        public async Task<PagedResponse<List<UserResult>>> GetAllUserResult(int uid, int join, int announcetype, string lang, PaginationFilter pageFilter , string url)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
            var message = string.Empty;
            var status = true;
            var status_login = true;
            var device_token_status = true;
            var startup_badge = new StartupBadgeViewModel();
            List<UserResult> data = new List<UserResult>();
            int totalRecords = 0;
            messagecode = 0;
            var validFilter = new PaginationFilter(pageFilter.page_number, pageFilter.page_size);
            PagedResponse<List<UserResult>> pagedReponse = null;

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
                    ui = _mySQLManagerRepository.GetUserInformation2(uid);
                    string deviceType = ui.device_type;
                    /////////////////////////////

                  var  models = _mySQLManagerRepository.GetAllResult(uid, join, announcetype, deviceType);
                    startup_badge = await _mySQLManager.GetStartupBadge(uid);
                    startup_badge.gift_badge_count = _mySQLManagerRepository.CountPrizeBadge(uid);
                    totalRecords = models.Count;
                    data = models.Skip((pageFilter.page_number - 1) * pageFilter.page_size).Take(pageFilter.page_size).ToList();
                }
                else
                {
                    status = false;
                    device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                    message = _mySQLManager.GetMessageLang(lang, messagecode, "", "");
                }
                pagedReponse = PaginationHelper.CreatePagedReponse<UserResult>(data, validFilter, totalRecords, _uriService, url, message, device_token_status, status, status_login, startup_badge);
            }
            catch (Exception ex)
            {
                status = false;
                messagecode = 302001; //Sorry,internal server error
                message = _mySQLManager.GetMessageLang(lang, messagecode, "", "");

                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetAllUserResult");
            }
            finally
            {
            }

            return pagedReponse;
        }
    }
}
