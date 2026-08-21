using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace FraudLens.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IDriver _driver;

        public HealthController(IDriver driver)
        {
            _driver = driver;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using var session = _driver.AsyncSession();

            var cursor = await session.RunAsync("RETURN 'Connected to CognoDB' AS Message");

            var record = await cursor.SingleAsync();

            return Ok(record["Message"]);
        }
    }
}
