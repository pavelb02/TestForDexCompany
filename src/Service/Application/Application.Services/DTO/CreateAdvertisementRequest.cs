namespace Application.Services.DTO;

public class CreateAdvertisementRequest
{
    public string Text { get; set; }
    public string Image { get; set; }
    public DateTime EndDate { get; set; }
}
