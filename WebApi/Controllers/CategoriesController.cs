using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await categoryService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await categoryService.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto) => Ok(await categoryService.CreateAsync(dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Ok(await categoryService.DeleteAsync(id));
}
