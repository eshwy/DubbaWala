using System;
using System.Collections.Generic;

namespace Dabbawalla.Models;

public partial class DeliveryStatus
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual OrderDetail? Order { get; set; }
}
