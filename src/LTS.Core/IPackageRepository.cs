namespace LTS.Core;

public interface IPackageRepository
{
    Task<Package?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Package>> GetByTrainerAsync(int trainerProfileId, CancellationToken ct = default);
    Task<IReadOnlyList<Package>> GetAllActiveAsync(CancellationToken ct = default);
    Task<IReadOnlyList<PackageBooking>> GetAllBookingsAsync(DateTime? from, DateTime? to, int? trainerProfileId, CancellationToken ct = default);
    Task<Package> AddAsync(Package package, CancellationToken ct = default);
    Task UpdateAsync(Package package, CancellationToken ct = default);
    Task<PackageBooking> AddBookingAsync(PackageBooking booking, CancellationToken ct = default);
}
