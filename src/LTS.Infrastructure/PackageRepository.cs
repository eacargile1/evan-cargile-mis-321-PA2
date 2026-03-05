using LTS.Core;
using Microsoft.EntityFrameworkCore;

namespace LTS.Infrastructure;

public class PackageRepository(LTSDbContext db) : IPackageRepository
{
    public async Task<Package?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Packages.Include(x => x.TrainerProfile).Include(x => x.PackageBookings)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Package>> GetByTrainerAsync(int trainerProfileId, CancellationToken ct = default) =>
        await db.Packages.Include(x => x.PackageBookings)
            .Where(x => x.TrainerProfileId == trainerProfileId).ToListAsync(ct);

    public async Task<IReadOnlyList<Package>> GetAllActiveAsync(CancellationToken ct = default) =>
        await db.Packages.Include(x => x.TrainerProfile).ThenInclude(x => x!.User)
            .Where(x => x.IsActive).ToListAsync(ct);

    public async Task<IReadOnlyList<PackageBooking>> GetAllBookingsAsync(DateTime? from, DateTime? to, int? trainerProfileId, CancellationToken ct = default)
    {
        var q = db.PackageBookings
            .Include(x => x.Package).ThenInclude(x => x!.TrainerProfile)
            .Where(x => x.Status != BookingStatus.Cancelled);

        if (trainerProfileId.HasValue) q = q.Where(x => x.Package.TrainerProfileId == trainerProfileId.Value);
        if (from.HasValue) q = q.Where(x => x.CreatedAt >= from.Value);
        if (to.HasValue) q = q.Where(x => x.CreatedAt <= to.Value);

        return await q.OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
    }

    public async Task<Package> AddAsync(Package package, CancellationToken ct = default)
    {
        db.Packages.Add(package);
        await db.SaveChangesAsync(ct);
        return package;
    }

    public async Task UpdateAsync(Package package, CancellationToken ct = default)
    {
        db.Packages.Update(package);
        await db.SaveChangesAsync(ct);
    }

    public async Task<PackageBooking> AddBookingAsync(PackageBooking booking, CancellationToken ct = default)
    {
        db.PackageBookings.Add(booking);
        await db.SaveChangesAsync(ct);
        return booking;
    }
}
