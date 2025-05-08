using System;
using System.Collections.Generic;

namespace Dabbawalla.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? EmailAddress { get; set; }

    public string? PasswordHash { get; set; }

    public int? RoleId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? RestrauntName { get; set; }

    public string? Pancard { get; set; }

    public string? BankIfsc { get; set; }

    public string? BankAccountNumber { get; set; }

    public string? RestrauntName1 { get; set; }

    //public virtual PaymentDetail? PaymentDetail { get; set; }

    public virtual Role? Role { get; set; }
}
