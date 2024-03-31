using System.Text.Json.Serialization;

namespace WebApi.Entities;
public class Order
{
    public Amount Amount { get; set; } = new Amount();

    public Guid Id { get; private set; } = Guid.NewGuid();

    public IEnumerable<Product> Products { get; set; } = new List<Product>();

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status Status { get; set; } = Status.NEW;
}