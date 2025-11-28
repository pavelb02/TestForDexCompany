using Application.Services.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.Mapping;

public class AdvertisementProfile : Profile
{
    public AdvertisementProfile()
    {
        CreateMap<Advertisement, AdvertisementDto>();
    }
}