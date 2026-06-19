using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class ShippedState : IOrderState
{
    public string Name => "Shipped";
    public void Deliver(Order o) => o.ChangeState(new DeliveredState());
    public void Cancel(Order o) => throw new InvalidOrderTransitionException(Name, "Cancel");
    public void Pay(Order o) => throw new InvalidOrderTransitionException(Name, "Pay");
    public void Process(Order o) => throw new InvalidOrderTransitionException(Name, "Process");
    public void Ship(Order o) => throw new InvalidOrderTransitionException(Name, "Ship");
}