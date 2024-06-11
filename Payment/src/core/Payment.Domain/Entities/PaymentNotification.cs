using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Payment.Domain.Entities
{
    public class PaymentNotification
    {

        [Key]
        public string? NotiId { get; set; }
        public string? RefId { get; set; }
        public DateTime? NotiDate { get; set; }
        public string? NotiStatus { get; set; }
        public decimal? NotiAmount { get; set; }
        public string? NotiContent { get; set; }
        public string? NotiMessage { get; set; }
        public DateTime? NotiResDate { get; set; }
        public string? NotiSignature { get; set; }

        // Foreign keys
        public string? PaymtId { get; set; }

        public string? MerchtId { get; set; }
    }
}
