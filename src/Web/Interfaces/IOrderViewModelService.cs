using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Interfaces;

public interface IOrderViewModelService
{
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<List<Order>> GetAll();

    Task<OrderViewModel> UpdateOrderStatusAsync(OrderViewModel model);
}
