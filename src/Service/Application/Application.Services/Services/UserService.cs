using Application.Services.DTO;
using Application.Services.Interfaces;
using Application.Services.Options;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Shared.Domain.Exceptions;

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

    public async Task<Guid> UpdateUserAsync(Guid userId, UpdateUserRequest updateRequest)
    {
        ArgumentNullException.ThrowIfNull(updateRequest);
        var user = await _userRepository.GetByIdAsync(userId, true);

        user.Update(updateRequest.Name);

        await _unitOfWork.SaveChangesAsync();
        return user.Id;
    }

    public async Task<UserDto> GetUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId, true);

        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId, true);

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
        
        await using var tx = await _unitOfWork.BeginTransactionAsync();
        
        var user = await _userRepository.GetByIdAsync(userId, true);

        if (user.Advertisements.Count >= _maxCountAdvertisements)
            throw new InvalidOperationException(
                "Пользователь не может иметь больше чем {maxAdvertisements} объявлений.");

        user.AddAdvertisement(
            createAdvertisementRequest.Text,
            createAdvertisementRequest.Image,
            createAdvertisementRequest.EndDate);

        await _unitOfWork.SaveChangesAsync();
        
        await tx.CommitAsync();
    }

    public async Task<Guid> UpdateAdvertisementAsync(Guid userId, Guid advertisementId,
        UpdateAdvertisementRequest updateAdvertisementRequest)
    {
        ArgumentNullException.ThrowIfNull(updateAdvertisementRequest);
        var user = await _userRepository.GetByIdAsync(userId, true);

        await _imageService.DeleteImageAsync(user.GetAdvertisement(advertisementId).Image);
        user.UpdateAdvertisement(
            advertisementId,
            updateAdvertisementRequest.Text,
            updateAdvertisementRequest.Image,
            updateAdvertisementRequest.EndDate);

        await _unitOfWork.SaveChangesAsync();
        return user.Id;
    }

    public async Task DeleteAdvertisementAsync(Guid userId, Guid advertisementId)
    {
        var user = await _userRepository.GetByIdAsync(userId, true);
        var advertisement = user.GetAdvertisement(advertisementId);

        await _imageService.DeleteImageAsync(advertisement.Image);
        user.RemoveAdvertisement(advertisementId);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> GetImageNameAsync(Guid userId, Guid advertisementId)
    {
        var user = await _userRepository.GetByIdAsync(userId, true);
        var advertisement = user.GetAdvertisement(advertisementId);

        return advertisement.Image;
    }

    public async Task SetRatingAsync(Guid userId, Guid advertisementId, RatingRequest rating)
    {
        var user = await _userRepository.GetByIdAsync(userId, true);

        var advertisement = user.Advertisements
            .FirstOrDefault(x => x.Id == advertisementId);

        if (advertisement == null)
            throw new EntityNotFoundException($"Объявление с Id {advertisementId} отсутствует.");

        advertisement.SetRating(rating.Rating);

        await _unitOfWork.SaveChangesAsync();
    }
}