using LTS.Core;
using Microsoft.EntityFrameworkCore;

namespace LTS.Infrastructure;

public class BookingRepository(LTSDbContext db) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Bookings
            .Include(x => x.Offering).ThenInclude(x => x!.TrainerProfile).ThenInclude(x => x!.User)
            .Include(x => x.Pet)
            .Include(x => x.Review)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Booking>> GetAllAsync(DateTime? from, DateTime? to, int? trainerId, CancellationToken ct = default)
    {
        var q = db.Bookings
            .Include(x => x.Offering).ThenInclude(x => x!.TrainerProfile).ThenInclude(x => x!.User)
            .Include(x => x.Pet)
            .Where(x => x.Status != BookingStatus.Cancelled);

        if (from.HasValue) q = q.Where(x => x.Offering.StartDateTime >= from.Value);
        if (to.HasValue) q = q.Where(x => x.Offering.EndDateTime <= to.Value);
        if (trainerId.HasValue) q = q.Where(x => x.Offering.TrainerProfileId == trainerId.Value);

        return await q.OrderBy(x => x.Offering.StartDateTime).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Booking>> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await db.Bookings
            .Include(x => x.Offering).ThenInclude(x => x!.TrainerProfile)
            .Include(x => x.Pet)
            .Include(x => x.Review)
            .Where(x => x.CustomerEmail == email)
            .OrderByDescending(x => x.Offering.StartDateTime)
            .ToListAsync(ct);

    public async Task<Booking> AddAsync(Booking booking, CancellationToken ct = default)
    {
        db.Bookings.Add(booking);
        await db.SaveChangesAsync(ct);
        return booking;
    }

    public async Task UpdateAsync(Booking booking, CancellationToken ct = default)
    {
        db.Bookings.Update(booking);
        await db.SaveChangesAsync(ct);
    }
}
