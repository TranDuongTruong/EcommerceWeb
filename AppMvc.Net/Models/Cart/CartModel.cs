using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMvc.Net.Models.Cart
{

    [Table("Cart")]
    public class CartModel
    {
        [Key]
        public int CartId { get; set; }
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        [NotMapped]
        public decimal TotalPrices
        {
            get
            {
                decimal total = 0;
                foreach (var item in CartItems)
                {
                    total += item.TotalPrice;
                }
                return total;
            }
        }
    }
}