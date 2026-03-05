using LTS.Core;
using Microsoft.EntityFrameworkCore;

namespace LTS.Infrastructure;

public class UserRepository(LTSDbContext db) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.Users.Include(x => x.TrainerProfile).FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await db.Users.Include(x => x.TrainerProfile).FirstOrDefaultAsync(x => x.Email == email.ToLower(), ct);

    public async Task<User> AddAsync(User user, CancellationToken ct = default)
    {
        user.Email = user.Email.ToLower();
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return user;
    }

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync(ct);
    }
}
