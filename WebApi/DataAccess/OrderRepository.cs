using WebApi.Entities;

namespace WebApi.DataAccess;

public class OrderRepository: IOrderRepository
{
    public Order GetById(Guid id)
    {
        var orders = FakeDb.Orders as List<Order>;
        return orders.SingleOrDefault(x => x.Id == id);
    }

    public void Add(Order order)
    {
        var orders = FakeDb.Orders as List<Order>;
        orders?.Add(order);
    }

    public void Update(Order order)
    {
        var orders = FakeDb.Orders as List<Order>;
        
    }
}