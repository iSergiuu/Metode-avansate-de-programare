namespace OrderProcessing.Api.Validation;

public record ValidationResult(bool IsValid, string? Error);