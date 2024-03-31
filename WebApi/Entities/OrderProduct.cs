namespace WebApi.Entities
{
    public class OrderProduct
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Product_id { get; set; }

        public int Quantity { get; set; }

        public OrderProduct? Replaced_with { get; set; }
    }
}
