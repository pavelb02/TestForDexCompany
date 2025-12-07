namespace Application.Services.DTO;

public class UserDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public List<AdvertisementDto>? Advertisements { get; init; }
}