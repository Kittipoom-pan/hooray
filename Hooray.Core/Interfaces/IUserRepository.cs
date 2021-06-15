using Hooray.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface IUserRepository 
    {
        Task<CompanyFollow> InsertAndGetNewUserFollow(int pCompanyID, int pUserID);
        Task<int> InsertNewNotificationFollow(int pUserID, string pCampaignID);
        Task<bool> UpdateUnFollow(int pCompanyID, int pUserID);
    }
}
