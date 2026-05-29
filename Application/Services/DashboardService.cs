using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Common;
using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IRepository<Book> _bookRepo;
    private readonly IRepository<Member> _memberRepo;
    private readonly IRepository<Borrow> _borrowRepo;

    public DashboardService(IRepository<Book> bookRepo, IRepository<Member> memberRepo, IRepository<Borrow> borrowRepo)
    {
        _bookRepo = bookRepo;
        _memberRepo = memberRepo;
        _borrowRepo = borrowRepo;
    }

    public async Task<Result<DashboardStatistics>> GetDashboardStatsAsync()
    {
        var totalBooks = await _bookRepo.Query().CountAsync();
        var totalMembers = await _memberRepo.Query().CountAsync();
        var activeBorrows = await _borrowRepo.Query().CountAsync(b => b.ReturnDate == null);
        var totalRevenue = await _bookRepo.Query().SumAsync(b => b.Price);

        return Result<DashboardStatistics>.Success(new DashboardStatistics(totalBooks, totalMembers, activeBorrows, totalRevenue));
    }
}
