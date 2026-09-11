using AutoMapper;
using FluentAssertions;
using Library.Application.Dto;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Moq;

namespace Library.Tests
{
    public class BookServiceTests
    {
        private readonly Mock<ILibraryRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BookService _service;
        public BookServiceTests( )
        {
            _repositoryMock = new Mock<ILibraryRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new BookService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllBooksAsync_WhenBooksExist_ReturnsBookResponseDtos()
        {
            // Arrange (Подготовка)
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Война и мир", Publisher = "АСТ" },
                new Book { Id = Guid.NewGuid(), Title = "Преступление и наказание", Publisher = "Эксмо" }
            };

            var bookDtos = new List<BookResponseDto>
            {
                new BookResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Война и мир", "АСТ", null, 1, 1),
                new BookResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Преступление и наказание", "Эксмо", null, 1, 1)
            };

            _repositoryMock
                .Setup(r => r.GetAllBooks(It.IsAny<CancellationToken>()))
                .ReturnsAsync(books);

            _mapperMock
                .Setup(m => m.Map<List<BookResponseDto>>(books))
                .Returns(bookDtos);

            // Act (Действие)
            var result = await _service.GetAllBooksAsync(CancellationToken.None);

            // Assert (Проверка)
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(bookDtos);
        }

    }
}
