using Subscription_Control_Backend.Contracts.Responses.BillingCycles;
using Subscription_Control_Backend.Contracts.Responses.Categories;

namespace Subscription_Control_Backend.Contracts.Responses.Subscriptions;

public class SubscriptionResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public CategoryResponse? Category { get; set; }
    public decimal Cost { get; set; }
    public string Currency { get; set; } = "EUR";
    public Guid BillingCycleId { get; set; }
    public BillingCycleResponse? BillingCycle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CancellationDeadline { get; set; }
    public bool AutoRenew { get; set; }
    public string Notes { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public decimal MonthlyCost { get; set; }
    public DateTime NextPaymentDate { get; set; }
    public DateTime CancellationDate { get; set; }
}
