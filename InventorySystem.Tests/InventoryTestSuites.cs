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
    }
}
