using LTS.Core;
using Microsoft.EntityFrameworkCore;

namespace LTS.Infrastructure;

public class OfferingRepository(LTSDbContext db) : IOfferingRepository
{
    public async Task<Offering?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Offerings
            .Include(x => x.TrainerProfile).ThenInclude(x => x!.User)
            .Include(x => x.Bookings)
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Offering>> GetOfferingsAsync(OfferingType? type, int? trainerId, DateTime? from, DateTime? to, string? breed, CancellationToken ct = default)
    {
        var q = db.Offerings
            .Include(x => x.TrainerProfile).ThenInclude(x => x!.User)
            .Include(x => x.Bookings)
            .Where(x => x.StartDateTime >= DateTime.UtcNow && x.Status == OfferingStatus.Active);

        if (type.HasValue) q = q.Where(x => x.Type == type.Value);
        if (trainerId.HasValue) q = q.Where(x => x.TrainerProfileId == trainerId.Value);
        if (from.HasValue) q = q.Where(x => x.StartDateTime >= from.Value);
        if (to.HasValue) q = q.Where(x => x.EndDateTime <= to.Value);
        if (!string.IsNullOrWhiteSpace(breed))
            q = q.Where(x => string.IsNullOrEmpty(x.AllowedBreeds) || x.AllowedBreeds.Contains(breed));

        return await q.OrderBy(x => x.StartDateTime).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Offering>> GetByTrainerAsync(int trainerProfileId, CancellationToken ct = default) =>
        await db.Offerings
            .Include(x => x.Bookings)
            .Where(x => x.TrainerProfileId == trainerProfileId)
            .OrderBy(x => x.StartDateTime)
            .ToListAsync(ct);

    public async Task<Offering> AddAsync(Offering offering, CancellationToken ct = default)
    {
        db.Offerings.Add(offering);
        await db.SaveChangesAsync(ct);
        return offering;
    }

    public async Task UpdateAsync(Offering offering, CancellationToken ct = default)
    {
        db.Offerings.Update(offering);
        await db.SaveChangesAsync(ct);
    }
}
