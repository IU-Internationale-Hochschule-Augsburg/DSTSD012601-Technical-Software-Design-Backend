using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.BillingCycles;
using Subscription_Control_Backend.Contracts.Responses.BillingCycles;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Mapper;

namespace Subscription_Control_Backend.Application.Services;

public class BillingCycleService : IBillingCycleService
{
    private readonly IRepository<BillingCycle> _repository;

    public BillingCycleService(IRepository<BillingCycle> repository)
    {
        _repository = repository;
    }

    public async Task<List<BillingCycleResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var billingCycles = await _repository.GetAllAsync(ct);
        return billingCycles.Select(BillingCycleMapper.ToResponse).ToList();
    }

    public async Task<BillingCycleResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var billingCycle = await _repository.GetByIdAsync(id, ct);
        return billingCycle is null ? null : BillingCycleMapper.ToResponse(billingCycle);
    }

    public async Task<BillingCycleResponse> CreateAsync(CreateBillingCycleRequest request, CancellationToken ct = default)
    {
        var billingCycle = BillingCycleMapper.ToEntity(request);
        await _repository.AddAsync(billingCycle, ct);
        return BillingCycleMapper.ToResponse(billingCycle);
    }

    public async Task<BillingCycleResponse?> UpdateAsync(Guid id, UpdateBillingCycleRequest request, CancellationToken ct = default)
    {
        var billingCycle = await _repository.GetByIdAsync(id, ct);
        if (billingCycle is null)
        {
            return null;
        }

        BillingCycleMapper.UpdateEntity(billingCycle, request);
        await _repository.UpdateAsync(billingCycle, ct);
        return BillingCycleMapper.ToResponse(billingCycle);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default) => _repository.DeleteAsync(id, ct);
}
