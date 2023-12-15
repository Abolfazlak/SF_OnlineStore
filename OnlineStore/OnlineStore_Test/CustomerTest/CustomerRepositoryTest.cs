namespace OnlineStore_Test.CustomerTest
{
    public class CustomerRepositoryTests
    {
        [Fact]
        public async Task CustomerRepository_AddOrderToDb_ReturnsTrue()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CustomerRepository>>();
            var dbContextOptions = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            var configurationMock = new Mock<IConfiguration>();
            var dbContext = new ProductDbContext(configurationMock.Object, dbContextOptions);
            var repository = new CustomerRepository(loggerMock.Object, dbContext);

            var order = new Order { Id = 1, ProductId = 1, UserId = 1, CreationDate = DateTime.Now};
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();


            // Act
            var result = await repository.AddOrderToDb(new BuyProductDto { UserId = 1, ProductId = 1}, DateTime.Now);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CustomerRepository_CheckUserByName_ReturnsTrue_WhenUserExists()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CustomerRepository>>();
            var dbContextOptions = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            var configurationMock = new Mock<IConfiguration>();
            var dbContext = new ProductDbContext(configurationMock.Object, dbContextOptions);
            var repository = new CustomerRepository(loggerMock.Object, dbContext);


            var username = "abolfazl";
            await repository.CreateUser(username);

            // Act
            var result = await repository.CheckUserByName(username);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task CustomerRepository_CheckUserByName_ReturnsFalse_WhenUserNotExists()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CustomerRepository>>();
            var dbContextOptions = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            var configurationMock = new Mock<IConfiguration>();
            var dbContext = new ProductDbContext(configurationMock.Object, dbContextOptions);
            var repository = new CustomerRepository(loggerMock.Object, dbContext);

            var username = "abolfazl";
            await repository.CreateUser(username);

            // Act
            var result = await repository.CheckUserByName("amir");

            // Assert
            Assert.False(result);
        }
    }
}