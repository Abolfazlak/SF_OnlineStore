namespace OnlineStore_Test.ProductTest
{
    public class ProductControllerTest
    {
        [Fact]
        public async Task ProductController_GetProductById_ReturnOK()
        {
            //Arrange
            var id = 1;
            var productDto = new ProductWithProperPriceDto
            {
                Id = id,
                Title = "chips",
                Discount = 0,
                HasDiscount = false,
                OriginalPrice = 15000,
                PriceAfterDiscount = 15000,
                InventoryCount = 15,
            };

            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(s => s.GetProductWithProperPriceService(id)).ReturnsAsync(productDto);

            var loggerMock = new Mock<ILogger<ProductController>>();
            var controller = new ProductController(productServiceMock.Object, loggerMock.Object);


            //Act
            var result = await controller.GetProductById(id);

            //Assert
            var OkRes = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<ProductWithProperPriceDto>(OkRes.Value);

            Assert.Equal(id, product.Id);
            Assert.Equal(productDto.Title, product.Title);
            Assert.Equal(productDto.Discount, product.Discount);
            Assert.Equal(productDto.OriginalPrice, product.OriginalPrice);
            Assert.Equal(productDto.PriceAfterDiscount, product.PriceAfterDiscount);
            Assert.Equal(productDto.InventoryCount, product.InventoryCount);
            Assert.Equal(productDto.HasDiscount, product.HasDiscount);
        }

        [Fact]
        public async Task ProductController_GetProductById_ReturnNotFound()
        {
            //Arrange
            var id = 3;

            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(s => s.GetProductWithProperPriceService(id)).ReturnsAsync((ProductWithProperPriceDto?)null);

            var loggerMock = new Mock<ILogger<ProductController>>();
            var controller = new ProductController(productServiceMock.Object, loggerMock.Object);


            //Act
            var result = await controller.GetProductById(id);

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }
    }
}
