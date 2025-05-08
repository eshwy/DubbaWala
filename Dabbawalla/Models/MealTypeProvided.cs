using System;
using System.Collections.Generic;

namespace Dabbawalla.Models;

public partial class MealTypeProvided
{
    public int Id { get; set; }

    public int? FoodId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? UserId { get; set; }
}
