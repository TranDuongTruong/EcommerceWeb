
using System.Collections.Generic;
using AppMvc.Net.Models.Blog;
using AppMvc.Net.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace AppMvc.Net.Components
{
    [ViewComponent]
    public class CategoryProductSideBar : ViewComponent
    {

        public class CategorySidebarData
        {
            public List<CategoryProduct> Categories { get; set; }
            public int level { get; set; }

            public string categoryslug { get; set; }

        }

        public IViewComponentResult Invoke(CategorySidebarData data)
        {
            return View(data);
        }

    }
}