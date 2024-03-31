using Microsoft.AspNetCore.Mvc;
using WebApi.DataAccess;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi.Endpoints;

public static class OrderExtensions
{
    public static void MapOrders(this WebApplication app)
    {
        app.MapGet("/api/products", () => FakeDb.Products);
        app.MapPost("/api/orders", (IOrderRepository repository) =>
        {
            var order = new Order();
            repository.Add(order);
        });
        app.MapGet("/api/orders/{orderId}", (IOrderRepository repository, Guid orderId) =>
        {
            var order = repository.GetById(orderId);
            if (order == null)
            {
                return Results.NotFound("Not found");
            }

            return Results.Ok(order);
        });
        app.MapGet("/api/orders/{orderId}/products", (IOrderRepository repository, Guid orderId) =>
        {
            var order = repository.GetById(orderId);
            if (order == null)
            {
                return Results.NotFound("Not found");
            }

            return Results.Ok(order.Products);
        });
        app.MapPost("/api/orders/{orderId}/products", (IOrderRepository repository, Guid orderId, [FromBody] IEnumerable<int> data) =>
        {
        });
        app.MapPatch("/api/orders/{orderId}/products/{productId}", (IOrderRepository repository, Guid orderId, Guid productId, [FromBody] OrderProductDto data) =>
        {
            return Results.NotFound("Not found");
        });
        app.MapPatch("/api/orders/{orderId}", (IOrderRepository repository, Guid orderId, [FromBody] OrderDto data) =>
        {
            
        });
    }
}