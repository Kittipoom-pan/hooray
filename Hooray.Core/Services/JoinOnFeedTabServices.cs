using Hooray.Core.AppsettingModels;
using Hooray.Core.Interfaces;
using Hooray.Core.Manager;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using static Hooray.Core.ViewModels.CampaignJoinViewModel;

namespace Hooray.Core.Services
{
    public class JoinOnFeedTabServices : IJoinOnFeedTabServices
    {
        private const string COMMA = ",";
        private int messagecode = 0;
        private string clear = "";
        private IMySQLManagerRepository _sql;
        private IMySQLManager _msg;
        private readonly IOptions<Resource> _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        public JoinOnFeedTabServices(IMySQLManagerRepository mySQLManagerRepository, IMySQLManager mySQLManager, IOptions<Resource> appSettings, IHttpContextAccessor httpContextAccessor , ILogger<JoinOnFeedTabServices> logger)
        {
            _logger = logger;
            _sql = mySQLManagerRepository;
            _msg = mySQLManager;
            _appSettings = appSettings;
            _httpContextAccessor = httpContextAccessor;
        }
        public BaseResponse<Join> AddJoinCampaign(int uid, string cpid, int sid, string tokenID, float lat, float lng, string lang)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<Join> obj = new BaseResponse<Join>();
            obj.message = string.Empty;
            obj.status = true;
            obj.device_token_status = true;
            obj.status_login = true;
            obj.data = new Join();
            messagecode = 0;

            try
            {
                if (clear == "")
                {
                    //CheckGroupJoin
                    bool isJoin = false;
                    bool is_group = _sql.CheckGroup(cpid);

                    if (is_group)
                    {
                        bool is_win = _sql.CheckWinGroup(uid, cpid);
                        if (is_win)
                        {
                            isJoin = false;
                            obj.status = false;
                            messagecode = 301053; //This campaign has already announced the result.
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
                        int statusJoin = _sql.CheckFollowAndJoin(sid, uid, cpid);
                        if (statusJoin == 1)
                        {
                            DataTable tableCheckCampaign = _sql.CheckCampaignBeforeJoin(cpid, uid);
                            if (tableCheckCampaign.Rows.Count > 0)
                            {
                                DataRow dr = tableCheckCampaign.Rows[0];
                                bool required_facebook = Convert.ToBoolean((dr["required_facebook"] == DBNull.Value ? 0: dr["required_facebook"]));
                                bool required_share_feed = Convert.ToBoolean((dr["required_share_feed"] == DBNull.Value ? 0 : dr["required_share_feed"]));
                                //HR-18 Watch video before join campaign
                                bool required_watch_video = Convert.ToBoolean((dr["required_watch_video"] == DBNull.Value ? 0 : dr["required_watch_video"]));

                                bool status_like_fanpage = Convert.ToBoolean(int.Parse(dr["status_like_fanpage"].ToString()));
                                bool status_share_feed = Convert.ToBoolean(int.Parse(dr["status_share_feed"].ToString()));
                                int valid = int.Parse(dr["valid"].ToString());
                                int join_expire = int.Parse(dr["join_expire"].ToString());
                                //HR-18 Watch video before join campaign
                                bool status_watch_video = Convert.ToBoolean(int.Parse(dr["status_watch_video"].ToString()));

                                if ((required_facebook) && (!status_like_fanpage))
                                {
                                    obj.status = false;
                                    messagecode = 301049; //กรุณา Like Fanpage ของกิจกรรมนี้ก่อนเข้าร่วม
                                }
                                else if ((required_share_feed) && (!status_share_feed))
                                {
                                    obj.status = false;
                                    messagecode = 301055; //กรุณา share campaign นี้ก่อนเข้าร่วม
                                }
                                //HR-18 Watch video before join campaign
                                else if ((required_watch_video) && (!status_watch_video))
                                {
                                    obj.status = false;
                                    messagecode = 301057; //กรุณา ดู video นี้ก่อนเข้าร่วม
                                }
                                else
                                {
                                    if (join_expire == 0)
                                    {
                                        obj.status = false;
                                        messagecode = 301052; //Campaign นี้ หมดเวลาร่วมสนุกแล้ว
                                    }
                                    else if (valid == 0)
                                    {
                                        obj.status = false;
                                        messagecode = 301051; //จำนวนการ join เต็ม
                                    }
                                    else
                                    {
                                        int resultDigit = _sql.GetCampaignDigit(cpid);
                                        double min;
                                        int mins;
                                        int maxs;
                                        double join_number = 0;
                                        if (resultDigit == 1)
                                        {
                                            mins = 1;
                                            maxs = 9;
                                        }
                                        else
                                        {
                                            //check เรื่อง min ถ้า 6 หลัก ต้องเริ่มที่ 000001
                                            min = Math.Pow(10, resultDigit - 1);
                                            mins = Convert.ToInt32(min);
                                            maxs = (mins * 10) - 1;
                                        }

                                        //check loop ไม่ซ้ำ เพิ่ม while loop ต้องไม่ซ้ำ
                                        //Random rand = new Random();
                                        //join_number = rand.Next(mins, maxs);
                                        //สิ้นสุด loop และนำ join_number มาใช้

                                        bool join_number_repeated = true;
                                        if (mins <= 0){mins = 1;}
                                        if (maxs <= 0){maxs = 1;}
                                        while (join_number_repeated)
                                        {
                                            Random rand = new Random();
                                            join_number = rand.Next(mins, maxs);
                                            join_number_repeated = _sql.CheckJoinNumberRepeated(join_number.ToString(), cpid);
                                        }

                                        obj.data = _sql.InsertNewUserJoinNew(cpid, uid, join_number, lat, lng, "0");
                                        if (obj.data == null)
                                        {
                                            obj.data = new Join();
                                            obj.status = false;
                                        }
                                    }
                                }
                            }
                        }
                        else if (statusJoin == 2) //ประกาศผลไปแล้ว
                        {
                            obj.status = true;
                            messagecode = 301045; //This campaign has already announced the result.
                        }
                        else if (statusJoin == 4) //ยังไม่ได้ Follow
                        {
                            obj.status = true;
                            messagecode = 301046; //Sorry, Please Follow the shop First
                        }
                        else if (statusJoin == 3) //Join ไปแล้ว
                        {
                            obj.status = true;
                            messagecode = 301050; //You already to enjoy this Campaign.

                        }
                        else
                        {
                            obj.status = true;
                        }
                    }

                }
                else
                {
                    obj.status = false;
                    obj.device_token_status = true;
                    obj.status_login = false;
                    messagecode = 302002; //Sorry,Your device token invalid.
                }
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("AddJoinCampaign -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "AddJoinCampaign");
            }
            finally
            {
            }

            return obj;
        }


        public BaseResponse<CampaignResult> GetCampaignResult(int uid, string cpid, string token, string lang)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<CampaignResult> obj = new BaseResponse<CampaignResult>();
            obj.message = string.Empty;
            obj.status = false;
            obj.status_login = false;
            obj.device_token_status = true;
            obj.data = new CampaignResult();
            messagecode = 0;

            try
            {
                if (clear == "")
                {
                    obj.status_login = true;
                    obj.device_token_status = true;
                    obj.data = _sql.CheckResultAward(uid, cpid);

                    double join_number_rate = Utility.GetRandomNumberDouble(0, 1);
                    int img_number = 1;

                    if ((obj.data.random_result_on_check) && (obj.data.check_announce == 0))
                    {
                        //double accum_win_rate = 0;
                        join_number_rate = Utility.GetRandomNumberDouble(0.0000001, 1);

                        _sql.UpdateCPUserCodeNumber(obj.data.id, 1, join_number_rate);

                        if (join_number_rate < obj.data.win_rate)
                        {
                            bool isPrize = _sql.UpdateCurrentWin(cpid, obj.data.id, obj.data.prize_id);
                            if (isPrize)
                            {
                                obj.data.result_type = 1;
                            }
                            else
                            {
                                obj.data.result_type = 2;
                            }
                        }
                        else
                        {
                            obj.data.result_type = 2;
                        }
                    }

                    if (obj.data.result_type == 1)
                    {
                        obj.status = true;
                        messagecode = 301042; //You Win.
                        if ((obj.data.result_image == "") || (obj.data.check_announce == 0))
                        {
                            if (obj.data.prize_count > 1)
                            {
                                img_number = obj.data.prize_order;
                                //pending dev เพิ่มกรณี แต่ละรางวัลมีหลายรูป
                            }
                            else
                            {
                                DataTable tableImgResult = _sql.GetImageResult(cpid, obj.data.result_type); //get image result
                                foreach (DataRow row in tableImgResult.Rows)
                                {
                                    double img_rate = double.Parse(row["img_rate"].ToString());
                                    int img_order = int.Parse(row["img_order"].ToString());
                                    //rateimage
                                    if (join_number_rate <= img_rate)
                                    {
                                        img_number = img_order;
                                        break;
                                    }
                                }
                            }

                            string imageName = "win_" + img_number + ImageType.image_gif;
                            string campaign_result_img_url = "";
                            string campaign_result_img_url2 = "";
                            if (obj.data.device_type == "Android")
                            {
                                campaign_result_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.ScratchpadPhotoPath, cpid + "/" + "c_" + "win_" + img_number + ImageType.image_html);

                                campaign_result_img_url2 = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.ScratchpadPhotoPath, cpid + "/" + "win_" + img_number + ImageType.image_html);
                                obj.data.img_url = campaign_result_img_url;
                            }
                            else
                            {
                                campaign_result_img_url2 = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                        _appSettings.Value.ScratchpadPhotoPath, cpid + "win_" + img_number + ImageType.image_gif);
                                obj.data.img_url = campaign_result_img_url2;
                            }

                            _sql.UpdateResultImage(uid, cpid, campaign_result_img_url2, imageName);

                            //obj.campaign_result.coin_qty
                            int user_prize_id = _sql.InsertNewUserPrize(cpid, uid, obj.data.id, obj.data.prize_id);
                            obj.data.user_prize_id = user_prize_id;

                            obj.data.img_path = "images/" + cpid + "/" + "win_" + img_number + ImageType.image_gif;
                        }
                        else
                        {
                            obj.data.img_url = obj.data.result_image;
                            obj.data.img_path = "images/" + cpid + "/" + obj.data.result_image_name;
                        }

                    }
                    else if (obj.data.result_type == 2)
                    {
                        obj.status = true;
                        messagecode = 301043; //You Lost.\
                        if (obj.data.result_image == "")
                        {
                            DataTable tableImgResult = _sql.GetImageResult(cpid, obj.data.result_type); //get image result
                            foreach (DataRow row in tableImgResult.Rows)
                            {
                                double img_rate = double.Parse(row["img_rate"].ToString());
                                int img_order = int.Parse(row["img_order"].ToString());
                                //rateimage
                                if (join_number_rate <= img_rate)
                                {
                                    img_number = img_order;
                                    break;
                                }
                            }
                            string imageName = "sorry_" + img_number + ImageType.image_gif;
                            string campaign_result_img_url = "";
                            string campaign_result_img_url2 = "";
                            if (obj.data.device_type == "Android")
                            {
                                campaign_result_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                       _appSettings.Value.ScratchpadPhotoPath, cpid + " /" + "c_" + "sorry_" + img_number + ImageType.image_html);
                                campaign_result_img_url2 = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                       _appSettings.Value.ScratchpadPhotoPath, cpid + "/" + "sorry_" + img_number + ImageType.image_html);
                                obj.data.img_url = campaign_result_img_url;
                            }
                            else
                            {
                                campaign_result_img_url2 = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                       _appSettings.Value.ScratchpadPhotoPath, cpid + "sorry_" + img_number + ImageType.image_gif);
                                obj.data.img_url = campaign_result_img_url2;
                            }
                            _sql.UpdateResultImage(uid, cpid, campaign_result_img_url2, imageName);
                            obj.data.img_path = "images/" + cpid + "/" + "sorry_" + img_number + ImageType.image_gif;
                        }
                        else
                        {
                            obj.data.img_url = obj.data.result_image;
                            obj.data.img_path = "images/" + cpid + "/" + obj.data.result_image_name;
                        }

                    }
                    else if (obj.data.result_type == 3)
                    {
                        obj.status = true;
                        messagecode = 301044; //Not Available.
                    }
                    //else if (obj.campaign_result.result_type == 4)
                    //{
                    //    obj.status = true;
                    //    //ยังไม่ได้ random รางวัล
                    //}
                    else
                    {
                        obj.status = false;
                    }
                }
                else
                {
                    obj.status = false;
                    obj.status_login = false;
                    obj.device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                }
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("GetCampaignResult -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetCampaignResult");
            }
            finally
            {
            }

            return obj;
        }
    }
}
