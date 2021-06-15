using System.Data;

namespace Hooray.Core.ViewModels
{
    public class CampaignResultViewModel
    {
            public string image_url { get; set; }
            public void loadDataImageCampaignResult(DataRow dr)
            {
                image_url = dr["path"].ToString();
            }
       
    }
}
