namespace SqlIntegrationTesting;

public class DuplicateOrderException : Exception
{
    public DuplicateOrderException(string externalId) : base ($"Order with external id {externalId} already exists.")
    {
        
    }
}