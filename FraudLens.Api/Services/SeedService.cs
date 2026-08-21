using FraudLens.Api.Contracts;
using Neo4j.Driver;

namespace FraudLens.Api.Services
{
    public class SeedService : ISeedService
    {
        private readonly IDriver _driver;

        public SeedService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task SeedAsync()
        {
            await using var session = _driver.AsyncSession();

            // Clear database
            await session.RunAsync(@"
        MATCH (n)
        DETACH DELETE n
    ");

            // Seed database
            await session.RunAsync(@"
        CREATE
        (delhi:City {Name:'Delhi'}),
        (mumbai:City {Name:'Mumbai'}),

        (iphone:Device {Name:'iPhone 15', Type:'Mobile'}),
        (laptop:Device {Name:'Dell XPS', Type:'Laptop'}),

        (ip1:IPAddress {Address:'103.45.12.1'}),
        (ip2:IPAddress {Address:'103.45.12.2'}),

        (john:Customer {
            CustomerId: randomUUID(),
            Name:'John Smith',
            Email:'john@test.com'
        }),

        (alice:Customer {
            CustomerId: randomUUID(),
            Name:'Alice Brown',
            Email:'alice@test.com'
        }),

        (bob:Customer {
            CustomerId: randomUUID(),
            Name:'Bob Wilson',
            Email:'bob@test.com'
        }),

        (acc1:Account {
            AccountNumber:'ACC001',
            Balance:25000
        }),

        (acc2:Account {
            AccountNumber:'ACC002',
            Balance:12000
        }),

        (acc3:Account {
            AccountNumber:'ACC003',
            Balance:8000
        }),

        (john)-[:OWNS]->(acc1),
        (alice)-[:OWNS]->(acc2),
        (bob)-[:OWNS]->(acc3),

        (john)-[:USES]->(iphone),
        (alice)-[:USES]->(iphone),
        (bob)-[:USES]->(laptop),

        (john)-[:LOGGED_IN_FROM]->(ip1),
        (alice)-[:LOGGED_IN_FROM]->(ip1),
        (bob)-[:LOGGED_IN_FROM]->(ip2),

        (john)-[:LIVES_IN]->(delhi),
        (alice)-[:LIVES_IN]->(delhi),
        (bob)-[:LIVES_IN]->(mumbai),

        (acc1)-[:TRANSFERRED_TO]->(acc2),
        (acc2)-[:TRANSFERRED_TO]->(acc3)
    ");
        }
    }
}
