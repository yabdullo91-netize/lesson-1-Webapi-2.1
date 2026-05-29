using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;

namespace API.Controllers;

public class BooksController : BaseApiController
{
    private readonly IBookService _service;
    public BooksController(IBookService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult> GetBooks([FromQuery] BookFilterParams @params) => HandleResult(await _service.GetBooksAsync(@params));

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id) => HandleResult(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult> Create(CreateBookDto dto) => HandleResult(await _service.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateBookDto dto) => HandleResult(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) => HandleResult(await _service.DeleteAsync(id));

    [HttpGet("statistics")]
    public async Task<ActionResult> GetStatistics() => HandleResult(await _service.GetStatisticsAsync());

    [HttpGet("top-borrowed")]
    public async Task<ActionResult> GetTopBorrowed() => HandleResult(await _service.GetTopBorrowedAsync());

    [HttpGet("details/{id}")]
    public async Task<ActionResult> GetDetails(int id) => HandleResult(await _service.GetDetailsAsync(id));
}
