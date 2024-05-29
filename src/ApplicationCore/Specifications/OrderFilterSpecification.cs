using System;
using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public class OrderFilterSpecification : Specification<Order>
{
    public OrderFilterSpecification(DateTimeOffset? orderDate)
    {
        Query.Where(i => (!orderDate.HasValue || i.OrderDate == orderDate));
    }
}
