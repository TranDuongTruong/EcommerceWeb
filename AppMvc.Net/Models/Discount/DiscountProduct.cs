using System.ComponentModel.DataAnnotations.Schema;
using AppMvc.Net.Models.Discount;
using AppMvc.Net.Models.Product;

namespace AppMvc.Net.Models.Discount
{
    [Table("DiscountProduct")]
    public class DiscountProduct
    {
        public int ProductID { set; get; }

        public int DiscountID { set; get; }


        [ForeignKey("ProductID")]
        public ProductModel Product { set; get; }


        [ForeignKey("DiscountID")]
        public DiscountModel Discount { set; get; }
    }
}