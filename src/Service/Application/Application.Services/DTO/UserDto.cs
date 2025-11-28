namespace Application.Services.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<AdvertisementDto> Advertisements { get; set; }
}