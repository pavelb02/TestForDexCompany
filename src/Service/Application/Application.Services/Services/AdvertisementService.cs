using Application.Services.DTO;
using Application.Services.Interfaces;
using Application.Services.Options;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Application.Services.Services;

public class AdvertisementService : IAdvertisementService
{
    private readonly IAdvertisementRepository _advertisementRepository;
    private readonly IMapper _mapper;

    public AdvertisementService(
        IAdvertisementRepository advertisementRepository,
        IMapper mapper)

    {
        _advertisementRepository = advertisementRepository;
        _mapper = mapper;
    }

    public async Task<List<AdvertisementDto>> SearchAsync(AdvertisementSearchRequest searchRequest)
    {
        ArgumentNullException.ThrowIfNull(searchRequest);
        var advertisements = await _advertisementRepository.SearchAsync(searchRequest);
        return _mapper.Map<List<AdvertisementDto>>(advertisements);
    }
}