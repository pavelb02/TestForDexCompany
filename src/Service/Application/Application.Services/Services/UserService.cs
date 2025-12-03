using Application.Services.DTO;
using Application.Services.Interfaces;
using Application.Services.Options;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace Application.Services.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IImageService _imageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly int _maxCountAdvertisements;

    public UserService(
        IUserRepository userRepository,
        IImageService imageService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOptions<AdvertisementOptions> advertisementOptions)
    {
        _userRepository = userRepository;
        _imageService = imageService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _maxCountAdvertisements = advertisementOptions.Value.MaxCountAdUser;
    }

    public async Task<Guid> CreateUserAsync(CreateUserRequest createRequest)
    {
        ArgumentNullException.ThrowIfNull(createRequest);
        var user = new User(createRequest.Name);

        await _userRepository.CreateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return user.Id;
    }

    public async Task<Guid> UpdateUserAsync(UpdateUserRequest updateRequest)
    {
        ArgumentNullException.ThrowIfNull(updateRequest);
        var user = await _userRepository.GetByIdAsync(updateRequest.Id, true).ConfigureAwait(false);

        user.Update(updateRequest.Name);

        //await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return user.Id;
    }

    public async Task<UserDto> GetUserAsync(Guid userId, bool trackChanges = false)
    {
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);

        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);

        foreach (var advertisement in user.Advertisements)
        {
            await _imageService.DeleteImageAsync(advertisement.Image);
        }

        await _userRepository.Delete(user);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddAdvertisementAsync(Guid userId, CreateAdvertisementRequest createAdvertisementRequest)
    {
        ArgumentNullException.ThrowIfNull(createAdvertisementRequest);
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);

        if (user.Advertisements.Count >= _maxCountAdvertisements)
            throw new InvalidOperationException(
                "Пользователь не может иметь больше чем {maxAdvertisements} объявлений.");

        int nextNumber = 1;
        if (user.Advertisements.Any())
        {
            nextNumber = user.Advertisements.Max(a => a.Number) + 1;
        }

        user.AddAdvertisement(
            nextNumber,
            createAdvertisementRequest.Text,
            createAdvertisementRequest.Image,
            createAdvertisementRequest.EndDate);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Guid> UpdateAdvertisementAsync(Guid userId, UpdateAdvertisementRequest updateAdvertisementRequest)
    {
        ArgumentNullException.ThrowIfNull(updateAdvertisementRequest);
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);

        await _imageService.DeleteImageAsync(user.GetAdvertisement(updateAdvertisementRequest.Id).Image);
        user.UpdateAdvertisement(
            updateAdvertisementRequest.Id,
            updateAdvertisementRequest.Text,
            updateAdvertisementRequest.Image,
            updateAdvertisementRequest.EndDate);

        await _unitOfWork.SaveChangesAsync();
        return user.Id;
    }

    public async Task DeleteAdvertisementAsync(Guid userId, Guid advertisementId)
    {
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);
        var advertisement = user.GetAdvertisement(advertisementId);

        await _imageService.DeleteImageAsync(advertisement.Image);
        user.RemoveAdvertisement(advertisementId);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> GetImageNameAsync(Guid userId, Guid advertisementId)
    {
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);
        var advertisement = user.GetAdvertisement(advertisementId);

        return advertisement.Image;
    }
}