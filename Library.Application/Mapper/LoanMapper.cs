using AutoMapper;
using Library.Application.Dto;
using Library.Domain.Entities;
using Library.Domain.Enums;

namespace Library.Application.Mapper
{
    public class LoanMapper : Profile
    {
        public LoanMapper()
        {
            CreateMap<Loan, LoanResponseDto>()
                .ForMember(dest => dest.BookTitle, opt =>
                opt.MapFrom(src => src.Book != null ? src.Book.Title : string.Empty));

            CreateMap<CreateLoanDto, Loan>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.LoanDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => DateTime.UtcNow.AddDays(src.LoanDays)))
                .ForMember(dest => dest.ReturnDate, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore()) 
                .ForMember(dest => dest.Status, opt => opt.MapFrom( _ => LoanStatus.Active)); 
        }
    }
}
    