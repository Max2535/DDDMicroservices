using System;
using System.Collections.Generic;

namespace inventory_management.Models;

public partial class stock_balance
{
    public int id { get; set; }

    public int product_id { get; set; }

    public int quantity { get; set; }

    public DateTime? updated_at { get; set; }

    public virtual product product { get; set; } = null!;
}
