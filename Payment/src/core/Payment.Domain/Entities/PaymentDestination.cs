using Payment.Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Payment.Domain.Entities
{
    public class PaymentDestination : BaseAuditableEntity
    {
        [Key]
        [Column(TypeName = "nvarchar(50)")]

        public string? PaymtDesId { get; set; }
        [Column(TypeName = "nvarchar(250)")]

        public string? DesLogo { get; set; }
        [Column(TypeName = "nvarchar(50)")]

        public string? DesShortName { get; set; }
        [Column(TypeName = "nvarchar(250)")]

        public string? DesName { get; set; }
        public int DesSortIndex { get; set; }
        public bool IsActive { get; set; }
        // Foreign key
        [Column(TypeName = "nvarchar(50)")]

        public string? PaymtId { get; set; }
        public string? DesParentId { get; set; } = string.Empty;

    }
}