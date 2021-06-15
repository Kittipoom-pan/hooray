using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.DBContexts;
using Hooray.Infrastructure.Manager;
using Hooray.Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Hooray.Core.ViewModels.CampaignJoinViewModel;

namespace Hooray.Infrastructure.MySQLManager
{
    public class MySQLManager : IMySQLManager
    {
        private readonly devhoorayContext _context;

        private readonly MySqlConnection _con;
        public MySQLManager(devhoorayContext context)
        {
            _context = context;
            _con = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString);
        }

        public async Task<StartupBadgeViewModel> GetStartupBadge(int pUserID)
        {
            StartupBadgeViewModel startupBadge = new StartupBadgeViewModel();

            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _con;
                _con.Open();
                cmd.CommandText = "hry_get_startup_badge";
                cmd.CommandType = CommandType.StoredProcedure;

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
                    startupBadge.loadDataStartupBadge(table.Rows[0]);
                }
            }
            return startupBadge;
        }


        public string GetMessageLang(string Lang, int MsgCode, string UserIDAction, string UserIDReaction)
        {
            string messageText = string.Empty;
            try
            {
                List<MessageUI> listMsg = new List<MessageUI>();
                listMsg = GetMessageLang(Lang.ToUpper(), MsgCode, 0);
                messageText = listMsg[0].message_text;
                if (!UserIDAction.Equals(""))
                {
                    messageText = messageText.Replace("{user_id_action}", UserIDAction);
                }
                if (!UserIDReaction.Equals(""))
                {
                    messageText = messageText.Replace("{user_id_reaction}", UserIDReaction);
                }
            }
            catch (Exception ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MessageControler");
            }

            return messageText;
        }

        public List<MessageUI> GetMessageLang(string pLang, int pMsgCode, int pMsgType)
        {
            List<MessageUI> msgList = new List<MessageUI>();
            var table = _context.SystemMessage.AsQueryable();
            if (pMsgType == 3)
            {
                table = table.Where(s => s.MessageLang == pLang && s.MessageType == pMsgType && s.MessageCode.ToString().Contains("31"));
            }
            else
            {
                if (string.IsNullOrEmpty(pLang))
                {
                    table = table.Where(s => s.MessageCode == pMsgCode);
                }
                else
                {
                    table = table.Where(s => s.MessageLang == pLang && s.MessageCode == pMsgCode);
                }
            }
            var data = table.ToList();
            if (data != null && data.Count > 0)
            {
                MessageUI msg;
                foreach (var row in data)
                {
                    msg = new MessageUI();
                    msg.loadDataMessageUI(row);
                    msgList.Add(msg);
                }
            }

            return msgList;
        }

        public async Task<bool> CheckVersion(float pVersionApp, string pDeviceType)
        {
            bool version_status = false;

            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _con;
                _con.Open();
                cmd.CommandText = "hry_check_version";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pVersionApp", pVersionApp);
                cmd.Parameters.AddWithValue("@pDeviceType", pDeviceType);

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
                    version_status = Convert.ToBoolean(int.Parse(dr["version_status"].ToString()));
                }
            }

            return version_status;
        }


        public async Task<bool> CheckGroup(string campaign_id)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    cmd.Connection = _con;
                    if (_con.State == ConnectionState.Closed)
                    {
                        _con.Open();
                    }
                    cmd.CommandText = "hry_check_group";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCampaignID", campaign_id);

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
                        success = Convert.ToBoolean(int.Parse(dr["check_group"].ToString()));
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

                return success;
            }
        }

        public async Task<bool> CheckWinGroup(int user_id, string campaign_id)
        {
            bool success = false;

            DataTable table = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_check_win_group";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCampaignID", campaign_id);

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
                        success = Convert.ToBoolean(int.Parse(dr["check_win_group"].ToString()));
                    }
                    return success;
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

        public async Task<int> CheckFollowAndJoin(int company_id, int user_id, string campaign_id)
        {
            int statusJoin = 0;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_check_follow_and_join";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCompanyID", company_id);
                    cmd.Parameters.AddWithValue("@pCampaignID", campaign_id);
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
                        statusJoin = int.Parse(dr["status_join"].ToString());
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

                return statusJoin;
            }
        }

        public async Task<CheckCampaignViewModel> CheckCampaignBeforeJoin(string campaign_id, int user_id)
        {
            CheckCampaignViewModel checkCampaign = null;
            DataTable table = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_check_campaign_before_join";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCampaignID", campaign_id);
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
                        checkCampaign = new CheckCampaignViewModel();

                        foreach (DataRow row in table.Rows)
                        {
                            checkCampaign.loadDataCheckCampaign(row);
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
            }

            return checkCampaign;
        }

        public async Task<int> GetCampaignDigit(string campaign_id) =>
            Convert.ToInt32(await _context.HryCampaign.Where(t => t.CampaignId == campaign_id).Select(i => i.ResultDigit).FirstOrDefaultAsync());

        public async Task<Join> InsertNewUserJoin(string campaign_id, int user_id, double join_number, float lat, float lng, string verify)
        {
            Join joinList = null;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_insert_user_join";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCampaignID", campaign_id);
                    cmd.Parameters.AddWithValue("@pUserID", user_id);
                    cmd.Parameters.AddWithValue("@pEnjoyNumber", join_number);
                    cmd.Parameters.AddWithValue("@pLat", lat);
                    cmd.Parameters.AddWithValue("@pLng", lng);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }

                    if (table != null && table.Rows.Count > 0)
                    {
                        joinList = new Join();
                        Join join;
                        CampaignPrizeViewModel campaignprize;

                        foreach (DataRow row in table.Rows)
                        {
                            join = new Join();
                            join.loadDataJoin(row, join_number);

                            //join.campaign_user_join_id

                            double accum_win_rate = 0;
                            int check_announce = 0;
                            double join_number_rate = Utility.GetRandomNumberDouble(0.0000001, 1);

                            _con.Close();
                            await UpdateCPUserCodeNumber(join.id, check_announce, join_number_rate);
                            await UpdateVerifyForJoin(join.id, verify);

                            DataTable tableCheckCampaign = GetCampaignPrize(campaign_id);
                            foreach (DataRow row2 in tableCheckCampaign.Rows)
                            {
                                campaignprize = new CampaignPrizeViewModel();
                                campaignprize.loadDataPrize(row2);

                                //announce_type >> 1=สุ่มรหัสให้, 2=รอประกาศผล, 3=กำหนดตำแหน่ง
                                if (join.announce_type == 1)
                                {
                                    accum_win_rate = accum_win_rate + campaignprize.win_rate;

                                    if (join_number_rate < accum_win_rate)
                                    //if(true)
                                    {
                                        bool isPrize = UpdateCurrentWin(Convert.ToInt32(join.campaign_id), join.id, campaignprize.prize_id);
                                        if (isPrize)
                                        {
                                            join.announce_text = row["win_text"].ToString();
                                            join.result = campaignprize.prize_order;
                                            break;
                                        }
                                        else
                                        {
                                            join.announce_text = row["lost_text"].ToString();
                                            join.result = 0;
                                        }
                                    }
                                    else
                                    {
                                        join.announce_text = row["lost_text"].ToString();
                                        join.result = 0;
                                    }

                                }

                                ///******** flow ผิด ยังไม่สามารถใช้งาน type 3 ได้
                                else if (join.announce_type == 3)
                                {
                                    bool check_win_reward = CheckAnnounce(Convert.ToInt32(join.campaign_id), join.count_user);

                                    await UpdateCPUserCodeNumber(join.id, check_announce, 0);

                                    if (check_win_reward)
                                    {
                                        bool isPrize = UpdateCurrentWin(Convert.ToInt32(join.campaign_id), join.id, campaignprize.prize_id);
                                        if (isPrize)
                                        {
                                            join.announce_text = row["win_text"].ToString();
                                            join.result = campaignprize.prize_order;
                                            break;
                                        }
                                        else
                                        {
                                            join.announce_text = row["lost_text"].ToString();
                                            join.result = 0;
                                        }
                                    }
                                    else
                                    {
                                        join.announce_text = row["lost_text"].ToString();
                                        join.result = 0;
                                    }
                                }
                                else
                                {
                                    await UpdateCPUserCodeNumber(join.id, check_announce, 0);
                                    join.announce_text = "รอลุ้นผลรางวัล ในวันที่ " + join.announce_date;
                                    join.result = 0;
                                    break;
                                }
                            }
                            //joinList.Add(join);
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
            }
            return joinList;
        }

        public async Task<bool> UpdateCPUserCodeNumber(int pCPUserCodeID, int pCheckAnnounce, double pJoinNumberRate)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_update_campaign_user_code_number";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCPUserCodeID", pCPUserCodeID);
                    cmd.Parameters.AddWithValue("@pCheckAnnounce", pCheckAnnounce);
                    cmd.Parameters.AddWithValue("@pJoinNumberRate", pJoinNumberRate);

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
                catch (System.IO.IOException e)
                {
                    throw e;
                }

                finally
                {
                    _con.Close();
                }
                return success;
            }
        }

        public async Task<bool> UpdateVerifyForJoin(int pCPUserJoinID, string pVerifyForJoin)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_update_verify_for_join";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCampaignUserCodeID", pCPUserJoinID);
                    cmd.Parameters.AddWithValue("@pVerifyForJoin", pVerifyForJoin);

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
                catch (System.IO.IOException e)
                {
                    throw e;
                }

                finally
                {
                    _con.Close();
                }
                return success;
            }
        }
        public DataTable GetCampaignPrize(string pCampaignID)
        {
            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                try
                {
                    cmd.Connection = _con;
                    _con.Open();
                    cmd.CommandText = "hry_get_campaign_prize_realtime";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
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

                return table;
            }
        }
        public bool UpdateCurrentWin(int pCampaignID, int pCPUserJoinID, int pPrizeID)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _con;
                _con.Open();
                cmd.CommandText = "hry_update_current_win";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pCPUserJoinID", pCPUserJoinID);
                cmd.Parameters.AddWithValue("@pPrizeID", pPrizeID);

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
                    success = Convert.ToBoolean(int.Parse(dr["isPrize"].ToString()));
                }
                return success;
            }
        }

        public bool CheckAnnounce(int pCampaignID, int pCountUser)
        {
            bool success = false;

            DataTable table = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = _con;
                _con.Open();
                cmd.CommandText = "hry_check_announce";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@pCampaignID", pCampaignID);
                cmd.Parameters.AddWithValue("@pCountUser", pCountUser);

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
                    success = Convert.ToBoolean(dr["user_announce"]);
                }
                return success;
            }
        }

        public string GetMessagePushAnnounce(string Lang, int MsgCode, string CampaignName, string CompanyName)
        {
            string messageText = string.Empty;
            try
            {
                List<MessageUI> listMsg = new List<MessageUI>();
                listMsg = GetMessageLang(Lang.ToUpper(), MsgCode, 0);

                messageText = listMsg[0].message_text;

                if (!CampaignName.Equals(""))
                {
                    messageText = messageText.Replace("{campaign_name}", CampaignName);
                }
                if (!CompanyName.Equals(""))
                {
                    messageText = messageText.Replace("{compant_name}", CompanyName);
                }
            }
            catch (Exception ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MessageControler");
            }

            return messageText;
        }

        public string GetMessageLangCard(string Lang, int MsgCode, string UserIDAction, string UserIDReaction, string CardName)
        {
            string messageText = string.Empty;
            try
            {
                List<MessageUI> listMsg = new List<MessageUI>();
                listMsg = GetMessageLang(Lang.ToUpper(), MsgCode, 0);
                messageText = listMsg[0].message_text;

                if (!UserIDAction.Equals(""))
                {
                    messageText = messageText.Replace("{user_id_action}", UserIDAction);
                }
                if (!UserIDReaction.Equals(""))
                {
                    messageText = messageText.Replace("{user_id_reaction}", UserIDReaction);
                }
                if (!CardName.Equals(""))
                {
                    messageText = messageText.Replace("{card_name}", CardName);
                }
            }
            catch (Exception ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MessageControler");
            }

            return messageText;
        }
    }
}
