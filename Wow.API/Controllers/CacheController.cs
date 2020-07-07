using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Wow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IDistributedCache cache;
        private readonly ILogger<CacheController> _logger;

        public CacheController(IDistributedCache cache,ILogger<CacheController> logger)
        {
            this.cache = cache;
            _logger = logger;
        }

        [HttpPost()]
        public async Task<IActionResult> Set([FromBody] CacheItem item)
        {
            _logger.LogInformation($"Setting key:{item.Key} value:{item.Value} in cache");
            await cache.SetStringAsync(item.Key, item.Value);
            return Ok();
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromRoute]string key)
        {
            _logger.LogInformation($"Getting key:{key} from cache");
            var result = await cache.GetStringAsync(key);
            return Ok(result);
        }
    }

    public class CacheItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
