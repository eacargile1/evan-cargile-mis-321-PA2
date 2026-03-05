namespace LTS.Core;

public interface ITrainerProfileRepository
{
    Task<TrainerProfile?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TrainerProfile?> GetByUserIdAsync(int userId, CancellationToken ct = default);
    Task<IReadOnlyList<TrainerProfile>> GetAllAsync(CancellationToken ct = default);
    Task<TrainerProfile> AddAsync(TrainerProfile profile, CancellationToken ct = default);
    Task UpdateAsync(TrainerProfile profile, CancellationToken ct = default);
}
