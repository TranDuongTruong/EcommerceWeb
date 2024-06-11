using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Features.Dtos
{
    public class PaymentTransactionDtos
    {
        public string? TranId { get; set; } = string.Empty;
        public string? TranMessage { get; set; } = string.Empty;
        public string? TranPayload { get; set; } = string.Empty;
        public string? TranStatus { get; set; } = string.Empty;
        public decimal? TranAmount { get; set; }
        public DateTime? TranDate { get; set; }
        public string? PaymtId { get; set; } = string.Empty;
        public string? TranRefId { get; set; } = string.Empty;
        public string? MerchantName { get; set; } = string.Empty;
        public string? DesName { get; set; } = string.Empty;
    }
}
