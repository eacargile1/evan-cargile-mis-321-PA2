namespace LTS.Core;

public interface IOfferingRepository
{
    Task<Offering?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Offering>> GetOfferingsAsync(OfferingType? type, int? trainerId, DateTime? from, DateTime? to, string? breed, CancellationToken ct = default);
    Task<IReadOnlyList<Offering>> GetByTrainerAsync(int trainerProfileId, CancellationToken ct = default);
    Task<Offering> AddAsync(Offering offering, CancellationToken ct = default);
    Task UpdateAsync(Offering offering, CancellationToken ct = default);
}
