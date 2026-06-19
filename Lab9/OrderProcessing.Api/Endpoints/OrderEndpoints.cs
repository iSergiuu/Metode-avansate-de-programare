using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Api.Domain;
using OrderProcessing.Api.Services;

namespace OrderProcessing.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", ([FromBody] CreateOrderRequest req, OrderService service) =>
        {
            var customer = new Customer(Guid.NewGuid(), req.Name, req.Email, req.Age, req.IsTrusted);
            var addr = new Address(req.Street, req.City, req.PostalCode, req.Country);
            var (order, validation) = service.CreateOrder(customer, addr, req.Items);

            return validation != null ? Results.BadRequest(validation) : Results.Created($"/orders/{order!.Id.Value}", order);
        });

        app.MapPost("/orders/{id:guid}/pay", (Guid id, OrderService s) => { s.PayOrder(id); return Results.Ok(s.GetOrder(id)); });
        app.MapPost("/orders/{id:guid}/process", (Guid id, OrderService s) => { s.ProcessOrder(id); return Results.Ok(s.GetOrder(id)); });
        app.MapPost("/orders/{id:guid}/ship", (Guid id, OrderService s) => { s.ShipOrder(id); return Results.Ok(s.GetOrder(id)); });
        app.MapPost("/orders/{id:guid}/deliver", (Guid id, OrderService s) => { s.DeliverOrder(id); return Results.Ok(s.GetOrder(id)); });
        app.MapPost("/orders/{id:guid}/cancel", (Guid id, OrderService s) => { s.CancelOrder(id); return Results.Ok(s.GetOrder(id)); });

        app.MapGet("/orders", (OrderService s) => Results.Ok(s.GetAllOrders()));
        app.MapGet("/orders/{id:guid}", (Guid id, OrderService s) => s.GetOrder(id) is { } o ? Results.Ok(o) : Results.NotFound());
    }
}

public record CreateOrderRequest(string Name, string Email, int Age, bool IsTrusted, string Street, string City, string PostalCode, string Country, List<OrderItem> Items);