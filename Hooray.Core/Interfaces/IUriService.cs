using Hooray.Core.ViewModels;
using System;

namespace Hooray.Core.Interfaces
{
    public interface IUriService
    {
        Uri GetPageUri(PaginationFilter paginationFilter, string route);

    }
}
