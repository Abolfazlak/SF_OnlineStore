namespace OnlineStore_Test.ProductTest;
public class ProductRepositoryTests
{
    [Fact]
    public async Task ProductRepository_GetProductByIdFromDb_ReturnsProductDto()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ProductRepository>>();
        var dbContextOptions = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
        var configurationMock = new Mock<IConfiguration>();
        var dbContext = new ProductDbContext(configurationMock.Object, dbContextOptions);
        var repository = new ProductRepository(loggerMock.Object, dbContext);

        var productId = 1;
        var product = new Product { Id = productId, Title = "chips", Price = 15000, InventoryCount = 10, Discount = 0 };
        dbContext.Products.Add(product);
        dbContext.SaveChanges();

        // Act
        var result = await repository.GetProductByIdFromDb(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result?.Id);
    }

    [Fact]
    public async Task ProductRepository_GetProductByIdFromDb_ReturnsNull()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ProductRepository>>();
        var dbContextOptions = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
        var configurationMock = new Mock<IConfiguration>();
        var dbContext = new ProductDbContext(configurationMock.Object, dbContextOptions);
        var repository = new ProductRepository(loggerMock.Object, dbContext);

        // Act
        var result = await repository.GetProductByIdFromDb(7);

        // Assert
        Assert.Null(result);
    }
}
