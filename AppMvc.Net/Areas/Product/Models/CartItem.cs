
using AppMvc.Net.Models.Product;
namespace AppMvc.Net.Areas.Product.Models
{
    public class CartItem
    {
        public int quantity { set; get; }
        public ProductModel product { set; get; }
    }
}