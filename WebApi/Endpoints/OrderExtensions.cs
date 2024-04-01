using Microsoft.AspNetCore.Mvc;
using WebApi.DataAccess;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi.Endpoints;

public static class OrderExtensions
{
    public static void MapOrders(this WebApplication app)
    {
        app.MapGet("/api/products", () => FakeDb.Products.OrderBy(x => x.Id));

        app.MapPost("/api/orders", (IOrderRepository repository) =>
        {
            var order = new Order();
            repository.Add(order);

            return Results.Ok(order);
        });

        app.MapGet("/api/orders/{orderId}", (IOrderRepository repository, Guid orderId) =>
        {
            var order = repository.GetById(orderId);
            return order == null ? Results.NotFound("Not found") : Results.Ok(order);
        });

        app.MapGet("/api/orders/{orderId}/products", (IOrderRepository repository, Guid orderId) =>
        {
            var order = repository.GetById(orderId);
            return order == null ? Results.NotFound("Not found") : Results.Ok(order.Products);
        });

        app.MapPost("/api/orders/{orderId}/products",
            (IOrderRepository repository, Guid orderId, [FromBody] IEnumerable<int> data) =>
                AddOrderProducts(repository, orderId, data));

        app.MapPatch("/api/orders/{orderId}/products/{productId}", (IOrderRepository repository, Guid orderId,
            Guid productId, [FromBody] OrderProductDto data) =>
        {
            var order = repository.GetById(orderId);
            var product = order?.Products.SingleOrDefault(x => x.Id == productId);

            if (order == null || product == null)
            {
                return Results.NotFound("Not found");
            }

            if (data.Quantity == null && data.ReplacementProduct == null)
            {
                return Results.BadRequest("Invalid parameters");
            }

            return data.Quantity.HasValue ? ChangeProductQuantity(product, data, order) : ReplaceProduct(order, data, product);
        });

        app.MapPatch("/api/orders/{orderId}",
            (IOrderRepository repository, Guid orderId, [FromBody] OrderDto data) =>
                ChangeOrderStatus(repository, orderId, data));
    }

    private static IResult ChangeProductQuantity(OrderProduct product, OrderProductDto data, Order order)
    {
        if (order.Status != Status.NEW)
        {
            return Results.BadRequest("Invalid parameters");
        }
        product.Quantity = data.Quantity!.Value;
        AmountCalculator.CalculateTotal(order);
        return Results.Ok("OK");
    }

    private static IResult AddOrderProducts(IOrderRepository repository, Guid orderId, IEnumerable<int> data)
    {
        var order = repository.GetById(orderId);

        if (order == null)
        {
            return Results.NotFound("Not found");
        }

        if (((List<int>)data).GroupBy(x => x).Any(g => g.Count() > 1) || ((List<int>)data).Except(FakeDb.Products.Select(x => x.Id)).Any())
        {
            return Results.BadRequest("Invalid parameters");
        }

        AddProductsToOrder(data, order);
        repository.Update(AmountCalculator.CalculateTotal(order));

        return Results.Ok("OK");
    }

    private static IResult ReplaceProduct(Order order, OrderProductDto data, OrderProduct product)
    {
        if (order.Status != Status.PAID)
        {
            return Results.BadRequest("Invalid parameters");
        }

        var replacement = FakeDb.Products.SingleOrDefault(x => x.Id == data.ReplacementProduct!.Product_id);
        if (replacement == null)
        {
            return Results.NotFound("Not found");
        }

        product.Replaced_with = new OrderProduct
        {
            Id = Guid.NewGuid(),
            Name = replacement.Name,
            Price = replacement.Price,
            Product_id = replacement.Id,
            Quantity = data.ReplacementProduct!.Quantity
        };

        AmountCalculator.CalculateReplacementAmounts(order);
        return Results.Ok("OK");
    }

    private static IResult ChangeOrderStatus(IOrderRepository repository, Guid orderId, OrderDto data)
    {
        
        var order = repository.GetById(orderId);

        if (order == null)
        {
            return Results.NotFound("Not found");
        }

        if (!Enum.TryParse<Status>(data.Status, out var status)) return Results.BadRequest("Invalid order status");
        if (status != Status.PAID)
        {
            return Results.BadRequest("Invalid order status");
        }
        order.Status = status;
        order.Amount.Paid = order.Amount.Total;

        repository.Update(order);

        return Results.Ok("OK");
    }

    private static void AddProductsToOrder(IEnumerable<int> data, Order order)
    {
        foreach (var item in data)
        {
            AddProduct(order, item);
        }
    }

    private static void AddProduct(Order order, int item)
    {
        var product = order.Products.SingleOrDefault(x => x.Product_id == item);
        if (product != null)
        {
            product.Quantity++;
        }
        else
        {
            var toAdd = FakeDb.Products.Single(x => x.Id == item);
            ((List<OrderProduct>)order.Products).Add(new OrderProduct
            {
                Id = Guid.NewGuid(),
                Name = toAdd.Name,
                Price = toAdd.Price,
                Product_id = toAdd.Id,
                Quantity = 1
            });
        }
    }
}