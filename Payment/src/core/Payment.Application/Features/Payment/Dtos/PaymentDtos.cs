using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Features.Dtos
{
    public class PaymentDtos
    {
        public string PaymtId { get; set; }
        public string? PaymtContent { get; set; }
        public string? PaymtCurrency { get; set; }
        public string? RefId { get; set; }
         public decimal? RequiredAmount { get; set; }
        public DateTime? PaymtDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? PaymtLanguage { get; set; }
        public string? PaymtDesId { get; set; }
        public string? MerchtId { get; set; } 
        public decimal? PaidAmount { get; set; }

    }
}