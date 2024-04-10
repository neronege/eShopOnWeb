using System;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.PublicApi.OrderListEndpoints;

public class OrderResponse : BaseResponse
{
    public OrderResponse(Guid correlationId) : base(correlationId)
    {
    }

    public OrderResponse()
    {
    }

    public List<OrderListDTO> _orderListDTO { get; set; } = new List<OrderListDTO>();
}
