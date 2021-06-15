using Hooray.Core.Interfaces;
using Hooray.Core.Manager;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Hooray.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MySqlConnection _con;
        private readonly devhoorayContext _context;

        public UserRepository(devhoorayContext context)
        {
            _context = context;
            _con = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
        }
        public async Task<CompanyFollow> InsertAndGetNewUserFollow(int pCompanyID, int pUserID)
        {
            CompanyFollow companyFollowList = null;
            DataTable table = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_insert_and_get_user_follow";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCompanyID", pCompanyID);
                    cmd.Parameters.AddWithValue("@pUserID", pUserID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    if (table != null && table.Rows.Count > 0)
                    {
                        companyFollowList = new CompanyFollow();
                        CompanyFollow companyFollow;
                        foreach (DataRow row in table.Rows)
                        {
                            companyFollow = new CompanyFollow();
                            companyFollow.loadDataShopFollow(row);

                            #region imgShoplogo
                            //string company_logo_img_path = "/company_logo/" + companyFollow.company_image_name + ImageType.image_logo;
                            //string company_logo_img_url = string.Format(WebConfigurationManager.AppSettings["shop_logo_path"], WebConfigurationManager.AppSettings["resource_ip"], companyFollow.company_image_name + ImageType.image_logo);
                            //companyFollow.shop_logo_image = GetImagePhoto(company_logo_img_path, shop_logo_img_url);
                            #endregion

                            //companyFollowList.Add(companyFollow);
                        }
                    }
                }

            }
            catch (System.IO.IOException e)
            {
                throw e;
            }

            finally
            {
                _con.Close();
            }

            return companyFollowList;
        }

        public async Task<int> InsertNewNotificationFollow(int pUserID, string pCampaignID)
        {
            DataTable table = new DataTable();
            int notificationID = 0;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_insert_new_notification_follow";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                    cmd.Parameters.AddWithValue("@pUserID", pUserID);

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
                        notificationID = int.Parse(dr["new_notification_id"].ToString());
                    }

                    return notificationID;
                }
            }
            catch (System.IO.IOException e)
            {
                throw e;
            }

            finally
            {
                _con.Close();
            }
        }

        public async Task<bool> UpdateUnFollow(int pCompanyID, int pUserID)
        {
            bool success = false;

            var result = await _context.HryUserFollow.Where(c => c.CompanyId == pCompanyID && c.UserId == pUserID.ToString()).FirstOrDefaultAsync();
            if (result != null)
            {
                result.UnFollowDate = DateTime.Now;

                _context.HryUserFollow.Update(result);
                _context.SaveChanges();
            }

            var campaignUserFollowId = await _context.HryUserFollow.Where(c => c.CompanyId == pCompanyID && c.UserId == pUserID.ToString() && c.UnFollowDate == DateTime.Now).Select(i => i.CampaignUserFollowId).FirstOrDefaultAsync();
            if (campaignUserFollowId != null)
            {
                success = true;
            }

            return success;
        }
    }
}
