using Subscription_Control_Backend.Contracts.Requests.Categories;
using Subscription_Control_Backend.Contracts.Responses.Categories;

namespace Subscription_Control_Backend.Application.Interfaces;

public interface ICategoryService : ICrudService<CategoryResponse, CreateCategoryRequest, UpdateCategoryRequest>
{
}
