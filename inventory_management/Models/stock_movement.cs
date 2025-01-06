using System;
using System.Collections.Generic;

namespace inventory_management.Models;

public partial class stock_movement
{
    public int id { get; set; }

    public int product_id { get; set; }

    public string movement_type { get; set; } = null!;

    public int quantity { get; set; }

    public DateTime? created_at { get; set; }

    public virtual product product { get; set; } = null!;
}
