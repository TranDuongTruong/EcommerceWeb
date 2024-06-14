using Payment.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Payment.Domain.Entities
{
    public class PaymentSignature 
    {
        [Key]
        [Column(TypeName = "nvarchar(50)")]

        public string PaymtSignId { get; set; }
        [Column(TypeName = "nvarchar(50)")]

        public string? SignValue { get; set; }
        [Column(TypeName = "nvarchar(50)")]

        public string? SignAlgo { get; set; }
        public DateTime? SignDate { get; set; }
        [Column(TypeName = "nvarchar(50)")]


        public string? SignOwn { get; set; }

        // Foreign key
        public string? PaymtId { get; set; }
        public bool IsValid { get; set; }
    }
}