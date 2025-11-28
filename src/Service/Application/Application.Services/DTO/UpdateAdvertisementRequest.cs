namespace Application.Services.DTO;

public class UpdateAdvertisementRequest
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public string PathImage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}