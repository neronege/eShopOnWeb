using System.ComponentModel.DataAnnotations;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.PublicApi.OrderEndpoints;

public class UpdateOrderRequest : BaseRequest
{
    [Range(1, 10000)]
    public int Id { get; set; }

    /*[Required]*/
    public Address ShipToAddress { get; set; }

    [Required]
    public string Status { get; set; }
}
