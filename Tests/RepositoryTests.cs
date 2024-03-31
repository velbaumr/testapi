using WebApi.DataAccess;
using WebApi.Entities;

namespace Tests;

public class RepositoryTests
{
    private readonly OrderRepository _repository = new ();
    
     private static void SetupOrders()
    {
        var orders = (List<Order>)FakeDb.Orders;
        orders.Clear();
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
        var orderProduct = new OrderProduct
        {
            Product_id = product.Id
        };
        var order = new Order();
        _repository.Add(order);
        ((List<OrderProduct>)order.Products).Add(orderProduct);
        _repository.Update(order);

        Assert.Single(order.Products);
        Assert.Equal(product.Id, order.Products.First().Product_id);
    }

    [Fact]
    public void AddsOrder()
    {
        SetupOrders();
        var order = new Order();
        _repository.Add(order);
        var result = FakeDb.Orders.FirstOrDefault();
        
        Assert.NotNull(result);
    }
}