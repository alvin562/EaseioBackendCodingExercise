using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EaseioBackendCodingExercise.Data;
using EaseioBackendCodingExercise.Models;
using EaseioBackendCodingExercise.Services;
using System.Text.RegularExpressions;
using System.Linq;


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
            var record = await _service.GetByGuidAsync(guid);
            
            if (record == null)
            {
                return NotFound();
            } 

            return Ok(record);
        }

        [HttpPost("{guid?}")]
        public async Task<IActionResult> CreateGUIDRecord(string? guid, [FromBody] CreateGUID request)
        {
            if (guid == null)
            {
                guid = GenerateGUID();
            }

            // Regex for 32 hexadecimal characters
            var hexPattern = @"^[0-9A-F]{32}$";

            if (!Regex.IsMatch(guid, hexPattern))
            {
                return BadRequest("GUID must be a 32-character uppercase hexadecimal string.");
            }

            var record = new GUIDRecord(guid, request.Expires, request.User);

            await _service.CreateAsync(record);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGUIDRecord([FromBody] CreateGUID request)
        {
            Console.WriteLine("Received request to create GUID record.");

            var guid = GenerateGUID();
            Console.WriteLine(guid);

            var record = new GUIDRecord(guid, request.Expires, request.User);

            await _service.CreateAsync(record);

            return Ok();
        }

        public string GenerateGUID()
        {
            // Create random byte array with 16 bytes
            var randomBytes = new byte[16]; 
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes); // Fill byte array with random data
            }

            // Convert to uppercase hexadecimal string
            string hexId = string.Concat(randomBytes.Select(b => b.ToString("X2")));

            return hexId;
        }
    }
}