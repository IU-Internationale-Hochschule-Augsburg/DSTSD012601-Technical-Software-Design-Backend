using Subscription_Control_Backend.Domain.Enums;

namespace Subscription_Control_Backend.Contracts.Responses.BillingCycles;

public class BillingCycleResponse
{
    public Guid Id { get; set; }
    public IntervalType IntervalTypeValue { get; set; }
    public int IntervalValue { get; set; }
}
