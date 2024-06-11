using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Features.Dtos 
{ 
    public class PaymentSignatureDtos
    {
        public string PaymtSignId { get; set; } = string.Empty;

        public string? SignValue { get; set; } = string.Empty;

        public string? SignAlgo { get; set; } = string.Empty;
        public DateTime? SignDate { get; set; }
        public string? SignOwn { get; set; } = string.Empty;
        // Foreign key
        public string? PaymtId { get; set; } = string.Empty;

        public bool IsValid { get; set; }
    }
}
