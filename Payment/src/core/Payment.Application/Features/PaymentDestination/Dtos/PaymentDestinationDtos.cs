using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Features.Dtos
{
    public class PaymentDestinationDtos
    {
        public string? PaymtDesId { get; set; }

        public string? DesLogo { get; set; }

        public string? DesShortName { get; set; }

        public string? DesName { get; set; }
        public int DesSortIndex { get; set; }
        public bool IsActive { get; set; }
        public string? PaymtId { get; set; }
        public string? DesParentId { get; set; } = string.Empty;

    }
}