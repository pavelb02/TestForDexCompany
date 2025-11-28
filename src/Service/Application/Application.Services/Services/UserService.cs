using Application.Services.DTO;
using Application.Services.Interfaces;
using Application.Services.Options;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Personnel.Application.Services.Interfaces;

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

    public async Task<UserDto> GetUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);
        
        foreach (var advertisement in user.Advertisements)
        {
                await _imageService.DeleteImageAsync(advertisement.PathImage);
        }
        
        await _userRepository.Delete(user);
        
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddAdvertisementAsync(Guid userId, CreateAdvertisementDto createAdvertisementDto)
    {
        ArgumentNullException.ThrowIfNull(createAdvertisementDto);
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);
        
        if (user.Advertisements.Count >= _maxCountAdvertisements)
            throw new InvalidOperationException("Пользователь не может иметь больше чем {maxAdvertisements} объявлений.");
        
        user.AddAdvertisement(
            createAdvertisementDto.Text,
            createAdvertisementDto.PathImage,
            createAdvertisementDto.EndDate);
       
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAdvertisementAsync(Guid userId, Guid advertisementId)
    {
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);
        var advertisement = user.GetAdvertisement(advertisementId);
      
        await _imageService.DeleteImageAsync(advertisement.PathImage);
        user.RemoveAdvertisement(advertisementId);
        
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Guid> UpdateAdvertisementAsync(Guid userId, UpdateAdvertisementRequest updateAdvertisementRequest)
    {
        ArgumentNullException.ThrowIfNull(updateAdvertisementRequest);
        var user = await _userRepository.GetByIdAsync(userId, true).ConfigureAwait(false);
        
        await _imageService.DeleteImageAsync(user.GetAdvertisement(updateAdvertisementRequest.Id).PathImage);
        user.UpdateAdvertisement(
            updateAdvertisementRequest.Id,
            updateAdvertisementRequest.Text,
            updateAdvertisementRequest.PathImage,
            updateAdvertisementRequest.EndDate);
        
        await _unitOfWork.SaveChangesAsync();
        return user.Id;
    }
}