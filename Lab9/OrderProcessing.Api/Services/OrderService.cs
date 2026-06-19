using System;
using System.Collections.Generic;
using OrderProcessing.Api.Domain;
using OrderProcessing.Api.Validation;

namespace OrderProcessing.Api.Services;

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IOrderValidationHandler _validationChain;

    public OrderService(IOrderRepository repository, IOrderValidationHandler validationChain)
    {
        _repository = repository;
        _validationChain = validationChain;
    }

    public (Order? Order, ValidationResult? Validation) CreateOrder(Customer customer, Address addr, List<OrderItem> items)
    {
        var order = new Order(customer, addr, items);
        var validation = _validationChain.Handle(order);

        if (!validation.IsValid) return (null, validation);

        _repository.Save(order);
        return (order, null);
    }

    public Order? GetOrder(Guid id) => _repository.GetById(id);
    public IEnumerable<Order> GetAllOrders() => _repository.GetAll();

    public void PayOrder(Guid id)
    {
        var o = GetOrder(id);
        if (o != null) { o.Pay(); _repository.Save(o); }
    }

    public void ProcessOrder(Guid id)
    {
        var o = GetOrder(id);
        if (o != null) { o.Process(); _repository.Save(o); }
    }

    public void ShipOrder(Guid id)
    {
        var o = GetOrder(id);
        if (o != null) { o.Ship(); _repository.Save(o); }
    }

    public void DeliverOrder(Guid id)
    {
        var o = GetOrder(id);
        if (o != null) { o.Deliver(); _repository.Save(o); }
    }

    public void CancelOrder(Guid id)
    {
        var o = GetOrder(id);
        if (o != null) { o.Cancel(); _repository.Save(o); }
    }
}