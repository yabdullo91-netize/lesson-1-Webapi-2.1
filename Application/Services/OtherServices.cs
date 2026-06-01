using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Common;
using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class LibraryCategoryService : ILibraryCategoryService
{
    private readonly IRepository<Category> _repo;
    public LibraryCategoryService(IRepository<Category> repo) => _repo = repo;

    public async Task<Result<IEnumerable<LibraryCategoryDto>>> GetAllAsync()
    {
        var items = await _repo.Query()
            .Select(c => new LibraryCategoryDto(c.Id, c.Name, c.Description))
            .ToListAsync();
        return Result<IEnumerable<LibraryCategoryDto>>.Success(items);
    }

    public async Task<Result<LibraryCategoryDto>> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return Result<LibraryCategoryDto>.Failure("Категория не найдена");
        return Result<LibraryCategoryDto>.Success(new LibraryCategoryDto(c.Id, c.Name, c.Description));
    }

    public async Task<Result<LibraryCategoryDto>> CreateAsync(CreateLibraryCategoryDto dto)
    {
        var c = new Category { Name = dto.Name, Description = dto.Description };
        await _repo.AddAsync(c);
        await _repo.SaveChangesAsync();
        return Result<LibraryCategoryDto>.Success(new LibraryCategoryDto(c.Id, c.Name, c.Description));
    }

    public async Task<Result<bool>> UpdateAsync(int id, CreateLibraryCategoryDto dto)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return Result<bool>.Failure("Категория не найдена");
        c.Name = dto.Name;
        c.Description = dto.Description;
        _repo.Update(c);
        await _repo.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return Result<bool>.Failure("Категория не найдена");
        _repo.Delete(c);
        await _repo.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<IEnumerable<CategoryWithBooksDto>>> GetWithBooksAsync()
    {
        var items = await _repo.Query()
            .Include(c => c.Books)
            .Select(c => new CategoryWithBooksDto(c.Id, c.Name, c.Books.Select(b => new BookDto(b.Id, b.Title, b.Author, b.Price, b.PublishedYear, b.CategoryId)).ToList()))
            .ToListAsync();
        return Result<IEnumerable<CategoryWithBooksDto>>.Success(items);
    }
}

public class MemberService : IMemberService
{
    private readonly IRepository<Member> _repo;
    private readonly IRepository<Borrow> _borrowRepo;

    public MemberService(IRepository<Member> repo, IRepository<Borrow> borrowRepo)
    {
        _repo = repo;
        _borrowRepo = borrowRepo;
    }

    public async Task<Result<IEnumerable<MemberDto>>> GetAllAsync()
    {
        var items = await _repo.Query()
            .Select(m => new MemberDto(m.Id, m.FullName, m.Email, m.RegisteredAt))
            .ToListAsync();
        return Result<IEnumerable<MemberDto>>.Success(items);
    }

    public async Task<Result<MemberDto>> GetByIdAsync(int id)
    {
        var m = await _repo.GetByIdAsync(id);
        if (m == null) return Result<MemberDto>.Failure("Член библиотеки не найден");
        return Result<MemberDto>.Success(new MemberDto(m.Id, m.FullName, m.Email, m.RegisteredAt));
    }

    public async Task<Result<MemberDto>> CreateAsync(CreateMemberDto dto)
    {
        var m = new Member { FullName = dto.FullName, Email = dto.Email, RegisteredAt = DateTime.UtcNow };
        await _repo.AddAsync(m);
        await _repo.SaveChangesAsync();
        return Result<MemberDto>.Success(new MemberDto(m.Id, m.FullName, m.Email, m.RegisteredAt));
    }

    public async Task<Result<bool>> UpdateAsync(int id, CreateMemberDto dto)
    {
        var m = await _repo.GetByIdAsync(id);
        if (m == null) return Result<bool>.Failure("Член библиотеки не найден");
        m.FullName = dto.FullName;
        m.Email = dto.Email;
        _repo.Update(m);
        await _repo.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var m = await _repo.GetByIdAsync(id);
        if (m == null) return Result<bool>.Failure("Член библиотеки не найден");
        _repo.Delete(m);
        await _repo.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<IEnumerable<TopReaderDto>>> GetTopReadersAsync()
    {
        var top = await _borrowRepo.Query()
            .GroupBy(b => b.Member.FullName)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => new TopReaderDto(g.Key, g.Count()))
            .ToListAsync();
        return Result<IEnumerable<TopReaderDto>>.Success(top);
    }
}

public class BorrowService : IBorrowService
{
    private readonly IRepository<Borrow> _repo;
    public BorrowService(IRepository<Borrow> repo) => _repo = repo;

    public async Task<Result<IEnumerable<BorrowDto>>> GetAllAsync()
    {
        var items = await _repo.Query()
            .Select(b => new BorrowDto(b.Id, b.BookId, b.MemberId, b.BorrowDate, b.ReturnDate))
            .ToListAsync();
        return Result<IEnumerable<BorrowDto>>.Success(items);
    }

    public async Task<Result<BorrowDto>> GetByIdAsync(int id)
    {
        var b = await _repo.GetByIdAsync(id);
        if (b == null) return Result<BorrowDto>.Failure("Выдача не найдена");
        return Result<BorrowDto>.Success(new BorrowDto(b.Id, b.BookId, b.MemberId, b.BorrowDate, b.ReturnDate));
    }

    public async Task<Result<BorrowDto>> CreateAsync(CreateBorrowDto dto)
    {
        var b = new Borrow { BookId = dto.BookId, MemberId = dto.MemberId, BorrowDate = DateTime.UtcNow };
        await _repo.AddAsync(b);
        await _repo.SaveChangesAsync();
        return Result<BorrowDto>.Success(new BorrowDto(b.Id, b.BookId, b.MemberId, b.BorrowDate, b.ReturnDate));
    }

    public async Task<Result<bool>> ReturnBookAsync(int id)
    {
        var b = await _repo.GetByIdAsync(id);
        if (b == null) return Result<bool>.Failure("Выдача не найдена");
        b.ReturnDate = DateTime.UtcNow;
        _repo.Update(b);
        await _repo.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var b = await _repo.GetByIdAsync(id);
        if (b == null) return Result<bool>.Failure("Выдача не найдена");
        _repo.Delete(b);
        await _repo.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<IEnumerable<ActiveBorrowDto>>> GetActiveAsync()
    {
        var items = await _repo.Query()
            .Where(b => b.ReturnDate == null)
            .Select(b => new ActiveBorrowDto(b.Id, b.Book.Title, b.Member.FullName, b.BorrowDate))
            .ToListAsync();
        return Result<IEnumerable<ActiveBorrowDto>>.Success(items);
    }

    public async Task<Result<IEnumerable<BorrowDto>>> GetHistoryByMemberAsync(int memberId)
    {
        var items = await _repo.Query()
            .Where(b => b.MemberId == memberId)
            .Select(b => new BorrowDto(b.Id, b.BookId, b.MemberId, b.BorrowDate, b.ReturnDate))
            .ToListAsync();
        return Result<IEnumerable<BorrowDto>>.Success(items);
    }

    public async Task<Result<IEnumerable<MonthlyBorrowDto>>> GetMonthlyReportAsync()
    {
        var last6Months = DateTime.UtcNow.AddMonths(-6);
        var report = await _repo.Query()
            .Where(b => b.BorrowDate >= last6Months)
            .GroupBy(b => new { b.BorrowDate.Year, b.BorrowDate.Month })
            .Select(g => new MonthlyBorrowDto($"{g.Key.Year}-{g.Key.Month:D2}", g.Count()))
            .ToListAsync();
        return Result<IEnumerable<MonthlyBorrowDto>>.Success(report);
    }
}
