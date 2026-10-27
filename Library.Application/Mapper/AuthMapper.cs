using AutoMapper;
using Library.Application.Dto.User;
using Library.Domain.Entities;

namespace Library.Application.Mapper
{
    public class AuthMapper : Profile
    {
        public AuthMapper()
        {
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Loans, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore()); 
        }
    }
}
