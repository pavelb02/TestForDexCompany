using Application.Services.Enums;

namespace Application.Services.DTO;

public class AdvertisementSearchRequest
{
    /// <summary>
    /// Идентификатор пользователя (фильтр объявлений конкретного пользователя).
    /// </summary>
    public Guid? UserId { get; init; }

    /// <summary>
    /// Минимальный рейтинг объявления.
    /// </summary>
    public int? MinRating { get; init; }

    /// <summary>
    /// Максимальный рейтинг объявления.
    /// </summary>
    public int? MaxRating { get; init; }

    /// <summary>
    /// Текст объявления.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// Начальная дата создания.
    /// </summary>
    public DateTime? StartDateFrom { get; init; }

    /// <summary>
    /// Конечная дата создания.
    /// </summary>
    public DateTime? StartDateTo { get; init; }

    /// <summary>
    /// Начальная дата окончания.
    /// </summary>
    public DateTime? EndDateFrom { get; init; }

    /// <summary>
    /// Конечная дата окончания.
    /// </summary>
    public DateTime? EndDateTo { get; init; }

    /// <summary>
    /// Поле, по которому выполняется сортировка.
    /// </summary>
    public AdvertisementSortBy? SortBy { get; init; }

    /// <summary>
    /// Признак сортировки по убыванию.
    /// </summary>
    public bool SortDesc { get; init; } = true;

    /// <summary>
    /// Номер страницы.
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Количество элементов на странице.
    /// </summary>
    public int PageSize { get; init; } = 10;
}
