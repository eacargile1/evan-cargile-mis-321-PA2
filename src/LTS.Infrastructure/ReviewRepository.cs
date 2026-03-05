using LTS.Core;
using Microsoft.EntityFrameworkCore;

namespace LTS.Infrastructure;

public class ReviewRepository(LTSDbContext db) : IReviewRepository
{
    public async Task<Review?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Reviews.Include(x => x.TrainerProfile).FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Review>> GetByTrainerAsync(int trainerProfileId, bool includeHidden = false, CancellationToken ct = default)
    {
        var q = db.Reviews.Where(x => x.TrainerProfileId == trainerProfileId);
        if (!includeHidden) q = q.Where(x => !x.IsHidden);
        return await q.OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Review>> GetFlaggedAsync(CancellationToken ct = default) =>
        await db.Reviews.Include(x => x.TrainerProfile).Include(x => x.Booking)
            .Where(x => x.IsFlagged).OrderByDescending(x => x.CreatedAt).ToListAsync(ct);

    public async Task<Review> AddAsync(Review review, CancellationToken ct = default)
    {
        db.Reviews.Add(review);
        await db.SaveChangesAsync(ct);
        return review;
    }

    public async Task UpdateAsync(Review review, CancellationToken ct = default)
    {
        db.Reviews.Update(review);
        await db.SaveChangesAsync(ct);
    }
}
