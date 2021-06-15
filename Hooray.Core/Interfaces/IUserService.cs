using Hooray.Core.RequestModels;
using Hooray.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<HoorayFollow>> AddUserFollow(AddUserFollowRequest model);

        Task<PagedResponse<List<UserResult>>> GetAllUserResult(int uid, int join, int announcetype, string lang, PaginationFilter pageFilter,string url);
    }
}
