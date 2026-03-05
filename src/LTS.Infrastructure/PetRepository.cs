using LTS.Core;
using Microsoft.EntityFrameworkCore;

namespace LTS.Infrastructure;

public class PetRepository(LTSDbContext db) : IPetRepository
{
    public async Task<Pet?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Pets.Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Pet>> GetByOwnerAsync(int ownerId, CancellationToken ct = default) =>
        await db.Pets.Where(x => x.OwnerId == ownerId).ToListAsync(ct);

    public async Task<IReadOnlyList<Pet>> GetAllAsync(CancellationToken ct = default) =>
        await db.Pets.Include(x => x.Owner).ToListAsync(ct);

    public async Task<Pet> AddAsync(Pet pet, CancellationToken ct = default)
    {
        db.Pets.Add(pet);
        await db.SaveChangesAsync(ct);
        return pet;
    }

    public async Task UpdateAsync(Pet pet, CancellationToken ct = default)
    {
        db.Pets.Update(pet);
        await db.SaveChangesAsync(ct);
    }
}
