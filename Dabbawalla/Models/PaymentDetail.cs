using System;
using System.Collections.Generic;

namespace Dabbawalla.Models;

public partial class PaymentDetail
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string? PaymentMode { get; set; }

    public decimal? Price { get; set; }

    public string? TransactionStatus { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    
    public virtual OrderDetail? Order { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
