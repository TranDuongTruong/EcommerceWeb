using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Base.Models
{
    public class BasePagingData<T>
    {
        public int PageIndex { get; set; } 
        public int PageSize { get; set; }
        public List<T> Items { get; set; }  = new List<T>();
        public int TotalPage { get; set; }
        public int TotalItem { get; set; }
        public string? NextPageUrl { get; set; } = string.Empty;
        public string? PreviousPageUrl { get;  set; } = string.Empty;
    }
}
