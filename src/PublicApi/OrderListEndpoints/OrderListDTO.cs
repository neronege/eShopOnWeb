using System;
using System.Collections.Generic;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.PublicApi.OrderItemEndpoints;

namespace Microsoft.eShopWeb.PublicApi.OrderListEndpoints;

public class OrderListDTO : Order
{
    public int OrderID { get; private set; }
    public string Status { get; private set; }
    public string BuyerId { get; private set; }
    public DateTimeOffset OrderDate { get; private set; }
    public Address ShipToAddress { get; private set; }
    public List<OrderItemDTO> OrderItems { get; set; }
    public decimal TotalPrice { get; set; }


}
