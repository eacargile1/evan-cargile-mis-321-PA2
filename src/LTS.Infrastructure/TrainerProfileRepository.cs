using LTS.Core;
using Microsoft.EntityFrameworkCore;

namespace LTS.Infrastructure;

public class TrainerProfileRepository(LTSDbContext db) : ITrainerProfileRepository
{
    public async Task<TrainerProfile?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.TrainerProfiles
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<TrainerProfile?> GetByUserIdAsync(int userId, CancellationToken ct = default) =>
        await db.TrainerProfiles
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId, ct);

    public async Task<IReadOnlyList<TrainerProfile>> GetAllAsync(CancellationToken ct = default) =>
        await db.TrainerProfiles
            .Include(x => x.User)
            .ToListAsync(ct);

    public async Task<TrainerProfile> AddAsync(TrainerProfile profile, CancellationToken ct = default)
    {
        db.TrainerProfiles.Add(profile);
        await db.SaveChangesAsync(ct);
        return profile;
    }

    public async Task UpdateAsync(TrainerProfile profile, CancellationToken ct = default)
    {
        db.TrainerProfiles.Update(profile);
        await db.SaveChangesAsync(ct);
    }
}
