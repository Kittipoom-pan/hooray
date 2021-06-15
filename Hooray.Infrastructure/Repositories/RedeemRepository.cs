using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hooray.Infrastructure.Repositories
{
    public class RedeemRepository : IRedeemRepository
    {
        private readonly devhoorayContext _context;
        private readonly MySqlConnection _con;
        public RedeemRepository(devhoorayContext context)
        {
            _context = context;
            _con = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
        }

        public async Task<int> GetDeal(string campaign_id)
        {
            //int deal_id = 0;

            var result =  (from c in _context.HryCampaign
                                join cd in _context.HryCampaignDeal
                                on c.CampaignId equals cd.CampaignId
                                where cd.CampaignId == campaign_id
                                select new
                                {
                                    deal_id = cd.DealId
                                }).FirstOrDefault();


            //DataTable table = new DataTable();
            //try
            //{
            //    using (MySqlCommand cmd = new MySqlCommand())
            //    {
            //        cmd.Connection = _con;
            //        _con.Open();
            //        cmd.CommandText = "get_deal";
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.AddWithValue("@pCampaignID", campaign_id);

            //        using (MySqlDataReader reader = cmd.ExecuteReader())
            //        {
            //            if (reader.HasRows)
            //            {
            //                table.Load(reader);
            //            }
            //        }

            //        if (table != null && table.Rows.Count > 0)
            //        {
            //            DataRow dr = table.Rows[0];
            //            deal_id = int.Parse(dr["deal_id"].ToString());
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MySQLManager");
            //}
            //finally
            //{
            //    if (_con != null)
            //    {
            //        _con.Dispose();
            //    }
            //}
            return result.deal_id;
        }

        public async Task<HryUserProfile> GetUser(string lineUserId, string displayName)
        {
            try
            {
                DealCodeRedeem dealCode = new DealCodeRedeem();

                var data = _context.HryUserProfile.Where(t => t.LineUserId == lineUserId.ToString()).FirstOrDefault();

                if (data != null)
                {
                    return data;
                }
                else
                {
                    var user = new HryUserProfile()
                    {
                        LineUserId = lineUserId,
                        DisplayFname = displayName,
                        CreateDate = DateTime.Now,
                        RegisterType = "Line"
                    };
              
                    _context.HryUserProfile.Add(user);
                    _context.SaveChanges();

                    return user;
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DealCodeRedeem> GetUserByLineId(string campaignId ,string lineUserId, string displayName)
        {
            try
            {
                DealCodeRedeem dealCode = new DealCodeRedeem();

                var data = _context.HryUserProfile.Where(t => t.LineUserId == lineUserId.ToString()).FirstOrDefault();

                if (data != null)
                {
                    var dealId = await GetDeal(campaignId);

                    if (data.DisplayFname == "" || data.DisplayFname == null)
                    {
                        data.DisplayFname = "No first name";
                    }

                    dealCode.loadData(dealId, data.DisplayFname, data.UserId);

                    return dealCode;
                }
                else
                {
                    var user = new HryUserProfile()
                    {
                        LineUserId = lineUserId,
                        DisplayFname = displayName,
                        CreateDate = DateTime.Now,
                        RegisterType = "Line"
                    };

                    _context.HryUserProfile.Add(user);
                    _context.SaveChanges();

                    var dealId = await GetDeal(campaignId);

                    if (user.DisplayFname == "" || user.DisplayFname == null)
                    {
                        user.DisplayFname = "No first name";
                    }

                    dealCode.loadData(dealId, user.DisplayFname, user.UserId);

                    return dealCode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
