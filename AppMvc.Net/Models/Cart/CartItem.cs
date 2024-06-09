using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppMvc.Net.Models.Product;

namespace AppMvc.Net.Models.Cart
{
    [Table("CartItem")]
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }

        [NotMapped]
        public decimal TotalPrice
        {
            get { return Quantity * (Product?.Prices ?? 0); }
        }


        public int? CartId { get; set; }

        [ForeignKey("CartId")]
        public CartModel Cart { get; set; }

        public int? CheckoutViewModelId { get; set; }

        [ForeignKey("CheckoutViewModelId")]
        public CheckoutViewModel CheckoutViewModel { get; set; }


    }
}