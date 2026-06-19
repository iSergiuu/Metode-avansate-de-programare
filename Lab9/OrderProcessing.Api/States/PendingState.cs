using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class PendingState : IOrderState
{
    public string Name => "Pending";
    public void Pay(Order o) => o.ChangeState(new ConfirmedState());
    public void Cancel(Order o) => o.ChangeState(new CancelledState());
    public void Process(Order o) => throw new InvalidOrderTransitionException(Name, "Process");
    public void Ship(Order o) => throw new InvalidOrderTransitionException(Name, "Ship");
    public void Deliver(Order o) => throw new InvalidOrderTransitionException(Name, "Deliver");
}