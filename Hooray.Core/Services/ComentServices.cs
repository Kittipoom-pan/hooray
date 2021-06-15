using Hooray.Core.AppsettingModels;
using Hooray.Core.Interfaces;
using Hooray.Core.Manager;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.Helpers;
using Hooray.Infrastructure.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hooray.Core.Services
{
    public class ComentServices : IComentServices
    {
        private readonly IOptions<Resource> _appSettings;
        private readonly IMySQLManager _msg;
        private IMySQLManagerRepository _sql;
        private int messagecode = 0;
        private readonly IUriService _uriService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string clear = "";
        private readonly ILogger _logger;
        public ComentServices( IOptions<Resource> appSettings, IMySQLManager mySQL, IMySQLManagerRepository mySQLManagerRepository,
            IUriService uriService, IHttpContextAccessor httpContextAccessor , ILogger<ComentServices> logger)
        {
            _logger = logger;
            _sql = mySQLManagerRepository;
            _appSettings = appSettings;
            _msg = mySQL;
            _uriService = uriService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<PagedResponse<List<CommentModel>>> GetAllFeedCommentList(int uid, string cpid, PaginationFilter pageFilte, string lang, string url)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
            var message = string.Empty;
            var status = true;
            var status_login = true;
            var device_token_status = true;
            var startup_badge = new StartupBadgeViewModel();
            List<CommentModel> models = new List<CommentModel>();
            int totalRecords = 0;
            int campaign_like_count = 0;
            int campaign_comment_count = 0;
            messagecode = 0;
            var validFilter = new PaginationFilter(pageFilte.page_number, pageFilte.page_size);
            PagedResponse<List<CommentModel>> pagedReponse = null;
            try
            {
                if (uid.ToString() == "0")
                {
                    status_login = false;
                }
                else
                {


                    messagecode = 0;
                    lang = (lang == null) ? _appSettings.Value.DefaultLanguage : lang;

                    DataTable dt = _sql.GetCount(cpid);
                    DataRow dr = dt.Rows[0];
                     campaign_like_count = dr["stat_like_count"] == DBNull.Value ? 0 : Convert.ToInt32(dr["stat_like_count"]);
                     campaign_comment_count = dr["comment_count"] == DBNull.Value ? 0 : Convert.ToInt32(dr["comment_count"]);

                    int perPage = (pageFilte.page_size == 0) ? int.Parse(_appSettings.Value.PerPageShop) : pageFilte.page_size;
                    int pageInt = (pageFilte.page_number == 0) ? 1 : pageFilte.page_number;

                    var campaigncomment = _sql.GetPerPageCampaignComment(cpid, pageInt, perPage);

                    foreach (DataRow item in campaigncomment.Rows)
                    {
                        CommentModel model = new CommentModel();
                        model.comment_time = Utility.convertToDateTimeServiceFormatString(item["comment_time"].ToString());

                        model.comment_id = item["comment_id"] == DBNull.Value ? 0 : Convert.ToInt32(item["comment_id"]);
                        model.user_id = item["user_id"] == DBNull.Value ? 0 : Convert.ToInt32(item["user_id"]);
                        model.campaign_id = item["campaign_id"].ToString();
                        model.display_name = item["display_show_name"].ToString();
                        model.is_image = item["is_image"] == DBNull.Value ? false : Convert.ToBoolean(item["is_image"]);
                        model.image_name_profile = item["image_name_profile"].ToString();

                        #region imgUserProfileRound
                        string user_r_img_path = "/user_profile_image/" + model.image_name_profile + ImageType.image_round;
                        string user_r_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                        _appSettings.Value.Photo.UserPhotoPath, model.image_name_profile + ImageType.image_round);
                        model.user_image_round = _sql.GetImagePhoto(user_r_img_path, user_r_img_url);
                        #endregion

                        #region imgUserProfileFull
                        string user_f_img_path = "/user_profile_image/" + model.image_name_profile + ImageType.imagejpg;
                        string user_f_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                  _appSettings.Value.Photo.UserPhotoPath, model.image_name_profile + ImageType.imagejpg);
                        model.user_image_full = _sql.GetImagePhoto(user_f_img_path, user_f_img_url);
                        #endregion

                        if (model.is_image == true)
                        {
                            model.comment_text = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                 _appSettings.Value.CommentPhotoPath, item["comment_image"].ToString());
                        }
                        else
                        {
                            model.comment_text = System.Web.HttpUtility.UrlDecode(item["comment_text"].ToString());
                        }
                        models.Add(model);
                    }


                    totalRecords = _sql.GetTotalCampaignComment(cpid);
                    startup_badge = await _msg.GetStartupBadge(uid);

                }
                pagedReponse = PaginationHelper.CreatePagedReponse<CommentModel>(models, validFilter, totalRecords, _uriService, url, message, device_token_status, status, status_login, startup_badge , campaign_like_count, campaign_comment_count);
            }
            catch (Exception ex)
            {
                 status = false;
                messagecode = 302001; //Sorry,internal server error
                message = _msg.GetMessageLang(lang, messagecode, "", "");
                pagedReponse = PaginationHelper.CreatePagedReponse<CommentModel>(models, validFilter, totalRecords, _uriService, url, message, device_token_status, status, status_login, startup_badge);
                _logger.LogError(ex, string.Format("GetAllFeedCommentList -- {0}", message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetAllFeedCommentList");
            }
            finally
            {
                _sql.InsertEventLog(uid, 0, "feed detail", "", 7, "", "", 0, 0, "", "");
            }

            return pagedReponse;
        }


        public BaseResponse<CommentModel> AddNewComment(int uid, string cpid, string comment, string tokenID, string lang)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<CommentModel> obj = new BaseResponse<CommentModel>();
            obj.message = string.Empty;
            obj.status = true;
            obj.status_login = true;
            obj.device_token_status = true;
            obj.data = new CommentModel();
            messagecode = 0;

            try
            {
                if (clear == "")
                {
                    string decodeComment = System.Web.HttpUtility.UrlDecode(comment);
                    var tablecomment = _sql.InsertNewComment(cpid, uid, decodeComment);
                    //List<CommentModel> models = new List<CommentModel>();
                    //foreach (DataRow item in tablecomment.Rows)
                    //{
                    DataRow item = tablecomment.Rows[0];
                    CommentModel model = new CommentModel();
                    model.comment_time = Utility.convertToDateTimeServiceFormatString(item["comment_time"].ToString());

                    model.comment_id = item["comment_id"] == DBNull.Value ? 0 : Convert.ToInt32(item["comment_id"]);
                    model.user_id = item["user_id"] == DBNull.Value ? 0 : Convert.ToInt32(item["user_id"]);
                    model.campaign_id = item["campaign_id"].ToString();
                    model.display_name = item["display_show_name"].ToString();
                    model.is_image = item["is_image"] == DBNull.Value ? false : Convert.ToBoolean(item["is_image"]);
                    model.image_name_profile = item["image_name_profile"].ToString();

                    #region imgUserProfileRound
                    string user_r_img_path = "/user_profile_image/" + model.image_name_profile + ImageType.image_round;
                    string user_r_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                          _appSettings.Value.Photo.UserPhotoPath, model.image_name_profile + ImageType.image_round);
                    model.user_image_round = _sql.GetImagePhoto(user_r_img_path, user_r_img_url);
                    #endregion

                    #region imgUserProfileFull
                    string user_f_img_path = "/user_profile_image/" + model.image_name_profile + ImageType.imagejpg;
                    string user_f_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                             _appSettings.Value.Photo.UserPhotoPath, model.image_name_profile + ImageType.imagejpg);
                    model.user_image_full = _sql.GetImagePhoto(user_f_img_path, user_f_img_url);
                    #endregion

                    if (model.is_image == true)
                    {

                        model.comment_text = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                _appSettings.Value.CommentPhotoPath, item["comment_image"].ToString());
                    }
                    else
                    {
                        model.comment_text = System.Web.HttpUtility.UrlDecode(item["comment_text"].ToString());

                        //CommentModel model = new CommentModel();
                        //model.comment_time = Utility.convertToDateTimeServiceFormatString(item["comment_time"].ToString());

                        //model.comment_id = item["comment_id"] == DBNull.Value ? 0 : Convert.ToInt32(item["comment_id"]);
                        //model.user_id = item["user_id"] == DBNull.Value ? 0 : Convert.ToInt32(item["user_id"]);
                        //model.campaign_id = item["campaign_id"].ToString();
                        //model.display_name = item["display_show_name"].ToString();
                        //model.is_image = item["is_image"] == DBNull.Value ? false : Convert.ToBoolean(item["is_image"]);
                        //model.image_name_profile = item["image_name_profile"].ToString();

                        //#region imgUserProfileRound
                        //string user_r_img_path = "/user_profile_image/" + model.image_name_profile + ImageType.image_round;
                        //string user_r_img_url = string.Format(_appSettings.Value.Photo.UserPhotoPath, _appSettings.Value.BaseUrl, model.image_name_profile + ImageType.image_round);
                        //model.user_image_round = _sql.GetImagePhoto(user_r_img_path, user_r_img_url);
                        //#endregion

                        //#region imgUserProfileFull
                        //string user_f_img_path = "/user_profile_image/" + model.image_name_profile + ImageType.imagejpg;
                        //string user_f_img_url = string.Format(_appSettings.Value.Photo.UserPhotoPath, _appSettings.Value.BaseUrl, model.image_name_profile + ImageType.imagejpg);
                        //model.user_image_full = _sql.GetImagePhoto(user_f_img_path, user_f_img_url);
                        //#endregion

                        //if (model.is_image == true)
                        //{
                        //    model.comment_text = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_appSettings.Value.BaseUrl}/" +
                        //                              $"{_appSettings.Value.CommentPhotoPath}{item["comment_image"].ToString()}";
                        //}
                        //else
                        //{
                        //    model.comment_text = System.Web.HttpUtility.UrlDecode(item["comment_text"].ToString());
                        //}
           

                    }
                    
                    //}
                    obj.data = model;
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
                _logger.LogError(ex, string.Format("AddNewComment -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "AddNewComment");
            }
            finally
            {
            }

            return obj;
        }
    }
}
