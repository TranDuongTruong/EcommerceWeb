
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Payment.Application.Features.Dtos
{
    public class MerchantDtos 
    {
   

        public string? MerchtId { get; set; }
  

        public string? MerchtName { get; set; }


        public string? WebLink { get; set; }

        public bool IsActive { get; set; }


     
     
        public string? MerchtIpnUrl { get; set; }
   

        public string? MerchtReturnUrl { get; set; }
    }
}
