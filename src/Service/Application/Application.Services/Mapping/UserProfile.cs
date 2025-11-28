using Application.Services.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Services.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Advertisements,
                opt => opt.MapFrom(src => src.Advertisements.ToList()));
    }
}