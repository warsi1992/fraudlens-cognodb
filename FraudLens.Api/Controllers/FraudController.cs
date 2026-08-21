using FraudLens.Api.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FraudLens.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FraudController : ControllerBase
    {
        private readonly IFraudService _service;

        public FraudController(IFraudService service)
        {
            _service = service;
        }

        [HttpGet("shared-device/{customerName}")]
        public async Task<IActionResult> SharedDevice(string customerName)
        {
            var result = await _service.GetSharedDevicesAsync(customerName);

            return Ok(result);
        }

        [HttpGet("shared-ip/{customerName}")]
        public async Task<IActionResult> SharedIp(string customerName)
        {
            return Ok(await _service.GetSharedIpsAsync(customerName));
        }

        [HttpGet("transfer-chain/{accountNumber}")]
        public async Task<IActionResult> TransferChain(string accountNumber)
        {
            var result = await _service.GetTransferChainAsync(accountNumber);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("score/{customerName}")]
        public async Task<IActionResult> FraudScore(string customerName)
        {
            return Ok(await _service.GetFraudScoreAsync(customerName));
        }
    }
}
