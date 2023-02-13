using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SqlIntegrationTesting;

public class CreateOrderCommandHandler
{
    private readonly OrderDbContext _dbContext;

    public CreateOrderCommandHandler(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<int> HandleAsync(Order order, CancellationToken cancellationToken = default)
    {
        try
        {
            this._dbContext.Add(order);

            await this._dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is SqlException { Number: 2601 })
            {
                throw new DuplicateOrderException(order.ExternalId);
            }
        }

        return order.Id;
    }
}