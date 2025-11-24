using EaseioBackendCodingExercise.Data;
using EaseioBackendCodingExercise.Models;
using Microsoft.EntityFrameworkCore;

namespace EaseioBackendCodingExercise.Services
{
    public class GUIDService
    {
        private readonly GUIDDbContext _context;

        public GUIDService(GUIDDbContext context)
        {
            _context = context;
        }

        public async Task<GUIDRecord?> GetByGuidAsync(string guid) =>
            await _context.GUIDRecords.FindAsync(guid);

        // public async Task<List<GUIDRecord>> GetAllAsync() =>
        //     await _context.GUIDRecords.ToListAsync();

        public async Task CreateAsync(GUIDRecord record)
        {
            _context.GUIDRecords.Add(record);
            await _context.SaveChangesAsync();
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