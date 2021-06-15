using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Hooray.Core.ViewModels
{
    public class BaseResponsePagination
    {
        public string message { get; set; }
        public bool device_token_status { get; set; }
        public bool status { get; set; }
        public bool status_login { get; set; }
        public StartupBadgeViewModel startup_badge { get; set; }
        //public List<T> data { get; set; }
        //public BaseResponsePagination(List<T> data)
        //{
        //    //Succeeded = true;
        //    //message = string.Empty;
        //    //Errors = null;
        //    data = data;
        //}
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPageUrl { get; set; }
        public Uri LastPageUrl { get; set; }
        public int TotalPage { get; set; }
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }

        //public void SetPagination(int total, int perPage, int current)
        //{
        //    this.TotalPage = total;
        //    this.PerPage = perPage;
        //    this.CurrentPage = current;

        //    double lastPage = total / Convert.ToDouble(PerPage);
        //    this.last_page = Convert.ToInt32(Math.Ceiling(lastPage));
        //    this.From = (PerPage * current) - (PerPage - 1);
        //    this.To = (total < perPage) ? total : PerPage * current;
        //    this.next_page = (current < this.last_page) ? GetNextUrl() : null;
        //    this.previous_page = (current > 1) ? GetPrevUrl() : null;

        //    if (total == 0)
        //    {
        //        this.From = 0;
        //        this.To = 0;
        //    }
        //    if (current == this.last_page)
        //    {
        //        this.To = total;
        //    }
        //}

        //private string GetNextUrl()
        //{
        //    return GetUrl(1);
        //}

        //private string GetPrevUrl()
        //{
        //    return GetUrl(-1);
        //}

        //private string GetUrl(int addPage)
        //{
        //    var uri = "";

        //    NameValueCollection parameters = null;
        //    if (!uri.Contains("?"))
        //    {
        //        var str = "?page=" + CurrentPage + "&limit=" + PerPage;
        //        parameters = HttpUtility.ParseQueryString(str);

        //    }
        //    else
        //    {
        //        var queryString = uri.Substring(uri.IndexOf('?'));
        //        parameters = HttpUtility.ParseQueryString(queryString);
        //        if (parameters == null)
        //        {
        //            var str = "?page=" + CurrentPage + "&limit=" + PerPage;
        //            parameters = HttpUtility.ParseQueryString(str);
        //        }
        //    }

        //    parameters["page"] = "" + (CurrentPage + addPage);

        //    return uri.Split('?')[0] + "?" + parameters.ToString();
        //}
    }
}
