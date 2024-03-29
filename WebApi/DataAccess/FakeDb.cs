using WebApi.Entities;

namespace WebApi.DataAccess;

public static class FakeDb
{
    public static IEnumerable<Product> Products => new[]
    {
        new Product
        {
            Id = 123,
            Name = "Ketchup",
            Price = 0.45M
        },
        new Product
        {
            Id = 456,
            Name = "Beer",
            Price = 2.33M
        },
        new Product
        {
            Id = 879,
            Name = "Õllesnäkk",
            Price = 0.42M
        },
        new Product
        {
            Id = 999,
            Name = "75'' OLED TV",
            Price = 1333.37M
        }
    };

    public static IEnumerable<Order> Orders { get; set; } = new List<Order>();
}