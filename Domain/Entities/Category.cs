namespace Subscription_Control_Backend.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public List<Subscription> Subscriptions { get; set; } = [];
}
