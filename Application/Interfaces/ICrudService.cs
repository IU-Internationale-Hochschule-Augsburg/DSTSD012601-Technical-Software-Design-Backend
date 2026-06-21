namespace Subscription_Control_Backend.Application.Interfaces;

public interface ICrudService<TResponse, in TCreateRequest, in TUpdateRequest>
{
    Task<List<TResponse>> GetAllAsync(CancellationToken ct = default);
    Task<TResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<TResponse> CreateAsync(TCreateRequest request, CancellationToken ct = default);
    Task<TResponse?> UpdateAsync(Guid id, TUpdateRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
