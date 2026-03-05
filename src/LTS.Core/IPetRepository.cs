namespace LTS.Core;

public interface IPetRepository
{
    Task<Pet?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Pet>> GetByOwnerAsync(int ownerId, CancellationToken ct = default);
    Task<IReadOnlyList<Pet>> GetAllAsync(CancellationToken ct = default);
    Task<Pet> AddAsync(Pet pet, CancellationToken ct = default);
    Task UpdateAsync(Pet pet, CancellationToken ct = default);
}
