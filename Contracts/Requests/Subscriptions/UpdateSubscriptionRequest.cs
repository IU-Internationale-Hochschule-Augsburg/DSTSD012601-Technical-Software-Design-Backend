namespace Subscription_Control_Backend.Contracts.Requests.Subscriptions;

public class UpdateSubscriptionRequest
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public decimal Cost { get; set; }
    public string Currency { get; set; } = "EUR";
    public Guid BillingCycleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CancellationDeadline { get; set; }
    public bool AutoRenew { get; set; } = true;
    public string Notes { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
