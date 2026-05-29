using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Common;
using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IRepository<Book> _bookRepo;
    private readonly IRepository<Borrow> _borrowRepo;

    public BookService(IRepository<Book> bookRepo, IRepository<Borrow> borrowRepo)
    {
        _bookRepo = bookRepo;
        _borrowRepo = borrowRepo;
    }

    public async Task<Result<PagedResult<BookDto>>> GetBooksAsync(BookFilterParams @params)
    {
        var query = _bookRepo.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(@params.SearchTerm))
        {
            var search = @params.SearchTerm.ToLower();
            query = query.Where(b => b.Title.ToLower().Contains(search) || b.Author.ToLower().Contains(search));
        }

        if (@params.CategoryId.HasValue)
            query = query.Where(b => b.CategoryId == @params.CategoryId);

        if (@params.MinPrice.HasValue)
            query = query.Where(b => b.Price >= @params.MinPrice);

        if (@params.MaxPrice.HasValue)
            query = query.Where(b => b.Price <= @params.MaxPrice);

        var total = await query.CountAsync();
        var items = await query.Skip((@params.Page - 1) * @params.PageSize)
            .Take(@params.PageSize)
            .Select(b => new BookDto(b.Id, b.Title, b.Author, b.Price, b.PublishedYear, b.CategoryId))
            .ToListAsync();

        return Result<PagedResult<BookDto>>.Success(new PagedResult<BookDto>
        {
            Items = items,
            PageNumber = @params.Page,
            PageSize = @params.PageSize,
            TotalCount = total
        });
    }

    public async Task<Result<BookDto>> GetByIdAsync(int id)
    {
        var book = await _bookRepo.GetByIdAsync(id);
        if (book == null) return Result<BookDto>.Failure("Книга не найдена");
        return Result<BookDto>.Success(new BookDto(book.Id, book.Title, book.Author, book.Price, book.PublishedYear, book.CategoryId));
    }

    public async Task<Result<BookDto>> CreateAsync(CreateBookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            Price = dto.Price,
            PublishedYear = dto.PublishedYear,
            CategoryId = dto.CategoryId
        };
        await _bookRepo.AddAsync(book);
        await _bookRepo.SaveChangesAsync();
        return Result<BookDto>.Success(new BookDto(book.Id, book.Title, book.Author, book.Price, book.PublishedYear, book.CategoryId));
    }

    public async Task<Result<bool>> UpdateAsync(int id, CreateBookDto dto)
    {
        var book = await _bookRepo.GetByIdAsync(id);
        if (book == null) return Result<bool>.Failure("Книга не найдена");

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.Price = dto.Price;
        book.PublishedYear = dto.PublishedYear;
        book.CategoryId = dto.CategoryId;

        _bookRepo.Update(book);
        await _bookRepo.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var book = await _bookRepo.GetByIdAsync(id);
        if (book == null) return Result<bool>.Failure("Книга не найдена");
        _bookRepo.Delete(book);
        await _bookRepo.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<BookStatistics>> GetStatisticsAsync()
    {
        var totalBooks = await _bookRepo.Query().CountAsync();
        var avgPrice = totalBooks > 0 ? await _bookRepo.Query().AverageAsync(b => b.Price) : 0;
        var totalBorrows = await _borrowRepo.Query().CountAsync();
        return Result<BookStatistics>.Success(new BookStatistics(totalBooks, avgPrice, totalBorrows));
    }

    public async Task<Result<IEnumerable<TopBorrowedBookDto>>> GetTopBorrowedAsync()
    {
        var top = await _borrowRepo.Query()
            .GroupBy(b => b.Book.Title)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new TopBorrowedBookDto(g.Key, g.Count()))
            .ToListAsync();
        return Result<IEnumerable<TopBorrowedBookDto>>.Success(top);
    }

    public async Task<Result<BookDetailsDto>> GetDetailsAsync(int id)
    {
        var book = await _bookRepo.Query()
            .Include(b => b.Category)
            .Include(b => b.Borrows)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null) return Result<BookDetailsDto>.Failure("Книга не найдена");

        var dto = new BookDetailsDto(
            book.Id,
            book.Title,
            book.Category.Name,
            book.Borrows.Select(b => new BorrowDto(b.Id, b.BookId, b.MemberId, b.BorrowDate, b.ReturnDate)).ToList()
        );
        return Result<BookDetailsDto>.Success(dto);
    }
}
