namespace Application.Services.DTO;

public class AdvertisementDto
{
    /// <summary>
    /// Уникальный идентификатор объявления.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Основной текст объявления.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Имя изображения объявления.
    /// </summary>
    public required string Image { get; init; }

    /// <summary>
    /// Дата создания объявления.
    /// </summary>
    public DateTime StartDate { get; init; }

    /// <summary>
    /// Дата окончания действия объявления.
    /// </summary>
    public DateTime EndDate { get; init; }

    /// <summary>
    /// Порядковый номер объявления.
    /// </summary>
    public int Number { get; init; }

    /// <summary>
    /// Рейтинг объявления.
    /// </summary>
    public int Rating { get; init; }
}
