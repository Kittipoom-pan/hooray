using Hooray.Core.RequestModels;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hooray.Configuration
{
    public class DealCodeRedeemConfig
    {
        private readonly devhoorayContext _context;
        private readonly MySqlConnection _con;
        public DealCodeRedeemConfig(devhoorayContext context)
        {
            _context = context;
            _con = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
        }

        public int GetDeal(string campaign_id)
        {
            var result = (from c in _context.HryCampaign
                          join cd in _context.HryCampaignDeal
                          on c.CampaignId equals cd.CampaignId
                          where cd.CampaignId == campaign_id
                          select new
                          {
                              deal_id = cd.DealId
                          }).FirstOrDefault();

            return result.deal_id;
        }

        public async Task<HryUserProfile> GetUser(string lineUserId, string displayName)
        {
            try
            {
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

        public async Task<(int, object)> ClientCodeRedeem(int dealId, string displayName, int userId)
        {
            try
            {
                Uri url = new Uri("https://privilege-api.dev.fysvc.com/api/v1/client-deal/redeem");
                //var dealCodeRedeem = new DealCodeRedeemRequest(dealId, "New", 291);
                var dealCodeRedeem = new DealCodeRedeemRequest(dealId, displayName, userId);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("x-api-key:" + "BgaPWiaxWjLxvsOxILkpirMqlhqPvCOqfGHoyoYwcIRXkDbdQsUMBjpzCZBONyES");
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
                {
                    string json = JsonConvert.SerializeObject(dealCodeRedeem);

                    streamWriter.WriteAsync(json);
                    streamWriter.FlushAsync();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = await streamReader.ReadToEndAsync();
                    var response = JsonConvert.DeserializeObject<DealCodeRedeemViewModel>(result);
 
                    //return (200, JsonConvert.DeserializeObject(result));
                    return (200,response);
                }
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;

                using (var streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();

                    var res = JsonConvert.DeserializeObject<StatusCodeResponseViewModel>(result);

                    Error error = new Error(res.error.code, res.error.message);

                    var data = new StatusCodeResponseViewModel((int)response.StatusCode, error);

                    return (400,data);
                }
            }
        }

        public async Task<int> GetDealId(string pCampaignID)
        {
            DataTable table = new DataTable();
            int deal_id = 0;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "get_deal";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

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
                        deal_id = int.Parse(dr["deal_id"].ToString());
                    }

                    return deal_id;
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
    }
}
