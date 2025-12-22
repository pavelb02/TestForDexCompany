using Application.Services.DTO;

namespace Application.Services.Interfaces;

public interface IAdvertisementService
{
    /// <summary>
    /// Обрабатывает запрос на поиск объявлений и возвращает результат в виде DTO.
    /// </summary>
    Task<List<AdvertisementDto>> SearchAsync(AdvertisementSearchRequest request);
}