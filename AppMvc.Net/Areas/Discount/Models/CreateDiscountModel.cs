using System.ComponentModel.DataAnnotations;
using AppMvc.Net.Models.Blog;
using AppMvc.Net.Models.Discount;
using AppMvc.Net.Models.Product;

namespace AppMvc.Net.Areas.Discount.Models
{
    public class CreateDiscountModel : DiscountModel
    {
        public CreateDiscountModel() { }
        [Display(Name = "Chuyên mục")]
        public int[] CategoryIDs { get; set; }

        [Display(Name = "San pham")]
        public int[] ProductIDs { get; set; }

        
    }
}