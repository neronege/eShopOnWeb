using System;

namespace Microsoft.eShopWeb.PublicApi.OrderEndpoints;

public class ListPagedOrderRequest : BaseRequest
{
    public int PageSize { get; init; }
    public int PageIndex { get; init; }
    public DateTimeOffset? OrderDate { get; init; }
    public string? BuyerId { get; init; }

    public ListPagedOrderRequest(int? pageSize, int? pageIndex, DateTimeOffset? orderDate, string? userId)
    {
        PageSize = pageSize ?? 0;
        PageIndex = pageIndex ?? 0;
        OrderDate = orderDate;
        BuyerId = userId;
    }
}
