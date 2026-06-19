using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Validation;

public abstract class BaseValidationHandler : IOrderValidationHandler
{
    protected IOrderValidationHandler? Next;

    public IOrderValidationHandler SetNext(IOrderValidationHandler next)
    {
        Next = next;
        return next;
    }

    public abstract ValidationResult Handle(Order order);
}