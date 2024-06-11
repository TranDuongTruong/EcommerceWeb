using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Features.Commands
{
    public class UpdatePaymentDestination
    {
        public string? PaymtDesId { get; set; }

        public string? DesLogo { get; set; } = string.Empty;

        public string? DesShortName { get; set; } = string.Empty;

        public string? DesName { get; set; } = string.Empty;
        public int DesSortIndex { get; set; }

        public string? DesParentId { get; set; } = string.Empty;

    }
}
