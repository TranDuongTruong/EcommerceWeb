using System.ComponentModel.DataAnnotations;

using AppMvc.Net.Models.Blog;
using AppMvc.Net.Models.Product;

namespace AppMvc.Net.Areas.Product.Models
{
    public class CreateProductModel : ProductModel
    {
        [Display(Name = "Chuyên mục")]
        public int[] CategoryIDs { get; set; }

    }
}