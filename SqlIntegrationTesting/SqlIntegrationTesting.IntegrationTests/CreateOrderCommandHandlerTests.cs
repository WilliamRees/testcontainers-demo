namespace SqlIntegrationTesting.IntegrationTests;

[CollectionDefinition("IntegrationTests collection")]
public class CreateOrderCommandHandlerTests : IClassFixture<IntegrationTestFixture>
{
    
    
    public CreateOrderCommandHandlerTests(IntegrationTestFixture fixture)
    {
    
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowDuplicateOrderException_WhenAnOrderIsSubmittedWithADuplicateExternalOrderId()
    {
        // Arrange
        var integrationTestDatabaseProvider = await IntegrationTestDatabaseProvider.Build();

        var dbContext = integrationTestDatabaseProvider.CreateDbContext();

        var handler = new CreateOrderCommandHandler(dbContext);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateOrderException>(async () => await handler.HandleAsync(new Order
        {
            Amount = 1000,
            ExternalId = "ab54cab60fc24e9f9592d7dbbe48a6aa",
            ProductUpc = "iphone"
        }));
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateOrderAndAssignNextAvailableId_WhenAnOrderDataIsValid()
    {
        // Arrange
        var integrationTestDatabaseFactory = await IntegrationTestDatabaseProvider.Build();

        var expectedOrderId = 2;

        var dbContext = integrationTestDatabaseFactory.CreateDbContext();

        var handler = new CreateOrderCommandHandler(dbContext);

        // Act
        var orderId = await handler.HandleAsync(new Order
        {
            Amount = 1000,
            ExternalId = "13ad8c515b294069add8756c769fbf57",
            ProductUpc = "iphone"
        });
        
        // Assert
        Assert.Equal(expectedOrderId, orderId);
    }
}