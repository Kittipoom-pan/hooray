using Hooray.Core.Entities;
using Hooray.Core.Interfaces;
using Hooray.Infrastructure.DBContexts;
using System;
using System.Threading.Tasks;

namespace Hooray.Infrastructure.Repositories
{
    public class CampaignActionRepository : ICampaignActionRepository
    {
        private readonly devhoorayContext _context;
        public CampaignActionRepository(devhoorayContext context)
        {
            _context = context;
        }

        public async Task AddCampaignAction(string campaign_id, string user_id, string lat, string lng, string event_name, int event_type)
        {
            try
            {
                float lat_ = lat != null && lat != "" ? float.Parse(lat) : 0;
                float lng_ = lng != null && lng != "" ? float.Parse(lng) : 0;

                var campaignAction = new HryCampaignAction
                {
                    CampaignId = campaign_id.ToString(),
                    UserId = user_id,
                    Latitude = lat_,
                    Longitude = lng_,
                    EventName = event_name,
                    EventType = event_type,
                    CreateDate = DateTime.Now
                };
                _context.HryCampaignAction.Add(campaignAction);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddUserJoinCampaign(string campaign_id, string user_id, string lat, string lng, int code_tpye)
        {
            try
            {
                float lat_ = lat != null && lat != "" ? float.Parse(lat) : 0;
                float lng_ = lng != null && lng != "" ? float.Parse(lng) : 0;

                var userJoin = new HryUserJoin
                {
                    CampaignId = campaign_id.ToString(),
                    UserId = user_id,
                    Latitude = lat_,
                    Longitude = lng_,
                    CodeType = code_tpye,
                    CreateDate = DateTime.Now
                };

                _context.HryUserJoin.Add(userJoin);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
