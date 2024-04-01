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

            var selfReplacements = order.Products
                .Where(x => x.Replaced_with != null &&
                            (x.Product_id == x.Replaced_with.Product_id && x.Quantity > x.Replaced_with.Quantity))
                .Select(x => x.Replaced_with)
                .Sum(x => x!.Quantity * x.Price);

            var notReplaced = order.Products
                .Where(x => x.Replaced_with == null)
                .Sum(x => x.Quantity * x.Price);
            
            order.Amount.Paid = total;
            order.Amount.Return = total > replacementTotals - selfReplacements + notReplaced
                ? total - replacementTotals - selfReplacements - notReplaced
                : 0;
            order.Amount.Discount = total < replacementTotals - selfReplacements + notReplaced
                ? replacementTotals - selfReplacements - total + notReplaced
                : 0;
            order.Amount.Total = order.Amount.Return > 0 ? total - order.Amount.Return : total;

            return order;
        }
    }
}