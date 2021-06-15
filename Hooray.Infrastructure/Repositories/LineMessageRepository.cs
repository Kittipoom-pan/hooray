using Hooray.Core.Entities;
using Hooray.Core.Interfaces;
using Hooray.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hooray.Infrastructure.Repositories
{
    public class LineMessageRepository : ILineMessageRepository
    {
        private readonly devhoorayContext _context;
        public LineMessageRepository(devhoorayContext context)
        {
            _context = context;
        }

        public async Task AddLineChannel(string channel_token, int company_id)
        {
            try
            {
                var data = await _context.LineChannelAccess.Where(t => channel_token.Contains(t.ChannelAccessToken)).FirstOrDefaultAsync();

                if (data != null)
                {
                    if (data.ChannelAccessToken != channel_token)
                    {
                        var model = new LineChannelAccess()
                        {
                            ChannelAccessToken = channel_token,
                            CompanyId = company_id,
                            CreateDate = DateTime.Now
                        };

                        _context.LineChannelAccess.Add(model);
                        _context.SaveChanges();
                    }
                    else
                    {
                        data.CompanyId = company_id;
                        data.UpdateDate = DateTime.Now;

                        _context.LineChannelAccess.Update(data);
                        _context.SaveChanges();
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddSendMessageLog(string user_id)
        {
            var campaignAction = new HryCampaignAction()
            {
                UserId = user_id,
                EventName = "prize sent",
                EventType = 17,
                CreateDate = DateTime.Now
            };

            _context.HryCampaignAction.Add(campaignAction);
            _context.SaveChanges();
        }

        public async Task<string> GetTokenLine(string env)
        {
            var data = await _context.LineChannelAccess.Where(t => t.Environment == env).FirstOrDefaultAsync();
            return data.ChannelAccessToken;
        }
    }
}
