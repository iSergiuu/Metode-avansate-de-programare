using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Validation;

public class PriceValidationHandler : BaseValidationHandler
{
    public override ValidationResult Handle(Order order)
    {
        if (order.Total.Amount <= 0)
            return new ValidationResult(false, "Totalul comenzii trebuie să fie pozitiv.");

        return Next?.Handle(order) ?? new ValidationResult(true, null);
    }
}