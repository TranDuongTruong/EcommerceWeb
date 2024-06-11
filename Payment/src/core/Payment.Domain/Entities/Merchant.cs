using Payment.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Payment.Domain.Entities
{
    public class Merchant : BaseAuditableEntity
    {
        [Key]
        [Column(TypeName = "nvarchar(50)")]

        public string? MerchtId { get; set; }
        [Column(TypeName = "nvarchar(50)")]

        public string? MerchtName { get; set; }
        [Column(TypeName = "nvarchar(250)")]

        public string? WebLink { get; set; }

        public bool IsActive { get; set; }
        [Column(TypeName = "nvarchar(50)")]

        public string? SecretKey { get; set; }
        [Column(TypeName = "nvarchar(250)")]

        public string? MerchtIpnUrl { get; set; }
        [Column(TypeName = "nvarchar(250)")]

        public string? MerchtReturnUrl { get; set; }

     
    }
}