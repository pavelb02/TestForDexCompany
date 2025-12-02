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

    public User(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public void Update(string name)
    {
        Name = name;
    }

    public void AddAdvertisement(int number, string text, string pathName, DateTime endDate)
    {
        var advertisement = new Advertisement(Id, number, text, pathName, endDate);
        _advertisements.Add(advertisement);
    }

    public void UpdateAdvertisement(Guid advertisementId, string text, string pathImage, DateTime endDate)
    {
        var advertisement = _advertisements.FirstOrDefault(x => x.Id == advertisementId);
        if (advertisement == null)
            throw new EntityNotFoundException($"Объявление с Id {advertisementId} отсутствует.");
        
        advertisement.Update(text, pathImage, endDate);
    }

    public void RemoveAdvertisement(Guid advertisementId)
    {
        var advertisement = _advertisements.FirstOrDefault(x => x.Id == advertisementId);
        if (advertisement == null)
            throw new EntityNotFoundException($"Объявление с Id {advertisementId} отсутствует.");

        _advertisements.Remove(advertisement);
    }

    public Advertisement GetAdvertisement(Guid advertisementId)
    {
        var advertisement = _advertisements.FirstOrDefault(x => x.Id == advertisementId);
        if (advertisement == null)
            throw new EntityNotFoundException($"Объявление с Id {advertisementId} отсутствует.");
       
        return advertisement;
    }
}