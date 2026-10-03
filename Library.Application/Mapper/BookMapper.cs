using AutoMapper;
using Library.Application.Dto.Book;
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
                .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("CreatedAt", opt => opt.MapFrom(src => src.CreatedAt))
                .ForCtorParam("Title", opt => opt.MapFrom(src => src.Title))
                .ForCtorParam("Publisher", opt => opt.MapFrom(src => src.Publisher))
                .ForCtorParam("Description", opt => opt.MapFrom(src => src.Description))
                .ForCtorParam("TotalCopies", opt => opt.MapFrom(src => src.TotalCopies))
                .ForCtorParam("AvailableCopies", opt => opt.MapFrom(src => src.AvailableCopies))
                .ForCtorParam("Authors", opt => opt.MapFrom(src =>
                src.BookAuthors.Select(ba => ba.Author)));
        }
    }
}
