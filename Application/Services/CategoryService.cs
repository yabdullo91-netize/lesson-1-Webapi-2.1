using Application.Common.Responses;
using Application.DTOs;
using Application.Interfaces;
using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class CategoryService(IApplicationDbContext context) : ICategoryService
{
    public async Task<BaseResponse<List<CategoryDto>>> GetAllAsync()
    {
        var categories = await context.Categories
            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
            .ToListAsync();
        return BaseResponse<List<CategoryDto>>.SuccessResponse(categories);
    }

    public async Task<BaseResponse<CategoryDto>> GetByIdAsync(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null) return BaseResponse<CategoryDto>.FailureResponse("Category not found");
        
        return BaseResponse<CategoryDto>.SuccessResponse(new CategoryDto { Id = category.Id, Name = category.Name });
    }

    public async Task<BaseResponse<bool>> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category { Name = dto.Name };
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return BaseResponse<bool>.SuccessResponse(true, "Category created");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null) return BaseResponse<bool>.FailureResponse("Category not found");

        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return BaseResponse<bool>.SuccessResponse(true, "Category deleted");
    }
}
