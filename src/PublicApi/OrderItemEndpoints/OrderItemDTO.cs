using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.PublicApi.OrderItemEndpoints;

public class OrderItemDTO : OrderItem
{
    public decimal UnitPrice { get; set; }
    public int Units { get; set; }

    public int Id { get; set; }
}
