using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EaseioBackendCodingExercise.Data;
using EaseioBackendCodingExercise.Models;
using EaseioBackendCodingExercise.Services;

namespace EaseioBackendCodingExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuidController : ControllerBase
    {
        private readonly GUIDService _service;

        public GuidController(GUIDService service)
        {
            _service = service;
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetGUIDRecord(string guid)
        {    
            // var record = new GUIDRecord(guid, DateTimeOffset.Parse("2024-10-01T14:18:00.000Z"), "Alvin Dantic");
            var record = await _service.GetByGuidAsync(guid);
            
            if (record == null) return NotFound();

            return Ok(record);
        }
    }
}