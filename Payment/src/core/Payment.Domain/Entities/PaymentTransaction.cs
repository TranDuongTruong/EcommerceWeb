using Payment.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Payment.Domain.Entities
{
    public class PaymentTransaction : BaseAuditableEntity
    {
        [Key]
        [Column(TypeName = "nvarchar(50)")]

        public string? TranId { get; set; }
        [Column(TypeName = "nvarchar(50)")]

        public string? TranMessage { get; set; }
        [Column(TypeName = "nvarchar(50)")]

        public string? TranPayload { get; set; }
        [Column(TypeName = "nvarchar(50)")]

        public string? TranStatus { get; set; }


        public decimal? TranAmount { get; set; }
        public DateTime? TranDate { get; set; }

        // Foreign key
        [Column(TypeName = "nvarchar(50)")]

        public string? PaymtId { get; set; }
      
    }
}