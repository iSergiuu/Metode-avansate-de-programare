using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderProcessing.Api.Domain;

public record OrderHistoryEntry(string FromState, string ToState, DateTime At);

public class Order
{
    public OrderId Id { get; }
    public Customer Customer { get; }
    public Address ShippingAddress { get; }
    public List<OrderItem> Items { get; }
    public Money Total { get; }
    public List<OrderHistoryEntry> History { get; } = new();

    internal OrderProcessing.Api.States.IOrderState CurrentState { get; private set; } = new OrderProcessing.Api.States.PendingState();
    public string Status => CurrentState.Name;

    public Order(Customer customer, Address shippingAddress, List<OrderItem> items)
    {
        Id = new OrderId(Guid.NewGuid());
        Customer = customer;
        ShippingAddress = shippingAddress;
        Items = items;

        var totalAmount = items.Sum(i => i.Quantity * i.UnitPrice);
        Total = new Money(totalAmount);
    }

    internal void ChangeState(OrderProcessing.Api.States.IOrderState newState)
    {
        History.Add(new OrderHistoryEntry(CurrentState.Name, newState.Name, DateTime.UtcNow));
        CurrentState = newState;
    }

    public void Pay() => CurrentState.Pay(this);
    public void Process() => CurrentState.Process(this);
    public void Ship() => CurrentState.Ship(this);
    public void Deliver() => CurrentState.Deliver(this);
    public void Cancel() => CurrentState.Cancel(this);
}