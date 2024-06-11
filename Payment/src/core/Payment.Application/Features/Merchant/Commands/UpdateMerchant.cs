using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Features.Commands
{
    public class UpdateMerchant
    {


        public string? MerchtId { get; set; } = string.Empty;

        public string? MerchtName { get; set; } = string.Empty;

        public string? WebLink { get; set; } = string.Empty;



        public string? SecretKey { get; set; } = string.Empty;

        public string? MerchtIpnUrl { get; set; } = string.Empty;

        public string? MerchtReturnUrl { get; set; } = string.Empty;
    }
}
