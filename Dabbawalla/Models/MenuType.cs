using System;
using System.Collections.Generic;

namespace Dabbawalla.Models;

public partial class MenuType
{
    public int MenuId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
}
