using FraudLens.Api.Contracts;
using FraudLens.Api.DTOs;
using Neo4j.Driver;

namespace FraudLens.Api.Repositories
{
    public class FraudRepository : IFraudRepository
    {
        private readonly IDriver _driver;

        public FraudRepository(IDriver driver)
        {
            _driver = driver;
        }


        public async Task<List<SharedIpDto>> GetSharedIpsAsync(string customerName)
        {
            var result = new List<SharedIpDto>();

            await using var session = _driver.AsyncSession();

            var cursor = await session.RunAsync("""
        MATCH (c:Customer)-[:LOGGED_IN_FROM]->(ip:IPAddress)
              <-[:LOGGED_IN_FROM]-(other:Customer)
        WHERE toLower(c.Name) = toLower($customerName)
          AND other.Name <> c.Name
        RETURN c.Name AS Customer,
               ip.Address AS IpAddress,
               other.Name AS SharedWith
        ORDER BY ip.Address
        """,
                new { customerName });

            while (await cursor.FetchAsync())
            {
                result.Add(new SharedIpDto
                {
                    CustomerName = cursor.Current["Customer"].As<string>(),
                    IpAddress = cursor.Current["IpAddress"].As<string>(),
                    SharedWith = cursor.Current["SharedWith"].As<string>()
                });
            }

            return result;
        }
        public async Task<TransferChainDto?> GetTransferChainAsync(string accountNumber)
        {
            await using var session = _driver.AsyncSession();

            var cursor = await session.RunAsync("""
        MATCH p=(a:Account)-[:TRANSFERRED_TO*1..5]->(b:Account)
        WHERE a.AccountNumber = $accountNumber
        RETURN [node IN nodes(p) | node.AccountNumber] AS Path
        ORDER BY length(p) DESC
        LIMIT 1
        """,
                new { accountNumber });

            if (!await cursor.FetchAsync())
                return null;

            return new TransferChainDto
            {
                StartAccount = accountNumber,
                Path = cursor.Current["Path"].As<List<string>>()
            };
        }
        public async Task<FraudScoreDto> GetFraudScoreAsync(string customerName)
        {
            await using var session = _driver.AsyncSession();

            var cursor = await session.RunAsync("""
        MATCH (c:Customer)
        WHERE toLower(c.Name) = toLower($customerName)

        OPTIONAL MATCH (c)-[:USES]->(:Device)<-[:USES]-(deviceCustomer:Customer)
        WHERE deviceCustomer <> c

        OPTIONAL MATCH (c)-[:LOGGED_IN_FROM]->(:IPAddress)
                    <-[:LOGGED_IN_FROM]-(ipCustomer:Customer)
        WHERE ipCustomer <> c

        OPTIONAL MATCH (c)-[:OWNS]->(:Account)
                    -[:TRANSFERRED_TO*2..5]->(:Account)

        RETURN
            count(DISTINCT deviceCustomer) AS SharedDevices,
            count(DISTINCT ipCustomer) AS SharedIps,
            count(DISTINCT deviceCustomer) +
            count(DISTINCT ipCustomer) AS RiskSignals
        """,
                new { customerName });

            if (!await cursor.FetchAsync())
            {
                return new FraudScoreDto
                {
                    CustomerName = customerName,
                    Score = 0
                };
            }

            var sharedDevices = cursor.Current["SharedDevices"].As<long>();
            var sharedIps = cursor.Current["SharedIps"].As<long>();

            var reasons = new List<string>();

            if (sharedDevices > 0)
                reasons.Add("Shared device detected");

            if (sharedIps > 0)
                reasons.Add("Shared IP address detected");

            var score = Math.Min(
                100,
                (int)(sharedDevices * 30 + sharedIps * 30));

            return new FraudScoreDto
            {
                CustomerName = customerName,
                Score = score,
                Reasons = reasons
            };
        }
        public async Task<List<SharedDeviceDto>> GetSharedDevicesAsync(string customerName)
        {
            var result = new List<SharedDeviceDto>();

            await using var session = _driver.AsyncSession();

            var cursor = await session.RunAsync(@"
MATCH (c:Customer)-[:USES]->(d:Device)<-[:USES]-(other:Customer)
WHERE c.Name=$customerName
AND other.Name<>$customerName
RETURN c.Name AS Customer,
       d.Name AS Device,
       other.Name AS SharedWith",
    new { customerName });

            while (await cursor.FetchAsync())
            {
                result.Add(new SharedDeviceDto
                {
                    CustomerName = cursor.Current["Customer"].As<string>(),
                    DeviceName = cursor.Current["Device"].As<string>(),
                    SharedWith = cursor.Current["SharedWith"].As<string>()
                });
            }

            return result;
        }
    }
}
