using System;

namespace Hooray.Core.ViewModels
{
    public class PagedResponse<T> : BaseResponse<T>
    {
        public int page_number { get; set; }
        public int page_size { get; set; }
        public Uri first_page { get; set; }
        public Uri last_page { get; set; }
        public int total_pages { get; set; }
        public int total_records { get; set; }
        public Uri next_page { get; set; }
        public int next_page_number { get; set; }
        public Uri previous_page { get; set; }
        public int previous_page_number { get; set; }
        public int campaign_like_count { get; set; }
        public int campaign_comment_count { get; set; }
        public PagedResponse(T data, int pageNumber, int pageSize, string message, bool deviceTokenStatus, bool status, bool statusLogin, StartupBadgeViewModel startupBadge)
        {
            this.data = data;
            base.message = message;
            device_token_status = deviceTokenStatus;
            base.status = status;
            status_login = statusLogin;
            startup_badge = startupBadge;
            this.page_number = pageNumber;
            this.page_size = pageSize;
        }
        public PagedResponse(T data, int pageNumber, int pageSize, string message, bool deviceTokenStatus, bool status, bool statusLogin, StartupBadgeViewModel startupBadge, int campaign_like_count, int campaign_comment_count)
        {
            this.data = data;
            base.message = message;
            device_token_status = deviceTokenStatus;
            base.status = status;
            status_login = statusLogin;
            startup_badge = startupBadge;
            this.page_number = pageNumber;
            this.page_size = pageSize;
            this.campaign_like_count = campaign_like_count;
            this.campaign_comment_count = campaign_comment_count;
        }
    }
}
