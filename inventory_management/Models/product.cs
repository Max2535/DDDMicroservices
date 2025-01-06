using System;
using System.Collections.Generic;

namespace inventory_management.Models;

public partial class product
{
    public int id { get; set; }

    public string sku { get; set; } = null!;

    public string name { get; set; } = null!;

    public decimal price { get; set; }

    public DateTime? created_at { get; set; }

    public virtual ICollection<stock_balance> stock_balances { get; set; } = new List<stock_balance>();

    public virtual ICollection<stock_movement> stock_movements { get; set; } = new List<stock_movement>();
}
