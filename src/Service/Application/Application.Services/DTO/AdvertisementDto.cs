namespace Application.Services.DTO;

public class AdvertisementDto
{
    public Guid Id { get; init; }
    public required string Text { get; init; }
    public required string Image { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int Number { get; init; }
    public int Rating { get; init; }
}