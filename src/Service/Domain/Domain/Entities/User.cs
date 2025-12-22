using Shared.Domain.Exceptions;

namespace Domain.Entities;

public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Коллекция объявлений пользователя
    /// </summary>

    private readonly List<Advertisement> _advertisements = new();
    public IReadOnlyCollection<Advertisement> Advertisements => _advertisements.AsReadOnly();

    protected User()
    {
    }

    public User(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    /// <summary>
    /// Обновить сущность
    /// </summary>
    public void Update(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Добавить объявление
    /// </summary>
    public void AddAdvertisement(string text, string pathName, DateTime endDate)
    {
        var advertisement = new Advertisement(Id, text, pathName, endDate);
        _advertisements.Add(advertisement);
    }

    /// <summary>
    /// Обновить объявление
    /// </summary>
    public void UpdateAdvertisement(Guid advertisementId, string text, string pathImage, DateTime endDate)
    {
        var advertisement = _advertisements.FirstOrDefault(x => x.Id == advertisementId);
        if (advertisement == null)
            throw new EntityNotFoundException($"Объявление с Id {advertisementId} отсутствует.");

        advertisement.Update(text, pathImage, endDate);
    }

    /// <summary>
    /// Удалить объявление
    /// </summary>
    public void RemoveAdvertisement(Guid advertisementId)
    {
        var advertisement = _advertisements.FirstOrDefault(x => x.Id == advertisementId);
        if (advertisement == null)
            throw new EntityNotFoundException($"Объявление с Id {advertisementId} отсутствует.");

        _advertisements.Remove(advertisement);
    }

    /// <summary>
    /// Получить объявление
    /// </summary>
    public Advertisement GetAdvertisement(Guid advertisementId)
    {
        var advertisement = _advertisements.FirstOrDefault(x => x.Id == advertisementId);
        if (advertisement == null)
            throw new EntityNotFoundException($"Объявление с Id {advertisementId} отсутствует.");

        return advertisement;
    }
}