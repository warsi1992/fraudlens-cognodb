using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace FraudLens.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDriver _driver;

        public DashboardController(IDriver driver)
        {
            _driver = driver;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using var session = _driver.AsyncSession();

            var cursor = await session.RunAsync("""
            CALL {
                MATCH (n:Customer)
                RETURN count(n) AS Customers
            }
            CALL {
                MATCH (n:Account)
                RETURN count(n) AS Accounts
            }
            CALL {
                MATCH (n:Device)
                RETURN count(n) AS Devices
            }
            CALL {
                MATCH (n:IPAddress)
                RETURN count(n) AS IpAddresses
            }
            CALL {
                MATCH ()-[r:TRANSFERRED_TO]->()
                RETURN count(r) AS Transfers
            }
            RETURN Customers,
                   Accounts,
                   Devices,
                   IpAddresses,
                   Transfers
            """);

            await cursor.FetchAsync();

            return Ok(new
            {
                customers = cursor.Current["Customers"].As<long>(),
                accounts = cursor.Current["Accounts"].As<long>(),
                devices = cursor.Current["Devices"].As<long>(),
                ipAddresses = cursor.Current["IpAddresses"].As<long>(),
                transfers = cursor.Current["Transfers"].As<long>()
            });
        }
    }
    }
