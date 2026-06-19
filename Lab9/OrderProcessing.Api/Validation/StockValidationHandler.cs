using System.Linq;
using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Validation;

public class StockValidationHandler : BaseValidationHandler
{
    public override ValidationResult Handle(Order order)
    {
        if (order.Items.Any(i => i.Quantity > 10))
            return new ValidationResult(false, "Stoc insuficient pentru unul sau mai multe produse.");

        return Next?.Handle(order) ?? new ValidationResult(true, null);
    }
}