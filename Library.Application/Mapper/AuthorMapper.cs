using AutoMapper;
using Library.Application.Dto;
using Library.Application.Dto.Author;
using Library.Domain.Entities;

namespace Library.Application.Mapper
{
    public class AuthorMapper : Profile
    {
        public AuthorMapper()
        {
            CreateMap<CreateAuthorDto, Author>();
            CreateMap<UpdateAuthorDto, Author>();
            CreateMap<Author, AuthorResponseDto>();
        }
    }
}
