using Microsoft.EntityFrameworkCore;

namespace SqlIntegrationTesting.IntegrationTests;

public class IntegrationTestDatabaseProvider
{
    private readonly string _connectionString;

    public IntegrationTestDatabaseProvider(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public OrderDbContext CreateDbContext()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();

        dbContextOptionsBuilder.UseSqlServer(this._connectionString);
        
        var dbContext = new OrderDbContext(dbContextOptionsBuilder.Options);

        return dbContext;
    }

    public static async Task<IntegrationTestDatabaseProvider> Build()
    {
        var databaseName = Guid.NewGuid();
        
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();

        var connectionString = IntegrationTestFixture.DatabaseConnectionString + $"Initial Catalog={databaseName};"; 
        
        dbContextOptionsBuilder.UseSqlServer(connectionString);
        
        var dbContext = new OrderDbContext(dbContextOptionsBuilder.Options);

        await dbContext.Database.MigrateAsync();

        var provider  = new IntegrationTestDatabaseProvider(connectionString);

        await provider.Seed();

        return provider;
    }    
    
    private async Task Seed()
    {
        await using var dbContext = this.CreateDbContext();

        dbContext.Orders.Add(new Order
        {
            Amount = 1000,
            ExternalId = "ab54cab60fc24e9f9592d7dbbe48a6aa",
            ProductUpc = "iphone"
        });
        
        await dbContext.SaveChangesAsync();
    }
}