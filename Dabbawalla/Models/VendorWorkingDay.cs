using System;
using System.Collections.Generic;

namespace Dabbawalla.Models;

public partial class VendorWorkingDay
{
    public int Id { get; set; }

    public int? WorkingDayId { get; set; }

    public string? UserId { get; set; }
}
