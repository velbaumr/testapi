using WebApi.Entities;

namespace WebApi
{
    public static class AmountCalculator
    {
        public static Order CalculateTotal(Order order)
        {
            var total = order.Products.Sum(x => x.Quantity * x.Price);
            order.Amount.Total = total;

            return order;
        }

        public static Order CalculateReplacementAmounts(Order order) 
        {
            var total = CalculateTotal(order).Amount.Total;

            var replacementTotals = order.Products
                .Where(x => x.Replaced_with != null)
                .Select(x => x.Replaced_with)
                .Sum(x => x!.Quantity * x.Price);

            order.Amount.Return = total > replacementTotals ? total - replacementTotals: 0;
            order.Amount.Discount = total < replacementTotals ? replacementTotals - total : 0;

            return order;
        }
    }
}
