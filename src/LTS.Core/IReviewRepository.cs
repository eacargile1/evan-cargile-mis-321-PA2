namespace LTS.Core;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Review>> GetByTrainerAsync(int trainerProfileId, bool includeHidden = false, CancellationToken ct = default);
    Task<IReadOnlyList<Review>> GetFlaggedAsync(CancellationToken ct = default);
    Task<Review> AddAsync(Review review, CancellationToken ct = default);
    Task UpdateAsync(Review review, CancellationToken ct = default);
}
