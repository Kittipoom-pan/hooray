using Hooray.Core.Interfaces;
using Hooray.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace Hooray.Infrastructure.Helpers
{
    public class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, 
            IUriService uriService, string route, string message, bool deviceTokenStatus, bool status, bool statusLogin, StartupBadgeViewModel startupBadge)
        {
            var respose = new PagedResponse<List<T>>(pagedData, validFilter.page_number, validFilter.page_size, message, deviceTokenStatus,
                            status, statusLogin, startupBadge);
            var totalPages = ((double)totalRecords / (double)validFilter.page_size);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.next_page =
                validFilter.page_number >= 1 && validFilter.page_number < roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.page_number + 1, validFilter.page_size), route)
                : null;
            respose.next_page_number = validFilter.page_number >= 1 && validFilter.page_number < roundedTotalPages ? validFilter.page_number + 1 : 0;
            respose.previous_page =
                validFilter.page_number - 1 >= 1 && validFilter.page_number <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.page_number - 1, validFilter.page_size), route)
                : null;
            respose.previous_page_number = validFilter.page_number - 1 >= 1 && validFilter.page_number <= roundedTotalPages ? validFilter.page_number - 1 : 0;
            respose.first_page = uriService.GetPageUri(new PaginationFilter(1, validFilter.page_size), route);
            respose.last_page = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.page_size), route);
            respose.total_pages = roundedTotalPages;
            respose.total_records = totalRecords;
            respose.message = message;
            respose.device_token_status = deviceTokenStatus;
            respose.status = status;
            respose.status_login = statusLogin;
            respose.startup_badge = startupBadge;
            return respose;
        }


        public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords,
           IUriService uriService, string route, string message, bool deviceTokenStatus, bool status, bool statusLogin, StartupBadgeViewModel startupBadge , int campaign_like_count, int campaign_comment_count)
        {
            var respose = new PagedResponse<List<T>>(pagedData, validFilter.page_number, validFilter.page_size, message, deviceTokenStatus,
                            status, statusLogin, startupBadge , campaign_like_count , campaign_like_count);
            var totalPages = ((double)totalRecords / (double)validFilter.page_size);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.next_page =
                validFilter.page_number >= 1 && validFilter.page_number < roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.page_number + 1, validFilter.page_size), route)
                : null;
            respose.next_page_number = validFilter.page_number >= 1 && validFilter.page_number < roundedTotalPages ? validFilter.page_number + 1 : 0;
            respose.previous_page =
                validFilter.page_number - 1 >= 1 && validFilter.page_number <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.page_number - 1, validFilter.page_size), route)
                : null;
            respose.previous_page_number = validFilter.page_number - 1 >= 1 && validFilter.page_number <= roundedTotalPages ? validFilter.page_number - 1 : 0;
            respose.first_page = uriService.GetPageUri(new PaginationFilter(1, validFilter.page_size), route);
            respose.last_page = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.page_size), route);
            respose.total_pages = roundedTotalPages;
            respose.total_records = totalRecords;
            respose.message = message;
            respose.device_token_status = deviceTokenStatus;
            respose.status = status;
            respose.status_login = statusLogin;
            respose.status_login = statusLogin;
            respose.startup_badge = startupBadge;
            respose.campaign_like_count = campaign_like_count;
            respose.campaign_comment_count = campaign_comment_count;
            return respose;
        }
    }
}
