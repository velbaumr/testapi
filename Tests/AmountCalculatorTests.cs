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
        
        private readonly Order _selfReplacementToTest = new()
        {
            Products = new List<OrderProduct>
            {
                new()
                {
                    Quantity = 2,
                    Price = 0.45M,
                    Replaced_with = new OrderProduct()
                    {
                        Quantity = 8,
                        Price = 0.45M
                    }
                }
      
            },
        };
        
        private readonly Order _mixedReplacementToTest = new()
        {
            Products = new List<OrderProduct>
            {
                new()
                {
                    Quantity = 3,
                    Price = 0.45M,
                },
                new()
                {
                    Quantity = 3,
                    Price = 1333.37M,
                    Replaced_with = new OrderProduct()
                    {
                        Quantity = 30,
                        Price = 0.45M
                    }
                },
                new()
                {
                    Quantity = 3,
                    Price = 0.42M,
                    Replaced_with = new OrderProduct()
                    {
                        Quantity = 20,
                        Price = 0.42M
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
        
        [Fact]
        public void CalculatesSelfReplacedAmounts()
        {
            var order = AmountCalculator.CalculateReplacementAmounts(_selfReplacementToTest);
            
            Assert.Equal(0.90M, order.Amount.Total);
            Assert.Equal(2.70M, order.Amount.Discount);
            Assert.Equal(0.00M, order.Amount.Return);
        }
        
        [Fact]
        public void CalculatesMixedReplacedAmounts()
        {
            var order = AmountCalculator.CalculateReplacementAmounts(_mixedReplacementToTest);
            
            Assert.Equal(23.25M, order.Amount.Total);
            Assert.Equal(0.00M, order.Amount.Discount);
            Assert.Equal(3979.47M, order.Amount.Return);
        }
    }
}
