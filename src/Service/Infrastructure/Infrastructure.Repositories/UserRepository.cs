using Application.Services.DTO;
using Application.Services.Enums;
using Application.Services.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TestForDexCompanyDbContext _dbContext;

    public UserRepository(TestForDexCompanyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        return await Task.FromResult(user.Id);
    }

    public async Task<User> GetByIdAsync(Guid userId, bool trackChanges)
    {
        var query = _dbContext.Users.Include(p => p.Advertisements).AsQueryable();

        if (!trackChanges)
            query = query.AsNoTracking();

        var user = await query.FirstOrDefaultAsync(c => c.Id == userId);

        if (user == null)
        {
            throw new EntityNotFoundException($"Пользователь с Id {userId} не найден.");
        }

        return user;
    }

    public Task Delete(User user)
    {
        _dbContext.Users.Remove(user);
        return Task.CompletedTask;
    }

    public async Task<List<Advertisement>> SearchAsync(AdvertisementSearchRequest request)
    {
        IQueryable<Advertisement> query = _dbContext.Advertisements;

        if (request.UserId.HasValue)
            query = query.Where(x => x.UserId == request.UserId);

        if (request.MinRating.HasValue)
            query = query.Where(x => x.Rating >= request.MinRating);

        if (request.MaxRating.HasValue)
            query = query.Where(x => x.Rating <= request.MaxRating);

        if (!string.IsNullOrWhiteSpace(request.Text))
            query = query.Where(x => x.Text.Contains(request.Text));


        if (request.StartDateFrom.HasValue)
            query = query.Where(x => x.StartDate >= request.StartDateFrom);

        if (request.StartDateTo.HasValue)
            query = query.Where(x => x.StartDate <= request.StartDateTo);

        if (request.EndDateFrom.HasValue)
            query = query.Where(x => x.EndDate >= request.EndDateFrom);

        if (request.EndDateTo.HasValue)
            query = query.Where(x => x.EndDate <= request.EndDateTo);

        var topRated = query
            .OrderByDescending(x => x.Rating)
            .Take(3);

        var others = query
            .Where(x => !topRated.Contains(x));

        if (request.SortBy.HasValue)
        {
            others = request.SortBy.Value switch
            {
                AdvertisementSortBy.Rating => request.SortDesc
                    ? others.OrderByDescending(x => x.Rating)
                    : others.OrderBy(x => x.Rating),

                AdvertisementSortBy.StartDate => request.SortDesc
                    ? others.OrderByDescending(x => x.StartDate)
                    : others.OrderBy(x => x.StartDate),

                AdvertisementSortBy.EndDate => request.SortDesc
                    ? others.OrderByDescending(x => x.EndDate)
                    : others.OrderBy(x => x.EndDate),

                AdvertisementSortBy.Number => request.SortDesc
                    ? others.OrderByDescending(x => x.Number)
                    : others.OrderBy(x => x.Number),

                _ => others.OrderByDescending(x => x.StartDate)
            };
        }
        else
        {
            others = others.OrderByDescending(x => x.StartDate);
        }

        var fullQuery = topRated.Concat(others);

        if (request.PageNumber > 0 && request.PageSize > 0)
        {
            fullQuery = fullQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);
        }

        return await fullQuery.ToListAsync();
    }
}