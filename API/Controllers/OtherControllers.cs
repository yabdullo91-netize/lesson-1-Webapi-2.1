using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;

namespace API.Controllers;

public class CategoriesController : BaseApiController
{
    private readonly ICategoryService _service;
    public CategoriesController(ICategoryService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult> GetAll() => HandleResult(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id) => HandleResult(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult> Create(CreateCategoryDto dto) => HandleResult(await _service.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateCategoryDto dto) => HandleResult(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) => HandleResult(await _service.DeleteAsync(id));

    [HttpGet("with-books")]
    public async Task<ActionResult> GetWithBooks() => HandleResult(await _service.GetWithBooksAsync());
}

public class MembersController : BaseApiController
{
    private readonly IMemberService _service;
    public MembersController(IMemberService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult> GetAll() => HandleResult(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id) => HandleResult(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult> Create(CreateMemberDto dto) => HandleResult(await _service.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateMemberDto dto) => HandleResult(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) => HandleResult(await _service.DeleteAsync(id));

    [HttpGet("top-readers")]
    public async Task<ActionResult> GetTopReaders() => HandleResult(await _service.GetTopReadersAsync());
}

public class BorrowsController : BaseApiController
{
    private readonly IBorrowService _service;
    public BorrowsController(IBorrowService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult> GetAll() => HandleResult(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id) => HandleResult(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult> Create(CreateBorrowDto dto) => HandleResult(await _service.CreateAsync(dto));

    [HttpPut("{id}/return")]
    public async Task<ActionResult> Return(int id) => HandleResult(await _service.ReturnBookAsync(id));

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id) => HandleResult(await _service.DeleteAsync(id));

    [HttpGet("active")]
    public async Task<ActionResult> GetActive() => HandleResult(await _service.GetActiveAsync());

    [HttpGet("history")]
    public async Task<ActionResult> GetHistory([FromQuery] int memberId) => HandleResult(await _service.GetHistoryByMemberAsync(memberId));

    [HttpGet("monthly-borrows")]
    public async Task<ActionResult> GetMonthlyReport() => HandleResult(await _service.GetMonthlyReportAsync());
}

public class DashboardController : BaseApiController
{
    private readonly IDashboardService _service;
    public DashboardController(IDashboardService service) => _service = service;

    [HttpGet("statistics")]
    public async Task<ActionResult> GetStats() => HandleResult(await _service.GetDashboardStatsAsync());
}
