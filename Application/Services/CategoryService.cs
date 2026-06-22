using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.Categories;
using Subscription_Control_Backend.Contracts.Responses.Categories;
using Subscription_Control_Backend.Domain.Entities;
using Subscription_Control_Backend.Domain.Interfaces;
using Subscription_Control_Backend.Mapper;

namespace Subscription_Control_Backend.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _repository;

    public CategoryService(IRepository<Category> repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoryResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var categories = await _repository.GetAllAsync(ct);
        return categories.Select(CategoryMapper.ToResponse).ToList();
    }

    public async Task<CategoryResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var category = await _repository.GetByIdAsync(id, ct);
        return category is null ? null : CategoryMapper.ToResponse(category);
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default)
    {
        var category = CategoryMapper.ToEntity(request);
        await _repository.AddAsync(category, ct);
        return CategoryMapper.ToResponse(category);
    }

    public async Task<CategoryResponse?> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken ct = default)
    {
        var category = await _repository.GetByIdAsync(id, ct);
        if (category is null)
        {
            return null;
        }

        CategoryMapper.UpdateEntity(category, request);
        await _repository.UpdateAsync(category, ct);
        return CategoryMapper.ToResponse(category);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default) => _repository.DeleteAsync(id, ct);
}
