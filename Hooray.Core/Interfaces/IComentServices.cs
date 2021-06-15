using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface IComentServices
    {
        Task<PagedResponse<List<CommentModel>>> GetAllFeedCommentList(int uid, string cpid, PaginationFilter pageFilte, string lang, string url);
        BaseResponse<CommentModel> AddNewComment(int uid, string cpid, string comment, string tokenID, string lang);
    }
}
