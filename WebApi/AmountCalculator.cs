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
                .Sum(x => x.Replaced_with!.Quantity * x.Replaced_with!.Price);
            
            var normalTotals = order.Products
                .Where(x => x.Replaced_with == null)
                .Sum(x => x.Quantity * x.Price);
            
            
            order.Amount.Paid = total;
            order.Amount.Return = total > replacementTotals + normalTotals
                ? total - (replacementTotals + normalTotals)
                : 0;
            order.Amount.Discount = total < replacementTotals + normalTotals
                ? replacementTotals + normalTotals - total
                : 0;
            order.Amount.Total = order.Amount.Return > 0 ? total - order.Amount.Return : total;

            return order;
        }
    }
}