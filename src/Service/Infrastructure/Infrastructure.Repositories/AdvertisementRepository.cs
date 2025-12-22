using Application.Services.DTO;
using Application.Services.Enums;
using Application.Services.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AdvertisementRepository: IAdvertisementRepository
{
    private readonly TestForDexCompanyDbContext _dbContext;

    public AdvertisementRepository(TestForDexCompanyDbContext dbContext)
    {
        _dbContext = dbContext;
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

        var fullQuery = query
            .OrderByDescending(x => x.Rating)
            .Select((x, index) => new
            {
                Ad = x,
                Priority = index < 3 ? 0 : 1
            });

        if (request.SortBy.HasValue)
        {
            fullQuery = request.SortBy.Value switch
            {
                AdvertisementSortBy.Rating => request.SortDesc
                    ? fullQuery.OrderBy(x => x.Priority).ThenByDescending(x => x.Ad.Rating)
                    : fullQuery.OrderBy(x => x.Priority).ThenBy(x => x.Ad.Rating),

                AdvertisementSortBy.StartDate => request.SortDesc
                    ? fullQuery.OrderBy(x => x.Priority).ThenByDescending(x => x.Ad.StartDate)
                    : fullQuery.OrderBy(x => x.Priority).ThenBy(x => x.Ad.StartDate),

                AdvertisementSortBy.EndDate => request.SortDesc
                    ? fullQuery.OrderBy(x => x.Priority).ThenByDescending(x => x.Ad.EndDate)
                    : fullQuery.OrderBy(x => x.Priority).ThenBy(x => x.Ad.EndDate),

                AdvertisementSortBy.Number => request.SortDesc
                    ? fullQuery.OrderBy(x => x.Priority).ThenByDescending(x => x.Ad.Number)
                    : fullQuery.OrderBy(x => x.Priority).ThenBy(x => x.Ad.Number),

                _ => fullQuery.OrderBy(x => x.Priority).ThenByDescending(x => x.Ad.StartDate)
            };
        }
        else
        {
            fullQuery = fullQuery.OrderBy(x => x.Priority).ThenByDescending(x => x.Ad.StartDate);
        }

        if (request.PageNumber > 0 && request.PageSize > 0)
        {
            fullQuery = fullQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);
        }

        return await fullQuery.Select(x => x.Ad).ToListAsync();
    }
}