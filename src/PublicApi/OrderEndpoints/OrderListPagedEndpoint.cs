using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using MinimalApi.Endpoint;

namespace Microsoft.eShopWeb.PublicApi.OrderEndpoints;

/// <summary>
/// List Orders (paged)
/// </summary>
public class OrderListPagedEndpoint : IEndpoint<IResult, ListPagedOrderRequest, IRepository<Order>>
{
    private readonly IUriComposer _uriComposer;
    private readonly IMapper _mapper;

    public OrderListPagedEndpoint(IUriComposer uriComposer, IMapper mapper)
    {
        _uriComposer = uriComposer;
        _mapper = mapper;
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "api/orders",
                async (
                    int? pageSize,
                    int? pageIndex,
                    DateTimeOffset? orderDate,
                    string? BuyerId,
                    IRepository<Order> itemRepository
                ) =>
                {
                    return await HandleAsync(
                        new ListPagedOrderRequest(pageSize, pageIndex, orderDate, BuyerId),
                        itemRepository
                    );
                }
            )
            .Produces<ListPagedOrderResponse>()
            .WithTags("OrderEndpoints");
    }

    public async Task<IResult> HandleAsync(
        ListPagedOrderRequest request,
        IRepository<Order> orderRepository
    )
    {
        /*await Task.Delay(1000);*/
        var response = new ListPagedOrderResponse(request.CorrelationId());

        var filterSpec = new OrderFilterSpecification(request.OrderDate);
        int totalItems = await orderRepository.CountAsync(filterSpec);

        // TODO: exclude OrderItems in List of order (OrderDto), since it is unnecessary detail in the list
        // But needed for Total price information for each order
        var pagedSpec = new OrderFilterPaginatedSpecification(
            skip: request.PageIndex * request.PageSize,
            take: request.PageSize,
            orderDate: request.OrderDate
        );

        var items = await orderRepository.ListAsync(pagedSpec);

        response.Orders.AddRange(items.Select(_mapper.Map<OrderDto>));

        if (request.PageSize > 0)
        {
            response.PageCount = int.Parse(
                Math.Ceiling((decimal)totalItems / request.PageSize).ToString()
            );
        }
        else
        {
            response.PageCount = totalItems > 0 ? 1 : 0;
        }

        return Results.Ok(response);
    }
}
