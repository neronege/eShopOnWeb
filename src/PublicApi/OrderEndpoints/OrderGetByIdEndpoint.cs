using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using MinimalApi.Endpoint;

namespace Microsoft.eShopWeb.PublicApi.OrderEndpoints;

/// <summary>
/// Get a Order by Id
/// </summary>
public class OrderGetByIdEndpoint : IEndpoint<IResult, GetByIdOrderRequest, IRepository<Order>>
{
    public OrderGetByIdEndpoint() { }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "api/orders/{orderId}",
                async (int orderId, IRepository<Order> orderRepository) =>
                {
                    return await HandleAsync(new GetByIdOrderRequest(orderId), orderRepository);
                }
            )
            .Produces<GetByIdOrderResponse>()
            .WithTags("OrderEndpoints");
    }

    public async Task<IResult> HandleAsync(
        GetByIdOrderRequest request,
        IRepository<Order> orderRepository
    )
    {
        var response = new GetByIdOrderResponse(request.CorrelationId());

        // get by spec with OrderItems instead of just getting the order by ID
        var spec = new OrderWithItemsByIdSpec(request.OrderId);
        var item = await orderRepository.FirstOrDefaultAsync(spec);
        if (item is null)
            return Results.NotFound();

        response.Order = new OrderDto
        {
            Id = item.Id,
            BuyerId = item.BuyerId,
            OrderDate = item.OrderDate,
            ShipToAddress = item.ShipToAddress,
            OrderItems = item.OrderItems,
            Total = item.Total(),
            Status = item.Status
        };
        return Results.Ok(response);
    }
}
