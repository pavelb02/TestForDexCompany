namespace Application.Services.DTO;

public class RatingRequest
{
    public Guid UserId { get; set; }
    public Guid AdvertisementId { get; set; }
    public int Rating { get; set; }
}