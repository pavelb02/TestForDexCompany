using Ardalis.GuardClauses;
using Shared.Domain.Exceptions;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private readonly List<Advertisement> _advertisements = [];
    public IReadOnlyCollection<Advertisement> Advertisements => _advertisements.AsReadOnly();

    protected User()
    {
    }

    protected User(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public void AddAdvertisement(Advertisement ad, int maxAdvertisements)
    {
        Guard.Against.Null(ad, nameof(ad));

        if (_advertisements.Count >= maxAdvertisements)
            throw new InvalidOperationException("Пользователь не может иметь больше чем {maxAdvertisements} объявлений.");
        _advertisements.Add(ad);
    }

    public void UpdateAdvertisement(Guid advertisementId, string text, string pathImage, DateTime endDate)
    {
        var advertisement = _advertisements.FirstOrDefault(x => x.Id == Id);
        if (advertisement == null)
            throw new EntityNotFoundException("Объявления с таким Id отсутствует.");
        
        advertisement.Update(text, pathImage, endDate);
    }

    public void RemoveAdvertisement(Guid advertisementId)
    {
        var advertisement = _advertisements.FirstOrDefault(x => x.Id == Id);
        if (advertisement == null)
            throw new EntityNotFoundException("Объявления с таким Id отсутствует.");

        _advertisements.Remove(advertisement);
    }

    public List<Advertisement> GetAllAdvertisements()
    {
        var advertisementList = _advertisements.ToList();
        return advertisementList;
    }
}