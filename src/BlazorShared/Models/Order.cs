using System;
using System.Collections.Generic;

namespace BlazorShared.Models;

/// <summary>
/// Order model
/// </summary>
public class Order
{
    public int id { get; set; }
    public string buyerId { get; set; }
    public DateTime orderDate { get; set; }
    public IReadOnlyCollection<OrderItem> orderItems { get; set; }
    public string status { get; set; }
    public decimal total { get; set; }
}
