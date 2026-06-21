using Subscription_Control_Backend.Contracts.Requests.BillingCycles;
using Subscription_Control_Backend.Contracts.Responses.BillingCycles;
using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Mapper;

public static class BillingCycleMapper
{
    public static BillingCycleResponse ToResponse(BillingCycle billingCycle) => new()
    {
        Id = billingCycle.Id,
        IntervalTypeValue = billingCycle.IntervalTypeValue,
        IntervalValue = billingCycle.IntervalValue
    };

    public static BillingCycle ToEntity(CreateBillingCycleRequest request) => new()
    {
        Id = Guid.NewGuid(),
        IntervalTypeValue = request.IntervalTypeValue,
        IntervalValue = request.IntervalValue
    };

    public static void UpdateEntity(BillingCycle billingCycle, UpdateBillingCycleRequest request)
    {
        billingCycle.IntervalTypeValue = request.IntervalTypeValue;
        billingCycle.IntervalValue = request.IntervalValue;
    }
}
