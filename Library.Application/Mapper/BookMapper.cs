using AutoMapper;
using Library.Application.Dto;
using Library.Domain.Entities;

namespace Library.Application.Mapper
{
    public class BookMapper : Profile
    {
        public BookMapper()
        {
            CreateMap<CreateBookDto, Book>();

            CreateMap<UpdateBookDto, Book>();

            CreateMap<Book, BookResponseDto>()
                .ForMember(dest => dest.Authors, opt =>
                opt.MapFrom(src => src.BookAuthors.Select(ba=>ba.Author)));
        }
    }
}
