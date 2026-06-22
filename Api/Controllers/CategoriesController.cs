using Microsoft.AspNetCore.Mvc;
using Subscription_Control_Backend.Api.Mapper;
using Subscription_Control_Backend.Application.Interfaces;
using Subscription_Control_Backend.Contracts.Requests.Categories;

namespace Subscription_Control_Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(ApiResponseMapper.ToResponse(true, await _service.GetAllAsync(ct), "Kategorien geladen.", string.Empty));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Kategorie nicht gefunden.")) : Ok(ApiResponseMapper.ToResponse(true, result, "Kategorie geladen.", string.Empty));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken ct)
    {
        var result = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponseMapper.ToResponse(true, result, "Kategorie erstellt.", string.Empty));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken ct)
    {
        var result = await _service.UpdateAsync(id, request, ct);
        return result is null ? NotFound(ApiResponseMapper.ToResponse(false, (object?)null, string.Empty, "Kategorie nicht gefunden.")) : Ok(ApiResponseMapper.ToResponse(true, result, "Kategorie aktualisiert.", string.Empty));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(id, ct);
        return deleted ? Ok(ApiResponseMapper.ToResponse(true, id, "Kategorie gelöscht.", string.Empty)) : NotFound(ApiResponseMapper.ToResponse(false, id, string.Empty, "Kategorie nicht gefunden."));
    }
}
