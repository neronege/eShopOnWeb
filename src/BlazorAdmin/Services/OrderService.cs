using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.Services;

public class OrdersService : IOrdersService
{
    private readonly HttpService _httpService;
    private readonly ILogger<OrdersService> _logger;
    private const string ORDERS_ENDPOINT = "orders";

    public OrdersService(HttpService httpService, ILogger<OrdersService> logger)
    {
        _httpService = httpService;
        _logger = logger;
    }

    public async Task<Order> Edit(Order orders)
    {
        return (await _httpService.HttpPut<EditOrdersResult>(ORDERS_ENDPOINT, orders)).Order;
    }

    // NEVER delete Order
    /*public async Task<string> Delete(int catalogItemId)*/
    /*{*/
    /*    return (*/
    /*        await _httpService.HttpDelete<DeleteOrdersResponse>("catalog-Order", catalogItemId)*/
    /*    ).Status;*/
    /*}*/

    public async Task<Order> GetById(int id)
    {
        _logger.LogInformation($"Fetching Order [{id}] from API.");
        var ordersGetTask = _httpService.HttpGet<EditOrdersResult>($"{ORDERS_ENDPOINT}/{id}");
        var order = (await ordersGetTask).Order;

        _logger.LogInformation($"Fetched Order [{order.id}] from API.");
        return order;
    }

    public async Task<List<Order>> ListPaged(int pageSize)
    {
        _logger.LogInformation("Fetching catalog Order from API.");

        var orderListTask = _httpService.HttpGet<PagedOrdersResponse>(
            $"{ORDERS_ENDPOINT}?PageSize={pageSize}"
        );
        var orders = (await orderListTask).Orders;
        return orders;
    }

    public async Task<Order> Approve(int id)
    {
        Order? order = await GetById(id);
        if (order is null)
            return null;

        order.status = "Approved";

        return (await _httpService.HttpPut<EditOrdersResult>(ORDERS_ENDPOINT, order)).Order;
    }

    public async Task<List<Order>> List()
    {
        _logger.LogInformation("Fetching Order from API.");

        var orderListTask = _httpService.HttpGet<PagedOrdersResponse>(ORDERS_ENDPOINT);
        var orders = (await orderListTask).Orders;

        _logger.LogInformation($"Fetching Order [{orders.Count}] from API.");
        return orders;
    }
}
