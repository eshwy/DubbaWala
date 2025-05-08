using System;
using System.Collections.Generic;

namespace Dabbawalla.Models;

public partial class Address
{
    public int Id { get; set; }

    public string? DoorNumber { get; set; }

    public string? Street { get; set; }

    public string? Area { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? AddressType { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
