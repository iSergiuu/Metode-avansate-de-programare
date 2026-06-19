using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Validation;

public interface IOrderValidationHandler
{
    IOrderValidationHandler SetNext(IOrderValidationHandler next);
    ValidationResult Handle(Order order);
}