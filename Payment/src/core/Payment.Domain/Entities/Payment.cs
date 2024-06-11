using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Payment.Domain.Entities
{


public class Payment
{
    [Key]
    [Column(TypeName = "nvarchar(50)")]

    public string PaymtId { get; set; }
    [Column(TypeName = "nvarchar(20)")]

    public string? PaymtStatus { get; set; }
    [Column(TypeName = "nvarchar(250)")]

    public string? PaymtContent { get; set; }
    [Column(TypeName = "nvarchar(50)")]

    public string? PaymtCurrency { get; set; }
    [Column(TypeName = "nvarchar(50)")]

    public string? RefId { get; set; }
    public decimal? PaidAmount { get; set; }
    [Column(TypeName = "nvarchar(50)")]

    public string? PaymtLastMessage { get; set; }
    public DateTime? PaymtDate { get; set; }
    public decimal? RequiredAmount { get; set; }
    public DateTime? ExpireDate { get; set; }
    [Column(TypeName = "nvarchar(10)")]

    public string? PaymtLanguage { get; set; }

    // Foreign keys
    public string? PaymtDesId { get; set; }

    public string? MerchtId { get; set; }
 

   
}
}