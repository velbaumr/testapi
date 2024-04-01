using WebApi.Entities;

namespace WebApi.Dtos
{
    public class OrderProductDto
    {
        public int? Quantity { get; set; }

        public ReplacementProduct? ReplacementProduct { get; set; }
    }

    public class ReplacementProduct
    {
        public int Product_id { get; set; }

        public int Quantity { get; set; }
    }
}
