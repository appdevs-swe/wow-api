using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Wow.API.Models;

namespace Wow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly IDistributedCache cache;
        private readonly ILogger<StudentsController> _logger;
        private readonly static string Key = "Key";

        public StudentsController(SchoolContext context, IDistributedCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("cacheset")]
        public async Task<IActionResult> SetCache()
        {
            var currentTimeUTC = DateTime.UtcNow.ToString();
            var result = await cache.GetStringAsync(Key);
            await cache.SetStringAsync(Key, $"{result} \n {currentTimeUTC}");
            return Ok();
        }

        [HttpGet("cacheget")]
        public async Task<IActionResult> GetCache()
        {
            
            var result = await cache.GetStringAsync(Key);
            return Ok(result);
        }


    }
}
