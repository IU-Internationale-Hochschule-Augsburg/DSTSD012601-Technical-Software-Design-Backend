namespace Subscription_Control_Backend.Contracts.Requests.Categories;

public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}
