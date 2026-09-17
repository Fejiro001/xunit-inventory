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
    }
}
