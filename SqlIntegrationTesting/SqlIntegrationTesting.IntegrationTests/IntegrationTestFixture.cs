using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

namespace SqlIntegrationTesting.IntegrationTests;

public class IntegrationTestFixture : IAsyncLifetime
{
    public static string DatabaseConnectionString => $"{DatabaseTestContainer?.ConnectionString} TrustServerCertificate=True; Encrypt=False;";

    private static MsSqlTestcontainer? DatabaseTestContainer { get; set; }
    
    public async Task InitializeAsync()
    {
        if (DatabaseTestContainer is null)
        {
            DatabaseTestContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration
                {
                    Password = "P@ssword1!",
                })
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithCleanUp(true)
                .WithName($"IntegrationTestDatabaseServer-{Guid.NewGuid().ToString()}")
                .Build();
        
            await DatabaseTestContainer.StartAsync();
        }
    }

    public async Task DisposeAsync()
    {
        if (DatabaseTestContainer is not null)
        {
            await DatabaseTestContainer.StopAsync();
        }
    }
}

[CollectionDefinition("IntegrationTests collection")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}