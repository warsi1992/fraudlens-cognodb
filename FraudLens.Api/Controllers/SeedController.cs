using FraudLens.Api.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FraudLens.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ISeedService _seedService;

        public SeedController(ISeedService seedService)
        {
            _seedService = seedService;
        }

        [HttpPost]
        public async Task<IActionResult> Seed()
        {
            await _seedService.SeedAsync();

            return Ok("Database seeded successfully.");
        }
    }
}
