using Microsoft.EntityFrameworkCore;
using EaseioBackendCodingExercise.Models;

namespace EaseioBackendCodingExercise.Data
{
    public class GUIDDbContext : DbContext
    {
        public GUIDDbContext(DbContextOptions<GUIDDbContext> options)
            : base(options) { }

        public DbSet<GUIDRecord> GUIDRecords => Set<GUIDRecord>();
    }
}