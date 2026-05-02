using Application.Common.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface IProductService
{
    Task<BaseResponse<List<ProductDto>>> GetAllAsync();
    Task<BaseResponse<ProductDto>> GetByIdAsync(int id);
    Task<BaseResponse<bool>> CreateAsync(CreateProductDto dto);
    Task<BaseResponse<bool>> DeleteAsync(int id);
}
