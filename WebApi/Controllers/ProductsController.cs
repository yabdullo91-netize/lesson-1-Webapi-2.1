using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await productService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await productService.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto) => Ok(await productService.CreateAsync(dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => Ok(await productService.DeleteAsync(id));
}
