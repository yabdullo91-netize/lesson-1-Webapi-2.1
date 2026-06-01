using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record BookDto(int Id, string Title, string Author, decimal Price, int PublishedYear, int CategoryId);
public record CreateBookDto(
    [Required][MaxLength(200)] string Title,
    [Required][MaxLength(100)] string Author,
    [Range(0, 1000000)] decimal Price,
    [Range(0, 2100)] int PublishedYear,
    int CategoryId);

public record LibraryCategoryDto(int Id, string Name, string? Description);
public record CreateLibraryCategoryDto([Required][MaxLength(100)] string Name, [MaxLength(500)] string? Description);

public record MemberDto(int Id, string FullName, string Email, DateTime RegisteredAt);
public record CreateMemberDto([Required][MaxLength(100)] string FullName, [Required][EmailAddress] string Email);

public record BorrowDto(int Id, int BookId, int MemberId, DateTime BorrowDate, DateTime? ReturnDate);
public record CreateBorrowDto(int BookId, int MemberId);

public record BookFilterParams(string? SearchTerm, int? CategoryId, decimal? MinPrice, decimal? MaxPrice, int Page = 1, int PageSize = 10);
public record BookStatistics(int TotalBooks, decimal AveragePrice, int TotalBorrows);
public record CategoryWithBooksDto(int CategoryId, string CategoryName, List<BookDto> Books);
public record ActiveBorrowDto(int Id, string BookTitle, string MemberName, DateTime BorrowDate);
public record TopBorrowedBookDto(string BookTitle, int TotalBorrows);
public record MonthlyBorrowDto(string Month, int Count);
public record BookDetailsDto(int Id, string Title, string Category, List<BorrowDto> Borrows);
public record TopReaderDto(string FullName, int TotalBorrows);
public record DashboardStatistics(int TotalBooks, int TotalMembers, int ActiveBorrows, decimal TotalRevenue);
