namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

public class OrderItem : BaseEntity
{
    public CatalogItemOrdered ItemOrdered { get; set; }
    public decimal UnitPrice { get; set; }
    public int Units { get; set; }

#pragma warning disable CS8618 // Required by Entity Framework
    public OrderItem() { }

    public OrderItem(CatalogItemOrdered itemOrdered, decimal unitPrice, int units)
    {
        ItemOrdered = itemOrdered;
        UnitPrice = unitPrice;
        Units = units;
    }
}
