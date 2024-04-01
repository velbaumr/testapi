using WebApi;
using WebApi.Entities;

namespace Tests
{
    public class AmountCalculatorTests
    {
        private readonly Order _orderToTest = new()
        {
            Products = new List<OrderProduct>
                {
                new()
                {
                    Quantity = 33,
                    Price = 2.33M
                },
                new()
                {
                    Quantity = 2,
                    Price = 0.45M
                }
            },
        };
        
        private readonly Order _replacementToTest = new()
        {
            Products = new List<OrderProduct>
            {
                new()
                {
                    Quantity = 2,
                    Price = 0.45M,
                    Replaced_with = new OrderProduct()
                    {
                        Quantity = 6,
                        Price = 2.33M
                    }
                }
      
            },
        };

        
        [Fact]
        public void CalculatesAmounts()
        {
            var order = AmountCalculator.CalculateTotal(_orderToTest);

            Assert.Equal(77.79M, order.Amount.Total);
        }

        [Fact]
        public void CalculatesReplacedAmounts()
        {
            var order = AmountCalculator.CalculateReplacementAmounts(_replacementToTest);
            
            Assert.Equal(0.90M, order.Amount.Total);
            Assert.Equal(13.08M, order.Amount.Discount);
            Assert.Equal(0.00M, order.Amount.Return);
        }
    }
}
