using Application.Services.DTO;
using Domain.Entities;

namespace Application.Services.Interfaces;

public interface IAdvertisementRepository
{
    /// <summary>
    /// Выполняет запрос к хранилищу данных для получения объявлений
    /// в соответствии с заданными параметрами поиска.
    /// </summary>
    Task<List<Advertisement>> SearchAsync(AdvertisementSearchRequest request);
}