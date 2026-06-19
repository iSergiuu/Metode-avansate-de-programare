using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class ConfirmedState : IOrderState
{
    public string Name => "Confirmed";
    public void Process(Order o) => o.ChangeState(new ProcessingState());
    public void Cancel(Order o) => o.ChangeState(new CancelledState());
    public void Pay(Order o) => throw new InvalidOrderTransitionException(Name, "Pay");
    public void Ship(Order o) => throw new InvalidOrderTransitionException(Name, "Ship");
    public void Deliver(Order o) => throw new InvalidOrderTransitionException(Name, "Deliver");
}