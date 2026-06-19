namespace OrderProcessing.Api.Domain;

public record Money(decimal Amount, string Currency = "RON");