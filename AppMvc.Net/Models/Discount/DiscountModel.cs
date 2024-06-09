using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using AppMvc.Net.Models.Product;

namespace AppMvc.Net.Models.Discount
{
    [Table("Discount")]
    public class DiscountModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        // New property for maximum usage count
        [Required]
        public int MaxUsage { get; set; }

        // New property for tracking the number of times the discount has been used
        public int CurrentUsage { get; set; }


        [Required]
        public DiscountType DiscountType { get; set; }

        [Range(0, 100)]
        public double? Percentage { get; set; }

        // Fixed amount discount
        [Range(0, double.MaxValue)]
        public decimal? Amount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // If the discount applies to specific products

        public List<DiscountProduct> Products { get; set; }

        // If the discount applies to specific categories
        public List<DiscountCategory> CategoryProducts { get; set; }
        // Optional: Adding a property to indicate if the discount is active



        // Discount can apply to products of a specific seller
        public string AuthorId { set; get; }
        [ForeignKey("AuthorId")]
        [Display(Name = "Người tạo")]
        public AppUser Author { set; get; }

        // Indicates if the discount applies to all products in the system
        public bool IsForAllProducts { get; set; }
        [Display(Name = "Đơn hàng tối thiểu")]
        public decimal MinimumOrder { get; set; }

        public bool IsActive
        {
            get
            {
                return (StartDate <= DateTime.Now && EndDate >= DateTime.Now) && (CurrentUsage < MaxUsage);
            }


        }
        [Required]
        public DiscountAuthorType AuthorType { get; set; }
        public decimal ApplyDiscount(decimal originalPrice)
        {
            decimal discountPrice = originalPrice;

            if (this.DiscountType == DiscountType.Amount)
            {
                decimal amount = this.Amount ?? 0m; // Sử dụng 0 nếu Amount là null
                discountPrice = originalPrice - amount;
            }
            else if (this.DiscountType == DiscountType.Percentage)
            {
                double percentage = this.Percentage ?? 0.0; // Sử dụng 0 nếu Percentage là null
                discountPrice = originalPrice - (decimal)((percentage / 100) * (double)originalPrice);
            }

            // Đảm bảo rằng giá trị không âm
            if (discountPrice < 0)
            {
                discountPrice = 0;
            }

            return discountPrice;
        }
    }
}
