using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Services;

public interface IOrderRepository
{
    void Save(Order order);
    Order? GetById(Guid id);
    IEnumerable<Order> GetAll();
}

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public void Save(Order order)
    {
        _orders[order.Id.Value] = order;
    }

    public Order? GetById(Guid id)
    {
        return _orders.TryGetValue(id, out var order) ? order : null;
    }

    public IEnumerable<Order> GetAll()
    {
        return _orders.Values;
    }
}