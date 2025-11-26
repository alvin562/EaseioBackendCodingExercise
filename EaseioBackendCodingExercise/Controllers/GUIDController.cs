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
        private readonly IGUIDService _service;

        public GuidController(IGUIDService service)
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

            if (record.Expires < DateTimeOffset.UtcNow)
            {
                return NotFound("Record found but it is expired.");
            }

            return Ok(record);
        }

        [HttpPost("{guid}")]
        public async Task<IActionResult> CreateGUIDRecord(string guid, [FromBody] CreateGUID request)
        {
            // Regex for 32 hexadecimal characters
            var hexPattern = @"^[0-9A-F]{32}$";

            if (!Regex.IsMatch(guid, hexPattern))
            {
                return BadRequest("GUID must be a 32-character uppercase hexadecimal string.");
            }

            var record = new GUIDRecord(guid, GetExpireDate(request), request.User);

            bool created = await _service.CreateAsync(record);

            if (!created)
            {
                return Conflict("Unable to create record. A record with guid " + guid + " already exists.");
            }

            return CreatedAtAction(nameof(GetGUIDRecord), new { guid = record.Guid }, record);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGUIDRecord([FromBody] CreateGUID request)
        {
            var guid = GenerateGUID();
            Console.WriteLine("Creating record with the following generated GUID: " + guid);

            var record = new GUIDRecord(guid, GetExpireDate(request), request.User);

            bool created = await _service.CreateAsync(record);

            if (!created)
            {
                return Conflict("Unable to create record. A record with guid " + guid + " already exists.");
            }

            return CreatedAtAction(nameof(GetGUIDRecord), new { guid = record.Guid }, record);
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> UpdateGUIDRecord(string guid, [FromBody] UpdateGUID request)
        {
            bool updated = await _service.UpdateAsync(guid, request.Expires);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteGUIDRecord(string guid)
        {
            var recordFound = await _service.DeleteAsync(guid);

            if (!recordFound)
            {
                Console.WriteLine("Unable to delete record as GUID is not found: " + guid);
                return NotFound("Unable to delete record as GUID is not found: " + guid);
            }

            return Ok();
        }

        public DateTimeOffset GetExpireDate(CreateGUID request)
        {
            if (request.Expires != null)
            {
                return request.Expires.Value;
            }

            return DateTimeOffset.UtcNow.AddDays(30);
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