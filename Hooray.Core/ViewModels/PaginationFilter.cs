using System;
using System.Collections.Generic;
using System.Text;

namespace Hooray.Core.ViewModels
{
    public class PaginationFilter
    {
        public int page_number { get; set; }
        public int page_size { get; set; }
        public PaginationFilter()
        {
            this.page_number = 1;
            this.page_size = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.page_number = pageNumber < 1 ? 1 : pageNumber;
            this.page_size = pageSize > 10 ? 10 : pageSize;
        }
    }
}
