using System;
using System.Collections.Generic;

namespace Dabbawalla.Models;

public partial class FoodItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? FoodId { get; set; }

    public int? MenuId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal? Rating { get; set; }

    public Guid? UserId { get; set; }

    public virtual FoodType? Food { get; set; }

    public virtual MenuType? Menu { get; set; }
}
