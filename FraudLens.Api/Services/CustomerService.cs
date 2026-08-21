using FraudLens.Api.Contracts;
using FraudLens.Api.DTOs;
using Neo4j.Driver;

namespace FraudLens.Api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDriver _driver;
        public CustomerService(IDriver driver)
        {
            _driver = driver;
        }
        public async Task<List<CustomerSearchDto>> SearchAsync(string name)
        {
            var result = new List<CustomerSearchDto>();

            await using var session = _driver.AsyncSession();

            var cursor = await session.RunAsync("""
        MATCH (c:Customer)
        WHERE toLower(c.Name) CONTAINS toLower($name)
        RETURN c.CustomerId AS CustomerId,
               c.Name AS Name,
               c.Email AS Email
        ORDER BY c.Name
        LIMIT 25
        """,
                new { name });

            while (await cursor.FetchAsync())
            {
                result.Add(new CustomerSearchDto
                {
                    CustomerId = cursor.Current["CustomerId"].As<string>(),
                    Name = cursor.Current["Name"].As<string>(),
                    Email = cursor.Current["Email"].As<string>()
                });
            }

            return result;
        }

        public async Task<CustomerDetailsDto?> GetDetailsAsync(string customerId)
        {
            await using var session = _driver.AsyncSession();

            var cursor = await session.RunAsync("""
        MATCH (c:Customer {CustomerId: $customerId})

        OPTIONAL MATCH (c)-[:OWNS]->(a:Account)
        OPTIONAL MATCH (c)-[:USES]->(d:Device)
        OPTIONAL MATCH (c)-[:LOGGED_IN_FROM]->(ip:IPAddress)
        OPTIONAL MATCH (c)-[:LIVES_IN]->(city:City)

        RETURN
            c.CustomerId AS CustomerId,
            c.Name AS Name,
            c.Email AS Email,
            collect(DISTINCT a.AccountNumber) AS Accounts,
            collect(DISTINCT d.Name) AS Devices,
            collect(DISTINCT ip.Address) AS IpAddresses,
            head(collect(DISTINCT city.Name)) AS City
        """,
                new { customerId });

            if (!await cursor.FetchAsync())
                return null;

            var record = cursor.Current;

            return new CustomerDetailsDto
            {
                CustomerId = record["CustomerId"].As<string>(),
                Name = record["Name"].As<string>(),
                Email = record["Email"].As<string>(),
                Accounts = record["Accounts"].As<List<string>>(),
                Devices = record["Devices"].As<List<string>>(),
                IpAddresses = record["IpAddresses"].As<List<string>>(),
                City = record["City"].As<string?>()
            };
        }
    }
}
