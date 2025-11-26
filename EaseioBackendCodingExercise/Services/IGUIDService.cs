using EaseioBackendCodingExercise.Data;
using EaseioBackendCodingExercise.Models;

public interface IGUIDService
{
    Task<GUIDRecord?> GetByGuidAsync(string guid);
    Task<bool> CreateAsync(GUIDRecord record);
    Task<bool> UpdateAsync(string guid, DateTimeOffset newExpires);
    Task<bool> DeleteAsync(string guid);
}