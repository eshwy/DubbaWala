using System;
using System.Collections.Generic;

namespace Dabbawalla.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? FoodId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public int? AddressId { get; set; }

    public int? PaymentDetailsId { get; set; }

    public bool? Completed { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UserId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<DeliveryStatus> DeliveryStatuses { get; set; } = new List<DeliveryStatus>();

    public virtual PaymentDetail? PaymentDetails { get; set; }

    public virtual ICollection<PaymentDetail> PaymentDetailsNavigation { get; set; } = new List<PaymentDetail>();
}
