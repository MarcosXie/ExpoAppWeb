namespace UExpo.Domain.CallCenterChat;

public interface ICallCenterChatRepository
{
    public Task<Guid> CreateAsync(CallCenterChat item, CancellationToken cancellationToken = default);
    public Task<Guid> AddMessageAsync(CallCenterMessage message, CancellationToken cancellationToken = default)
    public Task<List<CallCenterChat>> GetAsync(CancellationToken cancellationToken = default);
    public Task<CallCenterChat> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task UpdateAsync(CallCenterChat item, CancellationToken cancellationToken = default);
}
