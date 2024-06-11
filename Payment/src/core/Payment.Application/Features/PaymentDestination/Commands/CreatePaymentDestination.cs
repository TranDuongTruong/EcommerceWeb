using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Features.Commands
{
    public class CreatePaymentDestination
    {
        public string? DesLogo { get; set; } = string.Empty;

        public string? DesShortName { get; set; } = string.Empty;

        public string? DesName { get; set; } = string.Empty;
        public int DesSortIndex { get; set; }

        public string? DesParentId { get; set; } = string.Empty;

    }
}
