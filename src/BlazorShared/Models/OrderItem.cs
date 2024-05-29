using System;

namespace BlazorShared.Models;

/// <summary>
/// OrderItem model for items in orders
/// </summary>
public class OrderItem
{
    public int id { get; set; }
    public ItemOrdered ItemOrdered { get; set; }
    public decimal UnitPrice { get; set; }
    public int units { get; set; }
}

public class ItemOrdered
{
    public int CatalogItemId { get; set; }
    public string ProductName { get; set; }
    public string PictureUri { get; set; }
}
