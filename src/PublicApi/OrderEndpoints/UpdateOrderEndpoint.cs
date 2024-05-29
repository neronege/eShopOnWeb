using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using MinimalApi.Endpoint;

namespace Microsoft.eShopWeb.PublicApi.OrderEndpoints;

/// <summary>
/// Updates a Catalog Item
/// </summary>
public class UpdateOrderEndpoint : IEndpoint<IResult, UpdateOrderRequest, IRepository<Order>>
{
    /*private readonly IUriComposer _uriComposer;*/
    /**/
    /*public UpdateOrderEndpoint(IUriComposer uriComposer)*/
    /*{*/
    /*    _uriComposer = uriComposer;*/
    /*}*/
    public UpdateOrderEndpoint() { }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        _ = app.MapPut(
                "api/orders",
                async (UpdateOrderRequest request, IRepository<Order> orderRepository) =>
                {
                    return await HandleAsync(request, orderRepository);
                }
            )
            .Produces<UpdateOrderResponse>()
            .WithTags("OrderEndpoints");
    }

    public async Task<IResult> HandleAsync(
        UpdateOrderRequest request,
        IRepository<Order> orderRepository
    )
    {
        var response = new UpdateOrderResponse(request.CorrelationId());

        var existingItem = await orderRepository.GetByIdAsync(request.Id);
        if (existingItem == null)
        {
            return Results.NotFound();
        }

        existingItem.UpdateStatus(request.Status);

        await orderRepository.UpdateAsync(existingItem);

        var dto = new OrderDto
        {
            Id = existingItem.Id,
            BuyerId = existingItem.BuyerId,
            OrderDate = existingItem.OrderDate,
            ShipToAddress = existingItem.ShipToAddress,
            Status = existingItem.Status
        };
        response.Order = dto;
        return Results.Ok(response);
    }
}
