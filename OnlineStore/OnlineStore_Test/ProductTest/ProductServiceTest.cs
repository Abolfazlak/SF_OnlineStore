namespace OnlineStore_Test.ProductTest
{
    public class ProductServiceTest
    {
        [Fact]
        public async Task ProductService_GetProductWithProperPrice_ReturnsProductWithDiscount()
        {
            // Arrange
            var productId = 1;

            var configurationMock = new Mock<IConfiguration>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();
            var loggerMock = new Mock<ILogger<ProductService>>();

            var productService = new ProductService(configurationMock.Object, productRepositoryMock.Object, cacheServiceMock.Object, loggerMock.Object);

            var productDto = new ProductDto
            {
                Id = productId,
                Price = 100,
                Discount = 5,
                InventoryCount = 5
            };

            productRepositoryMock.Setup(x => x.GetProductByIdFromDb(productId)).ReturnsAsync(productDto);

            // Act
            var result = await productService.GetProductWithProperPriceService(productId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result?.HasDiscount);
            Assert.Equal(95, result?.PriceAfterDiscount);
        }

        [Fact]
        public void ProductService_CalculatePrice_ReturnsWithDiscount()
        {
            //Arrange
            var configurationMock = new Mock<IConfiguration>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();
            var loggerMock = new Mock<ILogger<ProductService>>();

            var productService = new ProductService(configurationMock.Object, productRepositoryMock.Object, cacheServiceMock.Object, loggerMock.Object);

            var originalPrice = 100;
            var discount = 10.0;

            //Act
            var result = productService.CalculatePrice(originalPrice, discount);

            //Assert
            Assert.Equal(90, result);
            Assert.NotEqual(89, result);
        }

        [Fact]
        public void ProductService_CalculatePrice_ReturnsWithoutDiscount()
        {
            //Arrange
            var configurationMock = new Mock<IConfiguration>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();
            var loggerMock = new Mock<ILogger<ProductService>>();

            var productService = new ProductService(configurationMock.Object, productRepositoryMock.Object, cacheServiceMock.Object, loggerMock.Object);

            var originalPrice = 100;
            var discount = 0;

            //Act
            var result = productService.CalculatePrice(originalPrice, discount);

            //Assert
            Assert.Equal(100, result);
        }
    }
}
