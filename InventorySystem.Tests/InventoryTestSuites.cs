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

        [Fact]
        public void ProcessOrder_WithDiscountBoundary10Items_AppliesTenPercentDiscount()
        {
            // Arrange
            Product product = new Product
            {
                Id = "P120",
                Name = "Ninja Air Fryer",
                UnitPrice = 500.00m,
                StockQuantity = 100
            };
            _orderService.AddProduct(product);
            int quantity = 10;
            decimal taxRate = 0.0m;

            // Act
            OrderResult order = _orderService.ProcessOrder(product.Id, quantity, taxRate);

            // Assert
            Assert.True(order.IsSuccess);
            Assert.Equal(4500.00m, order.TotalCost);
        }

        [Fact]
        public void AddProduct_DuplicateProductId_DoesNotOverwriteExistingProduct()
        {
            // Arrange
            Product productOne = new Product
            {
                Id = "P150",
                Name = "Standing Desk",
                UnitPrice = 1200.00m,
                StockQuantity = 25
            };
            Product productTwo = new Product
            {
                Id = "P150",
                Name = "Samsung Watch",
                UnitPrice = 500.00m,
                StockQuantity = 50
            };

            // Act
            _orderService.AddProduct(productOne);
            _orderService.AddProduct(productTwo);

            // Assert
            var existingProduct = _orderService.GetProduct("P150");
            Assert.Equal("Standing Desk", existingProduct.Name);
        }

        // === Exception Handling ===
        [Fact]
        public void AddProduct_NullProduct_ThrowsArgumentException()
        {
            // Arrange
            Product? product = null;

            // Assert & Act
            Assert.Throws<ArgumentException>(() => _orderService.AddProduct(product));
        }
    }
}
