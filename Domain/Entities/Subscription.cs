using Subscription_Control_Backend.Domain.Enums;

namespace Subscription_Control_Backend.Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public decimal Cost { get; set; }
    public string Currency { get; set; } = "EUR";
    public Guid BillingCycleId { get; set; }
    public BillingCycle? BillingCycle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CancellationDeadline { get; set; }
    public bool AutoRenew { get; set; } = true;
    public string Notes { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public List<Notification> Notifications { get; set; } = [];

    public decimal CalculateMonthlyCost()
    {
        if (BillingCycle is null || BillingCycle.IntervalValue <= 0)
        {
            return Cost;
        }

        return BillingCycle.IntervalTypeValue switch
        {
            IntervalType.Month => Cost / BillingCycle.IntervalValue,
            IntervalType.Year => Cost / (12 * BillingCycle.IntervalValue),
            IntervalType.Week => Cost * 52 / 12 / BillingCycle.IntervalValue,
            IntervalType.Day => Cost * 365 / 12 / BillingCycle.IntervalValue,
            _ => Cost
        };
    }

    public DateTime CalculateNextPaymentDate()
    {
        if (BillingCycle is null)
        {
            return StartDate;
        }

        var nextDate = DateTime.SpecifyKind(StartDate, DateTimeKind.Utc);
        while (nextDate <= DateTime.UtcNow)
        {
            nextDate = BillingCycle.GetNextDate(nextDate);
        }

        return nextDate;
    }

    public DateTime CalculateCancellationDate()
    {
        return CancellationDeadline ?? CalculateNextPaymentDate();
    }
}
