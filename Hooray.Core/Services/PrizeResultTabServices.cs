using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Hooray.Infrastructure.Manager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Hooray.Core.Services
{
    public class PrizeResultTabServices : IPrizeResultTabServices
    {
        private IMySQLManagerRepository _sql;
        private IMySQLManager _msg;
        private int messagecode = 0;
        private string clear = "";
        private readonly ILogger _logger;
        public PrizeResultTabServices(IMySQLManagerRepository mySQLManagerRepository, IMySQLManager mySQL, ILogger<PrizeResultTabServices> logger)
        {
            _logger = logger;
            _sql = mySQLManagerRepository;
            _msg = mySQL;
        }
        public BaseResponse<List<UserPrize>> GetPrizeList(int uid, string lang)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<List<UserPrize>> obj = new BaseResponse<List<UserPrize>>();
            obj.message = string.Empty;
            obj.status = true;
            obj.status_login = true;
            obj.device_token_status = true;
            obj.data = new List<UserPrize>();
            messagecode = 0;

            try
            {
                if (clear == "")
                {
                    obj.data = _sql.GetAllPrizeList(uid);
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
                _logger.LogError(ex, string.Format("GetPrizeList -- {0}", obj.message));

                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetPrizeList");
            }
            finally
            {
            }

            return obj;
        }

        public BaseResponse<PrizeDetailModel> GetPrizeDetail(int uid, int upid, string lang)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<PrizeDetailModel> obj = new BaseResponse<PrizeDetailModel>();
            obj.message = string.Empty;
            obj.status = true;
            obj.status_login = true;
            obj.device_token_status = true;
            obj.data = new PrizeDetailModel();
            messagecode = 0;

            try
            {
                if (clear == "")
                {
                    obj.data = _sql.GetPrizeDetail(upid);
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

                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetPrizeDetail");
            }
            finally
            {
            }

            return obj;
        }

        public BaseResponse<HoorayDeleteUserPrize> DeleteUserPrize(DeleteUserPrizeModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<HoorayDeleteUserPrize> obj = new BaseResponse<HoorayDeleteUserPrize>();
            obj.message = string.Empty;
            obj.status = false;
            obj.data = new HoorayDeleteUserPrize();
            obj.data.status_delete = false;
            messagecode = 0;

            try
            {
                //string[] values = Regex.Split(model.upid, @"\[\*\]");

                //for (int i = 0; i < values.Length; i++)
                //{
                //    string user_prize_id = values[i];
                //    obj.data.status_delete = _sql.DeleteUserPrize(uid, int.Parse(user_prize_id));
                //    obj.status = obj.data.status_delete;
                //}
                if (model.upid.Count >0)
                {
                    foreach (var item in model.upid)
                    {
                        obj.data.status_delete = _sql.DeleteUserPrize(model.uid, int.Parse(item));
                        obj.status = obj.data.status_delete;
                    }
                }
                
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");

                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "GetPrizeDetail");
            }
            finally
            {
            }

            return obj;
        }
    }
}
