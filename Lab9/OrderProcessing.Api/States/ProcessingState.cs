using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class ProcessingState : IOrderState
{
    public string Name => "Processing";
    public void Ship(Order o) => o.ChangeState(new ShippedState());
    public void Cancel(Order o) => o.ChangeState(new CancelledState());
    public void Pay(Order o) => throw new InvalidOrderTransitionException(Name, "Pay");
    public void Process(Order o) => throw new InvalidOrderTransitionException(Name, "Process");
    public void Deliver(Order o) => throw new InvalidOrderTransitionException(Name, "Deliver");
}