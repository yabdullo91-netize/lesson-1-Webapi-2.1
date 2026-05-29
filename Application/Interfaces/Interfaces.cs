using Domain.Entities;
using Application.DTOs;
using Application.Common;

namespace Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
    IQueryable<T> Query();
}

public interface IBookService
{
    Task<Result<PagedResult<BookDto>>> GetBooksAsync(BookFilterParams @params);
    Task<Result<BookDto>> GetByIdAsync(int id);
    Task<Result<BookDto>> CreateAsync(CreateBookDto dto);
    Task<Result<bool>> UpdateAsync(int id, CreateBookDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<BookStatistics>> GetStatisticsAsync();
    Task<Result<IEnumerable<TopBorrowedBookDto>>> GetTopBorrowedAsync();
    Task<Result<BookDetailsDto>> GetDetailsAsync(int id);
}

public interface ICategoryService
{
    Task<Result<IEnumerable<CategoryDto>>> GetAllAsync();
    Task<Result<CategoryDto>> GetByIdAsync(int id);
    Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto);
    Task<Result<bool>> UpdateAsync(int id, CreateCategoryDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<IEnumerable<CategoryWithBooksDto>>> GetWithBooksAsync();
}

public interface IMemberService
{
    Task<Result<IEnumerable<MemberDto>>> GetAllAsync();
    Task<Result<MemberDto>> GetByIdAsync(int id);
    Task<Result<MemberDto>> CreateAsync(CreateMemberDto dto);
    Task<Result<bool>> UpdateAsync(int id, CreateMemberDto dto);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<IEnumerable<TopReaderDto>>> GetTopReadersAsync();
}

public interface IBorrowService
{
    Task<Result<IEnumerable<BorrowDto>>> GetAllAsync();
    Task<Result<BorrowDto>> GetByIdAsync(int id);
    Task<Result<BorrowDto>> CreateAsync(CreateBorrowDto dto);
    Task<Result<bool>> ReturnBookAsync(int id);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<IEnumerable<ActiveBorrowDto>>> GetActiveAsync();
    Task<Result<IEnumerable<BorrowDto>>> GetHistoryByMemberAsync(int memberId);
    Task<Result<IEnumerable<MonthlyBorrowDto>>> GetMonthlyReportAsync();
}

public interface IDashboardService
{
    Task<Result<DashboardStatistics>> GetDashboardStatsAsync();
}
