using Subscription_Control_Backend.Domain.Enums;

namespace Subscription_Control_Backend.Domain.Entities;

public class BillingCycle
{
    public Guid Id { get; set; }
    public IntervalType IntervalTypeValue { get; set; }
    public int IntervalValue { get; set; } = 1;
    public List<Subscription> Subscriptions { get; set; } = [];

    public DateTime GetNextDate(DateTime from)
    {
        if (IntervalValue <= 0)
        {
            throw new InvalidOperationException("IntervalValue must be greater than zero.");
        }

        return IntervalTypeValue switch
        {
            IntervalType.Day => from.AddDays(IntervalValue),
            IntervalType.Week => from.AddDays(7 * IntervalValue),
            IntervalType.Month => from.AddMonths(IntervalValue),
            IntervalType.Year => from.AddYears(IntervalValue),
            _ => throw new ArgumentOutOfRangeException(nameof(IntervalTypeValue), IntervalTypeValue, null)
        };
    }
}
