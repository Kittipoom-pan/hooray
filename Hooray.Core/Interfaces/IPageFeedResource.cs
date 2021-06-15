using Hooray.Core.Services;
using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.Interfaces
{
    public interface IPageFeedResource 
    {
        BaseResponse<bool> AddQuestionResult(AddQuestionResultModel model);
    }
}
