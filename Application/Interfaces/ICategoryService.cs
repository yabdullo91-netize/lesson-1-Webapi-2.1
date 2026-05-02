using Application.Common.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface ICategoryService
{
    Task<BaseResponse<List<CategoryDto>>> GetAllAsync();
    Task<BaseResponse<CategoryDto>> GetByIdAsync(int id);
    Task<BaseResponse<bool>> CreateAsync(CreateCategoryDto dto);
    Task<BaseResponse<bool>> DeleteAsync(int id);
}
