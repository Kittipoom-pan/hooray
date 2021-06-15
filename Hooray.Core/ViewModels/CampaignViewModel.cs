using Hooray.Core.Entities;
using System.Collections.Generic;

namespace Hooray.Core.ViewModels
{
    public class CampaignViewModel
    {
        public string company_name { get; set; }
        public string campaign_desc { get; set; }
        public string campaign_short_desc { get; set; }
        public int company_id { get; set; }
        public ICollection<HryMedia> photo_campaign { get; set; }
        public HryMedia photo_company { get; set; }
    }


}
