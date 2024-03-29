using WebApi.DataAccess;
using WebApi.Entities;

namespace Tests;

public class RepositoryTests
{
    private readonly OrderRepository _repository = new ();
    
    [Fact]
    public void AddsOrder()
    {
        var orders = SetupOrders();
        var order = new Order();
        _repository.Add(order);
        
        Assert.Single(orders);
    }

    private static Array SetupOrders()
    {
        var orders = FakeDb.Orders as Array;
        Array.Clear(orders);
        return orders;
    }

    [Fact]
    public void GetsOrderById()
    {
        SetupOrders();
        var order = new Order();
        _repository.Add(order);
        var id = order.Id;
        var result = _repository.GetById(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
    
    [Fact]
    public void DoesntGetsOrderById()
    {
        SetupOrders();
        var order = new Order();
        _repository.Add(order);
        var id = Guid.Empty;
        var result = _repository.GetById(id);

        Assert.Null(result);
    }

    [Fact]
    public void UpdatesOrder()
    {
        SetupOrders();
        var product = FakeDb.Products.First();
        var order = new Order();
        _repository.Add(order);
        (order.Products as List<Product>).Add(product);
        _repository.Update(order);

        Assert.Single(order.Products);
        Assert.Equal(product.Id, order.Products.First().Id);
    }
}