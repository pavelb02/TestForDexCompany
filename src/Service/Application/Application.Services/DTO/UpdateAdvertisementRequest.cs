namespace Application.Services.DTO;

public class UpdateAdvertisementRequest
{
    public required string Text { get; init; }
    public required string Image { get; set; }
    public DateTime EndDate { get; init; }
}