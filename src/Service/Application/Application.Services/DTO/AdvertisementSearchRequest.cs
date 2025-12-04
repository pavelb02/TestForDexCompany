namespace Application.Services.DTO;

public class AdvertisementSearchRequest
{ 
    public Guid? UserId { get; set; }
    public int? MinRating { get; set; }
    public int? MaxRating { get; set; }

    public string? Text { get; set; }
    
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public DateTime? EndDateFrom { get; set; }
    public DateTime? EndDateTo { get; set; }
    
    public string? SortBy { get; set; } = "StartDate";
    public bool SortDesc { get; set; } = true;
    
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}