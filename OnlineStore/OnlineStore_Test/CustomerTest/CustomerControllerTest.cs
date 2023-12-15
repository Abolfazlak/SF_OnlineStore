
namespace OnlineStore_Test.CustomerTest
{
    public class CustomerControllerTest
    {
        [Fact]
        public async Task CustomerController_BuyProduct_ReturnsOkResult()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(x => x.BuyProductByUser(It.IsAny<BuyProductDto>(), It.IsAny<DateTime>())).ReturnsAsync(true);

            var loggerMock = new Mock<ILogger<CustomerController>>();
            var controller = new CustomerController(customerServiceMock.Object, loggerMock.Object);

            // Act
            var result = await controller.BuyProduct(new BuyProductDto { ProductId = 1, UserId = 1});

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task BuyProduct_ReturnsProblemResult()
        {
            // Arrange
            var customerServiceMock = new Mock<ICustomerService>();
            customerServiceMock.Setup(x => x.BuyProductByUser(It.IsAny<BuyProductDto>(), It.IsAny<DateTime>())).ReturnsAsync(false);

            var loggerMock = new Mock<ILogger<CustomerController>>();
            var controller = new CustomerController(customerServiceMock.Object, loggerMock.Object);

            // Act
            var result = await controller.BuyProduct(new BuyProductDto());

            // Assert
            Assert.Equal(500, (result as ObjectResult)?.StatusCode);
        }

    }

}
