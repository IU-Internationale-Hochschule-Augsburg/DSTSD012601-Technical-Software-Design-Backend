using Subscription_Control_Backend.Domain.Enums;

namespace Subscription_Control_Backend.Contracts.Requests.BillingCycles;

public class CreateBillingCycleRequest
{
    public IntervalType IntervalTypeValue { get; set; } = IntervalType.Month;
    public int IntervalValue { get; set; } = 1;
}
