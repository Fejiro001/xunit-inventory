namespace InventorySystem.Tests
{
    public class InventoryTestSuites
    {
        private readonly InventoryOrderService _orderService = new();

        // === Happy Path ===
        [Fact]
        public void AddProduct_ValidProduct_SuccessfullySavesToInventory()
        {
            // Arrange
            Product product = new Product
            {
                Id = "P500",
                Name = "Sony Headphones",
                UnitPrice = 350.00m,
                StockQuantity = 10
            };
            // Act
            _orderService.AddProduct(product);

            // Assert
            var productExists = _orderService.GetProduct(product.Id);
            Assert.NotNull(productExists);
            Assert.Equal("P500", productExists.Id);
        }

        [Fact]
        public void GetProduct_WithExistingId_ReturnsCorrectProduct()
        {
            // Arrange
            Product product = new Product
            {
                Id = "P120",
                Name = "Bluetooth Speakers",
                UnitPrice = 1050.00m,
                StockQuantity = 12
            };
            _orderService.AddProduct(product);

            // Act
            var exists = _orderService.GetProduct(product.Id);

            // Assert
            Assert.NotNull(exists);
            Assert.Equal("P120", exists.Id);
        }

        [Fact]
        public void ProcessOrder_ValidQuantityWithTax_CalculatesTotalAndDeductsStock()
        {
            // Arrange
            Product product = new Product
            {
                Id = "P100",
                Name = "Sony Headphones",
                UnitPrice = 350.00m,
                StockQuantity = 10
            };
            _orderService.AddProduct(product);
            int quantity = 5;
            decimal taxRate = 0.05m;

            // Act
            OrderResult order = _orderService.ProcessOrder(product.Id, quantity, taxRate);

            // Assert
            Assert.True(order.IsSuccess);
            Assert.Equal(1837.50m, order.TotalCost);
            Assert.Equal(5, product.StockQuantity);
        }

        // === Edge Cases / Boundaries ===
        [Fact]
        public void ProcessOrder_QuantityIsZero_ReturnsSuccessWithZeroCost()
        {
            // Arrange
            Product product = new Product
            {
                Id = "P312",
                Name = "Logitech Mouse",
                UnitPrice = 250.00m,
                StockQuantity = 15
            };
            _orderService.AddProduct(product);
            int quantity = 0;
            decimal taxRate = 0.0m;

            // Act
            OrderResult order = _orderService.ProcessOrder(product.Id, quantity, taxRate);

            // Assert
            Assert.True(order.IsSuccess);
            Assert.Equal(0.00m, order.TotalCost);
            Assert.Equal(15, product.StockQuantity);
        }
    }
}
