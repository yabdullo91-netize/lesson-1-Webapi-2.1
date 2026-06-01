using Application.Common.Responses;
using Application.DTOs;
using Application.Interfaces;
using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ProductService(IApplicationDbContext context) : IProductService
{
    public async Task<BaseResponse<List<ProductDto>>> GetAllAsync()
    {
        var products = await context.Products
            .Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price, CategoryId = p.CategoryId })
            .ToListAsync();
        return BaseResponse<List<ProductDto>>.SuccessResponse(products);
    }

    public async Task<BaseResponse<ProductDto>> GetByIdAsync(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return BaseResponse<ProductDto>.FailureResponse("Product not found");

        return BaseResponse<ProductDto>.SuccessResponse(new ProductDto 
        { 
            Id = product.Id, 
            Name = product.Name, 
            Price = product.Price, 
            CategoryId = product.CategoryId 
        });
    }

    public async Task<BaseResponse<bool>> CreateAsync(CreateProductDto dto)
    {
        var product = new Product 
        { 
            Name = dto.Name, 
            Price = dto.Price, 
            CategoryId = dto.CategoryId 
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return BaseResponse<bool>.SuccessResponse(true, "Product created");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return BaseResponse<bool>.FailureResponse("Product not found");

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return BaseResponse<bool>.SuccessResponse(true, "Product deleted");
    }
}
