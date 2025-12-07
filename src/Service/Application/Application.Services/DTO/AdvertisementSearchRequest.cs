using Application.Services.Enums;

namespace Application.Services.DTO;

public class AdvertisementSearchRequest
{
    public Guid? UserId { get; init; }
    public int? MinRating { get; init; }
    public int? MaxRating { get; init; }

    public string? Text { get; init; }

    public DateTime? StartDateFrom { get; init; }
    public DateTime? StartDateTo { get; init; }
    public DateTime? EndDateFrom { get; init; }
    public DateTime? EndDateTo { get; init; }

    public AdvertisementSortBy? SortBy { get; init; }

    public bool SortDesc { get; init; } = true;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}