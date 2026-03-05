namespace LTS.Core;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetAllAsync(DateTime? from, DateTime? to, int? trainerId, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Booking> AddAsync(Booking booking, CancellationToken ct = default);
    Task UpdateAsync(Booking booking, CancellationToken ct = default);
}
