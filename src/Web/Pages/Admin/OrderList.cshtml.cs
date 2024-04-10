using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Web.Pages.Admin
{
    [Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS)]
    public class OrderList : PageModel
    {
        private readonly IOrderService _orderService;

        public OrderList(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public List<Order> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            var orders = await _orderService.GetOrdersAsync();
            Orders = orders;

            return Page();
        }

    }
}

