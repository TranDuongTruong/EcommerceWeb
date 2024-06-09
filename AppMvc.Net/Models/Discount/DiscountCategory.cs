using System.ComponentModel.DataAnnotations.Schema;
using AppMvc.Net.Models.Discount;
using AppMvc.Net.Models.Product;

namespace AppMvc.Net.Models.Discount
{
    [Table("DiscountCategory")]
    public class DiscountCategory
    {
        public int CategoryID { set; get; }

        public int DiscountID { set; get; }


        [ForeignKey("CategoryID")]
        public CategoryProduct CategoryProduct { set; get; }


        [ForeignKey("DiscountID")]
        public DiscountModel Discount { set; get; }
    }
}