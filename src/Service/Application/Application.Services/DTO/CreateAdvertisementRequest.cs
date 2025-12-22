namespace Application.Services.DTO;

public class CreateAdvertisementRequest
{
    /// <summary>
    /// Текст объявления.
    /// </summary>
    public required string Text { get; init; }
    
    /// <summary>
    /// Имя изображения.
    /// </summary>
    public required string Image { get; set; }
    
    /// <summary>
    /// Дата окончания действия объявления.
    /// </summary>
    public DateTime EndDate { get; init; }
}
