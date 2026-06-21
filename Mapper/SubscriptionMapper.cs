using Subscription_Control_Backend.Contracts.Requests.Subscriptions;
using Subscription_Control_Backend.Contracts.Responses.Subscriptions;
using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Mapper;

public static class SubscriptionMapper
{
    public static SubscriptionResponse ToResponse(Subscription subscription) => new()
    {
        Id = subscription.Id,
        UserId = subscription.UserId,
        Name = subscription.Name,
        Provider = subscription.Provider,
        CategoryId = subscription.CategoryId,
        Category = subscription.Category is null ? null : CategoryMapper.ToResponse(subscription.Category),
        Cost = subscription.Cost,
        Currency = subscription.Currency,
        BillingCycleId = subscription.BillingCycleId,
        BillingCycle = subscription.BillingCycle is null ? null : BillingCycleMapper.ToResponse(subscription.BillingCycle),
        StartDate = subscription.StartDate,
        EndDate = subscription.EndDate,
        CancellationDeadline = subscription.CancellationDeadline,
        AutoRenew = subscription.AutoRenew,
        Notes = subscription.Notes,
        IsActive = subscription.IsActive,
        MonthlyCost = subscription.CalculateMonthlyCost(),
        NextPaymentDate = subscription.CalculateNextPaymentDate(),
        CancellationDate = subscription.CalculateCancellationDate()
    };

    public static Subscription ToEntity(CreateSubscriptionRequest request) => new()
    {
        Id = Guid.NewGuid(),
        UserId = request.UserId,
        Name = request.Name.Trim(),
        Provider = request.Provider.Trim(),
        CategoryId = request.CategoryId,
        Cost = request.Cost,
        Currency = request.Currency.Trim().ToUpperInvariant(),
        BillingCycleId = request.BillingCycleId,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        CancellationDeadline = request.CancellationDeadline,
        AutoRenew = request.AutoRenew,
        Notes = request.Notes.Trim(),
        IsActive = request.IsActive
    };

    public static void UpdateEntity(Subscription subscription, UpdateSubscriptionRequest request)
    {
        subscription.UserId = request.UserId;
        subscription.Name = request.Name.Trim();
        subscription.Provider = request.Provider.Trim();
        subscription.CategoryId = request.CategoryId;
        subscription.Cost = request.Cost;
        subscription.Currency = request.Currency.Trim().ToUpperInvariant();
        subscription.BillingCycleId = request.BillingCycleId;
        subscription.StartDate = request.StartDate;
        subscription.EndDate = request.EndDate;
        subscription.CancellationDeadline = request.CancellationDeadline;
        subscription.AutoRenew = request.AutoRenew;
        subscription.Notes = request.Notes.Trim();
        subscription.IsActive = request.IsActive;
    }
}
