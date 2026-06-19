using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Validation;

public class FraudDetectionHandler : BaseValidationHandler
{
    public override ValidationResult Handle(Order order)
    {
        if (order.Total.Amount > 10000 && !order.Customer.IsTrusted)
            return new ValidationResult(false, "Comandă suspectă — necesită verificare manuală.");

        return Next?.Handle(order) ?? new ValidationResult(true, null);
    }
}