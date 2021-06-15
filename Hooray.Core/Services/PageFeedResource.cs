using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Hooray.Core.Services
{
    public class PageFeedResource : IPageFeedResource
    {
        private IMySQLManagerRepository _sql;
        private IMySQLManager _msg;
        private readonly ILogger _logger;
        private int messagecode = 0;
        private string clear = "";
        public PageFeedResource(IMySQLManagerRepository mySQLManagerRepository, IMySQLManager mySQL, ILogger<PageFeedResource> logger)
        {
            _logger = logger;
            _sql = mySQLManagerRepository;
            _msg = mySQL;
        }
        public BaseResponse<bool> AddQuestionResult(AddQuestionResultModel model)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            BaseResponse<bool> obj = new BaseResponse<bool>();
            obj.message = string.Empty;
            obj.status = true;
            obj.device_token_status = true;
            messagecode = 0;

            try
            {
                string receivedata = "uid: " + model.uid + "; qtid: " + model.qtid + "; cucid: " + model.cucid + "; answer: " + model.answer + "; answeroption: " + model.answeroption + "; token: " + model.tokenID + "; lang: " + model.lang;
                int logID = _sql.InsertLogReceiveDataNew("AddQuestionResult", receivedata, "");

                if (clear == "")
                {
                    #region quiestion
                    //[*] คั่นข้อ [$] คั่นคำตอบในข้อนั้นๆ
                    //if (qtid.IndexOf("[*]") < 0)
                    //{
                    //    qtid += "[*]";
                    //}
                    string[] qt = Regex.Split(model.qtid, @"\[\*\]");
                    //qtid.Split('*');
                    #endregion

                    #region answer
                    if (model.answer.IndexOf("[*]") < 0)
                    {
                        model.answer += "[*]";
                    }
                    string[] ans = Regex.Split(model.answer, @"\[\*\]");

                    //string[] ansoption = Regex.Split(answeroption, @"\[\*\]");
                    string ansoption = "";
                    #endregion

                    for (int i = 0; i < qt.Length; i++)
                    {
                        if (ans[i].IndexOf("[$]") < 0)
                        {
                            ans[i] += "[$]";
                        }
                        string[] ans2 = Regex.Split(ans[i], @"\[\$\]");
                        string answer1 = (ans2.Length > 0) ? ans2[0] : "";
                        string answer2 = (ans2.Length > 1) ? ans2[1] : "";
                        string answer3 = (ans2.Length > 2) ? ans2[2] : "";
                        string answer4 = (ans2.Length > 3) ? ans2[3] : "";
                        string answer5 = (ans2.Length > 4) ? ans2[4] : "";
                        string answer6 = (ans2.Length > 5) ? ans2[5] : "";
                        string answer7 = (ans2.Length > 6) ? ans2[6] : "";
                        string answer8 = (ans2.Length > 7) ? ans2[7] : "";
                        string answer9 = (ans2.Length > 8) ? ans2[8] : "";
                        string answer10 = (ans2.Length > 9) ? ans2[9] : "";

                        _sql.InsertQuestionResult(model.uid, int.Parse(qt[i]), model.cucid, answer1, answer2, answer3, answer4, answer5, answer6, answer7, answer8, answer9, answer10, ansoption);
                    }
                    //obj.status = _sql.InsertQuestionResult(uid, qtid, cucid, answer1, answer2, answer3, answer4, answer5, answer6, answer7, answer8, answer9, answer10, answeroption);
                }
                else
                {
                    obj.status = false;
                    obj.device_token_status = true;
                    messagecode = 302002; //Sorry,Your device token invalid.
                    obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                }
            }
            catch (Exception ex)
            {
                obj.status = false;
                messagecode = 302001; //Sorry,internal server error
                obj.message = _msg.GetMessageLang(model.lang, messagecode, "", "");
                _logger.LogError(ex, string.Format("AddQuestionResult -- {0}", obj.message));
                //HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "AddQuestionResult");
            }
            finally
            {
            }

            return obj;
        }
    }
}
