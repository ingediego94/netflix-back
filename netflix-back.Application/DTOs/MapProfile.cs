using AutoMapper;
using netflix_back.Domain.Entities;

namespace netflix_back.Application.DTOs;

public class MapProfile : Profile
{
    public MapProfile()
    {
        // JWT 
        CreateMap<RegisterDto, User>();
        CreateMap<User, RegisterDto>();

        CreateMap<User, UserRegisterResponseDto>();
        CreateMap<UserRegisterResponseDto, User>();

        CreateMap<UserAuthResponseDto, User>();
        CreateMap<User, UserAuthResponseDto>();
        
        // User:
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();
        CreateMap<User, UserResponseDto>();
        
        // Mapping of Video:
        CreateMap<Video, VideoResponseDto>();
        CreateMap<VideoResponseDto, Video>();
        
        // Mapping of History:
        CreateMap<HistoryCreateDto, History>();
        CreateMap<HistoryUpdateDto, History>();
        CreateMap<History, HistoryResponseDto>();
        
        // Content:
        CreateMap<Content, ContentResponseDto>()
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.GenreName));
        
    }
}