namespace UExpo.Domain.Attendents;

public interface IAttendentRepository
{
    Task<Guid> CreateAsync(Attendent item, CancellationToken cancellationToken = default);
    Task<Attendent> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
