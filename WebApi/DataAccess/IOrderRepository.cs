using WebApi.Entities;

namespace WebApi.DataAccess;

public interface IOrderRepository
{
    Order GetById(Guid id);
    void Add(Order order);
    void Update(Order order);
}