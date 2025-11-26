using EaseioBackendCodingExercise.Data;
using EaseioBackendCodingExercise.Models;
using Microsoft.EntityFrameworkCore;

namespace EaseioBackendCodingExercise.Services
{
    public class GUIDService : IGUIDService
    {
        private readonly GUIDDbContext _context;

        public GUIDService(GUIDDbContext context)
        {
            _context = context;
        }

        public async Task<GUIDRecord?> GetByGuidAsync(string guid) =>
            await _context.GUIDRecords.FindAsync(guid);

        public async Task<bool> CreateAsync(GUIDRecord record)
        {
            var foundRecord = await _context.GUIDRecords.FindAsync(record.Guid);

            if (foundRecord != null)
            {
                return false;   // record already exists
            }

            _context.GUIDRecords.Add(record);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(string guid, DateTimeOffset newExpires)
        {
            var record = await _context.GUIDRecords.FindAsync(guid);

            if (record == null)
            {
                return false;
            }

            record.Expires = newExpires;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(string guid)
        {
            var record = await _context.GUIDRecords.FindAsync(guid);

            if (record == null)
            {
                return false;   // record doesn't exist
            }

            //var record = new GUIDRecord(guid);
            _context.GUIDRecords.Attach(record);
            _context.GUIDRecords.Remove(record);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}