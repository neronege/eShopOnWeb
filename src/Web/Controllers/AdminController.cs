using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.Web.Controllers;
[ApiExplorerSettings(IgnoreApi = true)]
[Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
[Route("[controller]/[action]")]
public class AdminController : Controller
{


    private readonly IOrderService _orderService;

    public AdminController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("orderlist")]
    public async Task<ActionResult<List<Order>>> GetOrdersAdmin()
    {
        var orders = await _orderService.GetOrdersAsync();
        return Ok(orders);
    }
}
