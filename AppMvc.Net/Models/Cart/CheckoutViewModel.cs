using System.ComponentModel.DataAnnotations;
using AppMvc.Net.Areas.Product.Models;
namespace AppMvc.Net.Models.Cart
{
    public class CheckoutViewModel
    {
        [Key]
        public int CheckoutViewModelId { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get; set; }
        public string ShopDiscountCode { get; set; }
        public string SystemDiscountCode { get; set; }
    }
}
