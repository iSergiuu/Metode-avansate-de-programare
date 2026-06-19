using System.Linq;
using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Validation;

public class AgeVerificationHandler : BaseValidationHandler
{
    public override ValidationResult Handle(Order order)
    {
        if (order.Items.Any(i => i.HasAgeRestriction) && order.Customer.Age < 18)
            return new ValidationResult(false, "Clientul trebuie să aibă peste 18 ani pentru acest produs.");

        return Next?.Handle(order) ?? new ValidationResult(true, null);
    }
}