using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.Api.Endpoints;
using OrderProcessing.Api.Services;
using OrderProcessing.Api.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var validationChain = new StockValidationHandler();
validationChain
    .SetNext(new PriceValidationHandler())
    .SetNext(new FraudDetectionHandler())
    .SetNext(new AgeVerificationHandler());

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<IOrderValidationHandler>(validationChain);
builder.Services.AddSingleton<OrderService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.MapOrderEndpoints();

app.Run();