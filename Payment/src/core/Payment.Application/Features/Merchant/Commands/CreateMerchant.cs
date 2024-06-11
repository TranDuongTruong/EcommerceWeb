using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Features.Commands
{
    public class CreateMerchant
    {
        public string? MerchtName { get; set; }=string.Empty;
        [Column(TypeName = "nvarchar(250)")]
        public string? WebLink { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(250)")]
        public string? MerchtIpnUrl { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(250)")]
        public string? MerchtReturnUrl { get; set; } = string.Empty;

    }
}
