using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using MinimalApi.Endpoint;

namespace Microsoft.eShopWeb.PublicApi.OrderListEndpoints;

public class OrderListEndpoint : IEndpoint<IResult, IRepository<Order>>
{
    private readonly IMapper _mapper;

    public OrderListEndpoint(IMapper mapper)
    {
        _mapper = mapper;
    }
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("api/admin/orderlist",
             async (IRepository<Order> orderListRepository) =>
             {
                 return await HandleAsync(orderListRepository);
             })
            .Produces<OrderResponse>()
            .WithTags("admin/orderlist");
    }

    public async Task<IResult> HandleAsync(IRepository<Order> orderListRepository)
    {
        var response = new OrderResponse();

        var items = await orderListRepository.ListAsync();

        response._orderListDTO.AddRange(items.Select(_mapper.Map<OrderListDTO>));

        return Results.Ok(response);
    }
}
