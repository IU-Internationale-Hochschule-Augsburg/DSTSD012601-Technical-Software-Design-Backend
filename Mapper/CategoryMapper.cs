using Subscription_Control_Backend.Contracts.Requests.Categories;
using Subscription_Control_Backend.Contracts.Responses.Categories;
using Subscription_Control_Backend.Domain.Entities;

namespace Subscription_Control_Backend.Mapper;

public static class CategoryMapper
{
    public static CategoryResponse ToResponse(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Icon = category.Icon
    };

    public static Category ToEntity(CreateCategoryRequest request) => new()
    {
        Id = Guid.NewGuid(),
        Name = request.Name.Trim(),
        Icon = request.Icon.Trim()
    };

    public static void UpdateEntity(Category category, UpdateCategoryRequest request)
    {
        category.Name = request.Name.Trim();
        category.Icon = request.Icon.Trim();
    }
}
