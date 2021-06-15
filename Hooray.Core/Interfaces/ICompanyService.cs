using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface ICompanyService
    {
        Task<PagedResponse<List<CompanyDetailModel>>> GetFollowCompany(int uid, int follow, string lang, PaginationFilter pageFilter , string route);
        PagedResponse<List<FollowModel>> GetAllCampaignFollowCompanyPage(int uid, int sid, string lang, PaginationFilter pageFilter ,string route);
        PagedResponse<List<FollowModel>> GetAllCompanyFollowCampaignList(string uid, string sid, string lang, PaginationFilter pageFilter ,string route);
        BaseResponse<CompanyDetail> GetFollowCompanyDetail(int uid, int sid, string lang);
    }
}
