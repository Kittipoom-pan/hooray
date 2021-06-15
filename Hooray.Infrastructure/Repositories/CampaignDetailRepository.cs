using Hooray.Core.AppsettingModels;
using Hooray.Core.Interfaces;
using Hooray.Core.Manager;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using Hooray.Infrastructure.Manager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Hooray.Infrastructure.Repositories
{
    public class CampaignDetailRepository : ICampaignDetailRepository
    {
        private readonly MySqlConnection _con;
        private readonly devhoorayContext _context;
        private readonly IOptions<Resource> _appSettings;
        private readonly IMySQLManagerRepository _mySQLManagerRepository;

        public CampaignDetailRepository(devhoorayContext context, IOptions<Resource> appSettings,
            IMySQLManagerRepository mySQLManagerRepository)
        {
            _context = context;
            _appSettings = appSettings;
            _con = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
            _mySQLManagerRepository = mySQLManagerRepository;
        }

        //public async Task<string> AddUserLineId(int user_id)
        //{
        //    var line_id = _context.HryUserProfile.SingleOrDefault(e => e.LineId == user_id);
        //    return "";
        //}

        public async Task<UserJoin> CheckJoinUserCampaign(string campaign_id, string user_id)
        {
            try
            {
                bool success = false;

                DataTable table = new DataTable();
                UserJoin userJoin = new UserJoin();

                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_check_join"; // The name of the Stored Proc
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@userID", user_id);
                    cmd.Parameters.AddWithValue("@campaignID", campaign_id);

                    //table = executeProcedureWithReturnTable();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    //using (var reader = cmd.ExecuteReader())
                    //{
                    //    while (reader.Read())
                    //    {
                    //        var HotelId = reader["hotel_id"] != DBNull.Value ? Convert.ToInt32(reader["hotel_id"]) : 0;
                    //    }
                    //}
                    if (table != null && table.Rows.Count > 0)
                    {
                        _con.Close();
                        var pathImage = await GetUserCampaignResult(campaign_id, user_id);
                        userJoin.loadData(true, pathImage);
                        return userJoin;
                        //return (true, pathImage);
                    }
                    else
                    {
                        return userJoin;
                        //return (false, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetUserCampaignResult(string campaign_id, string user_id)
        {
            try
            {
                List<CampaignResultViewModel> campaignResultsList = new List<CampaignResultViewModel>();

                DataTable table = new DataTable();

                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //if(_con.Open)
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_get_user_result";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@userID", user_id);
                    cmd.Parameters.AddWithValue("@campaignID", campaign_id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    if (table != null && table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            CampaignResultViewModel campaignResults = new CampaignResultViewModel();

                            campaignResults.loadDataImageCampaignResult(row);
                            campaignResultsList.Add(campaignResults);
                        }
                        var imageResult = await GetImageCampaignResult(campaignResultsList);
                        _con.Close();

                        return imageResult;
                    }

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<HryUserProfile> GetUserInformation(int user_id) => await _context.HryUserProfile.Where(t => t.UserId == user_id).FirstOrDefaultAsync();

        public async Task<(UserInfo, string)> GetUserInformation(int user_id)
        {
            UserInfo userInfo = new UserInfo();

            var data = await _context.HryUserProfile.Where(t => t.UserId == user_id).FirstOrDefaultAsync();
            if (userInfo != null)
            {
                #region imgUserProfile
                string user_img_path = "/user_profile_image/" + userInfo.image_name_profile + ImageType.image_thumbnail;
                string user_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                    _appSettings.Value.Photo.UserPhotoPath, userInfo.image_name_profile + ImageType.image_thumbnail);
                userInfo.user_image = _mySQLManagerRepository.GetImagePhoto(user_img_path, user_img_url);
                #endregion

                #region imgUserProfileFull
                string user_f_img_path = "/user_profile_image/" + userInfo.image_name_profile + ImageType.imagejpg;
                string user_f_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                        _appSettings.Value.Photo.UserPhotoPath, userInfo.image_name_profile + ImageType.imagejpg);
                userInfo.user_image_full = _mySQLManagerRepository.GetImagePhoto(user_f_img_path, user_f_img_url);
                #endregion
            }
            return (userInfo, data.DeviceType);
        }

        public List<Campaign> GetAllCampaign(int user_id, string device_type)
        {
            List<Campaign> campaignList = new List<Campaign>();

            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _con;
                _con.Open();
                cmd.CommandText = "hry_get_all_campaign";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", user_id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }

                if (table != null && table.Rows.Count > 0)
                {
                    Campaign campaign;
                    foreach (DataRow row in table.Rows)
                    {
                        campaign = new Campaign();
                        campaign.loadDataCampaign(row);

                        #region imgShoplogo
                        string comapny_logo_img_path = "/company_logo/" + campaign.company_image_name + ImageType.image_logo;
                        string company_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                       _appSettings.Value.CompanyLogoPath, campaign.company_image_name + ImageType.image_logo);

                        campaign.company_logo_image = _mySQLManagerRepository.GetImagePhoto(comapny_logo_img_path, company_logo_img_url);
                        #endregion

                        campaignList.Add(campaign);
                    }
                }
            }

            return campaignList;
        }

        private async Task<string> GetImageCampaignResult(List<CampaignResultViewModel> campaignResults)
        {
            Random random = new Random();
            List<CampaignResultViewModel>[] imagesResult = new List<CampaignResultViewModel>[] { campaignResults };
            var iamges = imagesResult[random.Next(0, imagesResult.Length)];
            int index = random.Next(iamges.Count);
            var campaignImageResult = iamges[index];

            return campaignImageResult.image_url;
        }

        public List<Campaign> GetPerPageCampaign(int user_id, int page, int per_page)
        {
            List<Campaign> campaignList = null;

            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _con;
                _con.Open();
                cmd.CommandText = "hry_perpage_campaign";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pUserID", user_id);
                cmd.Parameters.AddWithValue("@pPage", page);
                cmd.Parameters.AddWithValue("@pPerPage", per_page);

                if (table != null && table.Rows.Count > 0)
                {
                    Campaign campaign;
                    foreach (DataRow row in table.Rows)
                    {
                        campaign = new Campaign();
                        campaign.loadDataCampaign(row);

                        #region imgShoplogo
                        string comapny_logo_img_path = "/company_logo/" + campaign.company_image_name + ImageType.image_logo;
                        string company_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                       _appSettings.Value.CompanyLogoPath, campaign.company_image_name + ImageType.image_logo);

                        campaign.company_logo_image = _mySQLManagerRepository.GetImagePhoto(comapny_logo_img_path, company_logo_img_url);
                        #endregion

                        campaignList.Add(campaign);
                    }
                }
            }

            return campaignList;
        }

        public List<Campaign> GetAllFeedCampaign2(int user_id, int page, int per_page, string device_type)
        {
            List<Campaign> campaignList = new List<Campaign>();

            DataTable table = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_get_all_campaign2";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pUserID", user_id);
                    cmd.Parameters.AddWithValue("@pPage", page);
                    cmd.Parameters.AddWithValue("@pPerPage", per_page);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    if (table != null && table.Rows.Count > 0)
                    {
                        Campaign campaign;
                        foreach (DataRow row in table.Rows)
                        {
                            campaign = new Campaign();
                            campaign.loadDataCampaign(row);

                            #region imgCompanylogo
                            string comapny_logo_img_path = "/company_logo/" + campaign.company_image_name + ImageType.image_logo;
                            string company_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                       _appSettings.Value.CompanyLogoPath, campaign.company_image_name + ImageType.image_logo);

                            campaign.company_logo_image = _mySQLManagerRepository.GetImagePhoto(comapny_logo_img_path, company_logo_img_url);
                            
                            #endregion

                            #region imgCampaignGallery
                            campaign.campaign_gallery_image = new List<ImagePhoto>();
                            ImagePhoto imgPhoto;
                            for (int i = 1; i <= campaign.campaign_gallery_count; i++)
                            {
                                imgPhoto = new ImagePhoto();
                                string cpn_gal_img_path = "/campaign_gallery/" + campaign.campaign_id + "_" + i + ImageType.image_first;
                                //string cpn_gal_img_url = string.Format(_httpContextAccessor.HttpContext.Request.Host.Value,
                                //    campaign.campaign_id + "_" + i + "_" + campaign.campaign_gallery_no + ImageType.image_first);
                                string cpn_gal_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                       _appSettings.Value.CompanyLogoPath, campaign.campaign_id + "_" + i + "_" + 
                                                       campaign.campaign_gallery_no + ImageType.image_first);
                                imgPhoto = _mySQLManagerRepository.GetImagePhoto(cpn_gal_img_path, cpn_gal_img_url);
                                campaign.campaign_gallery_image.Add(imgPhoto);
                            }
                            #endregion

                            if (campaign.campaign_image_type == 2)
                            {
                                if (campaign.video_url != "")
                                {
                                    #region imgCampaignVideo
                                    string cpn_video_path = campaign.video_url;
                                    string cpn_video_url = string.Format(_appSettings.Value.YoutubeVideoURL, campaign.video_url);
                                    campaign.campaign_photo_image = _mySQLManagerRepository.GetImagePhoto(cpn_video_path, cpn_video_url);
                                    #endregion
                                }
                                else
                                {
                                    #region imgCampaignPhoto
                                    string cpn_video_path = "/campaign_video/" + campaign.campaign_id + ImageType.video_type;

                                    string cpn_video_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                            _appSettings.Value.CampaignVideoPath, campaign.campaign_id + ImageType.video_type);
                                    campaign.campaign_photo_image = _mySQLManagerRepository.GetImagePhoto(cpn_video_path, cpn_video_url);
                                    #endregion
                                }
                            }
                            else if (campaign.campaign_image_type == 3)
                            {
                                #region imgCampaignPhoto
                                string cpn_photo_img_path = "/campaign_image/" + campaign.campaign_image_name + ImageType.image_gif;
                                string cpn_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                            _appSettings.Value.CampaignPhotoPath, campaign.campaign_image_name + ImageType.image_gif);
                                campaign.campaign_photo_image = _mySQLManagerRepository.GetImagePhoto(cpn_photo_img_path, cpn_photo_img_url);
                                #endregion
                           
                            }
                            
                            else
                            {
                                #region imgCampaignPhoto
                                string cpn_photo_img_path = "/campaign_image/" + campaign.campaign_image_name + ImageType.imagejpg;
                                string cpn_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                            _appSettings.Value.CampaignPhotoPath, campaign.campaign_image_name + ImageType.imagejpg);
                                campaign.campaign_photo_image = _mySQLManagerRepository.GetImagePhoto(cpn_photo_img_path, cpn_photo_img_url);
                                #endregion
                            }
                            //#endregion

                            campaign.question = new List<Question>();
                            if (campaign.is_question)
                            {
                                campaign.question = _mySQLManagerRepository.GetQuestion(campaign.campaign_id);
                            }

                            campaignList.Add(campaign);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MySQLManager");
            }
            finally
            {
                if (_con != null)
                {
                    _con.Dispose();
                }
            }

            return campaignList;
        }
        public List<Campaign> GetAllFeedCampaign3(int user_id,int company_id, int page, int per_page, string device_type)
        {
            List<Campaign> campaignList = new List<Campaign>();

            DataTable table = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_get_all_campaign_company";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pUserID", user_id);
                    cmd.Parameters.AddWithValue("@pCompany_id", company_id);
                    cmd.Parameters.AddWithValue("@pPage", page);
                    cmd.Parameters.AddWithValue("@pPerPage", per_page);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    if (table != null && table.Rows.Count > 0)
                    {
                        Campaign campaign;
                        foreach (DataRow row in table.Rows)
                        {
                            campaign = new Campaign();
                            campaign.loadDataCampaign(row);

                            #region imgCompanylogo
                            string comapny_logo_img_path = "/company_logo/" + campaign.company_image_name + ImageType.image_logo;
                            string company_logo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                       _appSettings.Value.CompanyLogoPath, campaign.company_image_name + ImageType.image_logo);

                            campaign.company_logo_image = _mySQLManagerRepository.GetImagePhoto(comapny_logo_img_path, company_logo_img_url);

                            #endregion

                            #region imgCampaignGallery
                            campaign.campaign_gallery_image = new List<ImagePhoto>();
                            ImagePhoto imgPhoto;
                            for (int i = 1; i <= campaign.campaign_gallery_count; i++)
                            {
                                imgPhoto = new ImagePhoto();
                                string cpn_gal_img_path = "/campaign_gallery/" + campaign.campaign_id + "_" + i + ImageType.image_first;
                                //string cpn_gal_img_url = string.Format(_httpContextAccessor.HttpContext.Request.Host.Value,
                                //    campaign.campaign_id + "_" + i + "_" + campaign.campaign_gallery_no + ImageType.image_first);
                                string cpn_gal_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                       _appSettings.Value.CompanyLogoPath, campaign.campaign_id + "_" + i + "_" +
                                                       campaign.campaign_gallery_no + ImageType.image_first);
                                imgPhoto = _mySQLManagerRepository.GetImagePhoto(cpn_gal_img_path, cpn_gal_img_url);
                                campaign.campaign_gallery_image.Add(imgPhoto);
                            }
                            #endregion

                            if (campaign.campaign_image_type == 2)
                            {
                                if (campaign.video_url != "")
                                {
                                    #region imgCampaignVideo
                                    string cpn_video_path = campaign.video_url;
                                    string cpn_video_url = string.Format(_appSettings.Value.YoutubeVideoURL, campaign.video_url);
                                    campaign.campaign_photo_image = _mySQLManagerRepository.GetImagePhoto(cpn_video_path, cpn_video_url);
                                    #endregion
                                }
                                else
                                {
                                    #region imgCampaignPhoto
                                    string cpn_video_path = "/campaign_video/" + campaign.campaign_id + ImageType.video_type;

                                    string cpn_video_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                            _appSettings.Value.CampaignVideoPath, campaign.campaign_id + ImageType.video_type);
                                    campaign.campaign_photo_image = _mySQLManagerRepository.GetImagePhoto(cpn_video_path, cpn_video_url);
                                    #endregion
                                }
                            }
                            else if (campaign.campaign_image_type == 3)
                            {
                                #region imgCampaignPhoto
                                string cpn_photo_img_path = "/campaign_image/" + campaign.campaign_image_name + ImageType.image_gif;
                                string cpn_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                            _appSettings.Value.CampaignPhotoPath, campaign.campaign_image_name + ImageType.image_gif);
                                campaign.campaign_photo_image = _mySQLManagerRepository.GetImagePhoto(cpn_photo_img_path, cpn_photo_img_url);
                                #endregion

                            }

                            else
                            {
                                #region imgCampaignPhoto
                                string cpn_photo_img_path = "/campaign_image/" + campaign.campaign_image_name + ImageType.imagejpg;
                                string cpn_photo_img_url = string.Format("{0}/{1}/{2}", _appSettings.Value.BaseUrl,
                                                            _appSettings.Value.CampaignPhotoPath, campaign.campaign_image_name + ImageType.imagejpg);
                                campaign.campaign_photo_image = _mySQLManagerRepository.GetImagePhoto(cpn_photo_img_path, cpn_photo_img_url);
                                #endregion
                            }
                            //#endregion

                            campaign.question = new List<Question>();
                            if (campaign.is_question)
                            {
                                campaign.question = _mySQLManagerRepository.GetQuestion(campaign.campaign_id);
                            }

                            campaignList.Add(campaign);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MySQLManager");
            }
            finally
            {
                if (_con != null)
                {
                    _con.Dispose();
                }
            }

            return campaignList;
        }
        public async Task<int> GetTotalCampaign2(int user_id)
        {
            int total = 0;

            DataTable table = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "get_total_campaign2";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pUserID", user_id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    if (table != null && table.Rows.Count > 0)
                    {
                        DataRow dr = table.Rows[0];
                        total = int.Parse(dr["total"].ToString());
                    }
                }
            }
            catch (MySqlException ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MySQLManager");
            }
            finally
            {
                if (_con != null)
                {
                    _con.Dispose();
                }
            }
            return total;
        }
        public async Task<int> GetTotalCampaign3(int user_id , int company_id)
        {
            int total = 0;

            DataTable table = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "get_total_campaign_company";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pUserID", user_id);
                    cmd.Parameters.AddWithValue("@pCompany_id", company_id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    if (table != null && table.Rows.Count > 0)
                    {
                        DataRow dr = table.Rows[0];
                        total = int.Parse(dr["total"].ToString());
                    }
                }
            }
            catch (MySqlException ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MySQLManager");
            }
            finally
            {
                if (_con != null)
                {
                    _con.Dispose();
                }
            }
            return total;
        }
        public async Task<int> InsertCampaignLike(string campaign_id, string user_id, int like_type)
        {
            int statusLike = 0;

            DataTable table = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_check_and_insert_campaign_like";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pUserID", user_id);
                    cmd.Parameters.AddWithValue("@pCampaignID", campaign_id);
                    cmd.Parameters.AddWithValue("@pLikeType", like_type);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    if (table != null && table.Rows.Count > 0)
                    {
                        DataRow dr = table.Rows[0];
                        statusLike = int.Parse(dr["statusLike"].ToString());
                    }
                }
            }
            catch (MySqlException ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MySQLManager");
            }
            finally
            {
                if (_con != null)
                {
                    _con.Dispose();
                }
            }
            return statusLike;
        }

        public async Task<int> GetLikeCampaign(string campaign_id, int like_type)
        {
            int campaignLikeCount = 0;

            DataTable table = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_get_like_campaign";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCampaignID", campaign_id);
                    cmd.Parameters.AddWithValue("@pLikeType", like_type);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    if (table != null && table.Rows.Count > 0)
                    {
                        DataRow dr = table.Rows[0];
                        campaignLikeCount = int.Parse(dr["like_count"].ToString());
                    }
                }
            }
            catch (MySqlException ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MySQLManager");
            }
            finally
            {
                if (_con != null)
                {
                    _con.Dispose();
                }
            }
            return campaignLikeCount;
        }

        public async Task<bool> UpdateLikeCampaign(string campaign_id, int like_count, int like_type)
        {
            bool success = false;

            DataTable table = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_get_like_campaign";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCampaignID", campaign_id);
                    cmd.Parameters.AddWithValue("@pLikeType", like_type);
                    cmd.Parameters.AddWithValue("@pLikeCount", like_count);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    if (table != null && table.Rows.Count > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MySQLManager");
            }
            finally
            {
                if (_con != null)
                {
                    _con.Dispose();
                }
            }
            return success;
        }
    }
}
