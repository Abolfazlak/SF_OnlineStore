namespace OnlineStore_Test.CustomerTest
{
    public class CustomerServiceTest
    {
        [Fact]
        public async Task CustomerService_BuyProductByUser_ReturnsTrue()
        {
            // Arrange
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var productServiceMock = new Mock<IProductService>();
            var loggerMock = new Mock<ILogger<CustomerService>>();

            var customerService = new CustomerService(customerRepositoryMock.Object, productServiceMock.Object, loggerMock.Object);

            customerRepositoryMock.Setup(x => x.CheckUserById(It.IsAny<int>())).ReturnsAsync(true);
            productServiceMock.Setup(x => x.CheckProductByCountAndIdService(It.IsAny<int>())).ReturnsAsync(true);
            customerRepositoryMock.Setup(x => x.AddOrderToDb(It.IsAny<BuyProductDto>(), It.IsAny<DateTime>())).ReturnsAsync(true);
            productServiceMock.Setup(x => x.DecreaseFromProductCountService(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await customerService.BuyProductByUser(new BuyProductDto
            {
                UserId = 1,
                ProductId = 1
            }, DateTime.Now);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CustomerService_BuyProductByUser_ReturnsFalse_WhenUserNotExists()
        {
            // Arrange
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var productServiceMock = new Mock<IProductService>();
            var loggerMock = new Mock<ILogger<CustomerService>>();

            var customerService = new CustomerService(customerRepositoryMock.Object, productServiceMock.Object, loggerMock.Object);

            customerRepositoryMock.Setup(x => x.CheckUserById(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await customerService.BuyProductByUser(new BuyProductDto
            {
                UserId = 1,
                ProductId = 1
            }, DateTime.Now);

            // Assert
            Assert.False(result);
        }
    }
}
