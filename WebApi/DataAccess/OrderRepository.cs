using WebApi.Entities;

namespace WebApi.DataAccess;

public class OrderRepository: IOrderRepository
{
    public Order? GetById(Guid id)
    {
        var orders = (List<Order>)FakeDb.Orders;
        return orders.SingleOrDefault(x => x.Id == id);
    }

    public void Add(Order order)
    {
        var orders = (List<Order>)FakeDb.Orders;
        orders?.Add(order);
    }

    public void Update(Order order)
    {
        var orders = (List<Order>)FakeDb.Orders;
        int index = orders.FindIndex(s => s.Id == order.Id);
        orders[index] = order;
    }
}