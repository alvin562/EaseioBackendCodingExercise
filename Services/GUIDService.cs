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

        // public async Task CreateAsync(GUIDRecord record)
        // {
        //     _context.GUIDRecords.Add(record);
        //     await _context.SaveChangesAsync();
        // }

        // public async Task UpdateAsync(GUIDRecord record)
        // {
        //     _context.GUIDRecords.Update(record);
        //     await _context.SaveChangesAsync();
        // }

        // public async Task DeleteAsync(GUIDRecord record)
        // {
        //     _context.GUIDRecords.Remove(record);
        //     await _context.SaveChangesAsync();
        // }
    }
}