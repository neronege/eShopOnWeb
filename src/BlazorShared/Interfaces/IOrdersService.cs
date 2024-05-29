using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorShared.Models;

namespace BlazorShared.Interfaces;

public interface IOrdersService
{
    Task<Order> Edit(Order orders);
    Task<Order> GetById(int id);
    Task<Order> Approve(int id);
    Task<List<Order>> ListPaged(int pageSize);
    Task<List<Order>> List();
}
