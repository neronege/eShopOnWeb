using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate
{
    public class Order : BaseEntity, IAggregateRoot
    {
        public Order() { }

        public Order(string buyerId, Address shipToAddress, List<OrderItem> items, string status)
        {
            Guard.Against.NullOrEmpty(buyerId, nameof(buyerId));

            BuyerId = buyerId;
            ShipToAddress = shipToAddress;
            _orderItems = items;

            Status = status;
            TotalPrice = Total();
        }


        public string Status { get; set; }
        public string BuyerId { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
        public decimal Total()
        {
            var total = 0m;
            foreach (var item in _orderItems)
            {
                total += item.UnitPrice * item.Units;
            }

            return total;
        }

        public string SetStatus()
        {
            return Status = "Approved";
        }


    }
}
