using AutoMapper;
using FluentAssertions;
using Library.Application.Dto;
using Library.Application.Dto.Author;
using Library.Application.Exceptions;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Moq;

namespace Library.Tests.Services
{
    public class AuthorServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly AuthorService _service;
        public AuthorServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _service = new AuthorService(_authorRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAuthorsAsync_WhenAuthorsExist_ReturnsAuthorsResponseDtos()
        {
            var authors = new List<Author>
            {
                new Author { Id = Guid.NewGuid(), FirstName = "Alex", LastName = "Gtg" },
                new Author { Id = Guid.NewGuid(), FirstName = "Tom", LastName = "Hol"}
            };

            var authorsDtos = new List<AuthorResponseDto>
            {
                new AuthorResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Alex", "Gtg", null),
                new AuthorResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Tom", "Hol", null)
            };

            _authorRepositoryMock
                .Setup(r => r.GetAll(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(authors);

            _mapperMock
                .Setup(m => m.Map<List<AuthorResponseDto>>(authors))
                .Returns(authorsDtos);

            var result = await _service.GetAllAuthorsAsync(1, 10, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllAuthorsAsync_WhenNoAuthors_ReturnsEmptyList()
        {
            var authors = new List<Author>();
            var authorsDtos = new List<AuthorResponseDto>();

            _authorRepositoryMock
                .Setup(r => r.GetAll(1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(authors);

            _mapperMock
                .Setup(m => m.Map<List<AuthorResponseDto>>(authors))
                .Returns(authorsDtos);

            var result = await _service.GetAllAuthorsAsync(1, 10, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAuthorById_WhenAuthorExist_ReturnsResponseDto()
        {
            var authorId = Guid.NewGuid();
            var author = new Author
            {
                Id = authorId,
                FirstName = "Bob",
                LastName = "Great",
                Biography = null,
                CreatedAt = DateTime.UtcNow
            };

            var authorDto = new AuthorResponseDto(authorId, DateTime.UtcNow, "Bob", "Great", null);

            _authorRepositoryMock
                .Setup(r => r.GetById(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            _mapperMock
                .Setup(m => m.Map<AuthorResponseDto>(author))
                .Returns(authorDto);

            var result = await _service.GetAuthorByIdAsync(authorId, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(authorId);
            result.FirstName.Should().Be("Bob");
        }

        [Fact]
        public async Task GetAuthorById_WhenAuthorNotExist_ThrowsNotFoundException()
        {
            var authorId = Guid.NewGuid();

            _authorRepositoryMock
                .Setup(r => r.GetById(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Author?)null);

            Func<Task> act = async () =>
            {
                await _service.GetAuthorByIdAsync(authorId, CancellationToken.None);
            };

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Author with Id: {authorId} not found");
        }

        [Fact]
        public async Task DeleteAuthorAsync_WhenAuthorExists_DeletesAuthor()
        {
            var authorId = Guid.NewGuid();
            var author = new Author
            {
                Id = authorId,
                FirstName = "Bob",
                LastName = "Great",
                CreatedAt = DateTime.UtcNow,
                Biography = null
            };

            _authorRepositoryMock
                .Setup(r => r.GetById(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            await _service.DeleteAuthorAsync(authorId, CancellationToken.None);

            _authorRepositoryMock
                .Verify(r => r.Delete(author));

            _authorRepositoryMock
                .Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthorAsync_WhenAuthorNotExists_ThrowsNotFoundException()
        {
            var authorId = Guid.NewGuid();

            _authorRepositoryMock
                .Setup(r => r.GetById(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Author?)null);

            Func<Task> act = async () =>
            {
                await _service.DeleteAuthorAsync(authorId, CancellationToken.None);
            };

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Author with Id: {authorId} not found");
        }

        [Fact]
        public async Task CreateAuthorAsync_ValidDto_ReturnsAuthorResponseDto()
        {
            var createDto = new CreateAuthorDto("Bob", "Great", null);

            var author = new Author
            {
                FirstName = "Bob",
                LastName = "Great",
                Biography = null
            };

            var responseDto = new AuthorResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Bob", "Great", null);

            _mapperMock
                .Setup(m => m.Map<Author>(createDto))
                .Returns(author);

            _mapperMock
                .Setup(m => m.Map<AuthorResponseDto>(author))
                .Returns(responseDto);

            var result = await _service.CreateAuthorAsync(createDto, CancellationToken.None);

            result.Should().NotBeNull();
            result.FirstName.Should().Be("Bob");
        }

        [Fact]
        public async Task CreateAuthorAsync_ValidDto_CallsRepositoryAdd()
        {
            var createDto = new CreateAuthorDto("Bob", "Great", null);

            var author = new Author
            {
                FirstName = "Bob",
                LastName = "Great",
                Biography = null
            };

            var responseDto = new AuthorResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Bob", "Great", null);

            _mapperMock
                .Setup(m => m.Map<Author>(createDto))
                .Returns(author);

            _mapperMock
                .Setup(m => m.Map<AuthorResponseDto>(author))
                .Returns(responseDto);

            await _service.CreateAuthorAsync(createDto, CancellationToken.None);

            _authorRepositoryMock
                .Verify(r => r.Add(It.IsAny<Author>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAuthorAsync_ValidDto_CallsSaveChanges()
        {
            var createDto = new CreateAuthorDto("Bob", "Great", null);

            var author = new Author
            {
                FirstName = "Bob",
                LastName = "Great",
                Biography = null
            };

            var responseDto = new AuthorResponseDto(Guid.NewGuid(), DateTime.UtcNow, "Bob", "Great", null);

            _mapperMock
                .Setup(m => m.Map<Author>(createDto))
                .Returns(author);

            _mapperMock
                .Setup(m => m.Map<AuthorResponseDto>(author))
                .Returns(responseDto);

            await _service.CreateAuthorAsync(createDto, CancellationToken.None);

            _authorRepositoryMock
                .Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAuthorAsync_WhenAuthorExists_ReturnsUpdatedAuthor()
        {
            var authorId = Guid.NewGuid();

            var author = new Author
            {
                Id = authorId,
                Biography = null,
                FirstName = "OldBob",
                LastName = "OldGreat"
            };

            var updateDto = new UpdateAuthorDto("NewBob", "NewGreat", null);

            var updatedResponse = new AuthorResponseDto(authorId, DateTime.UtcNow, "NewBob", "NewGreat", null);

            _authorRepositoryMock
                .Setup(r => r.GetById(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            _mapperMock
                .Setup(m => m.Map<AuthorResponseDto>(author))
                .Returns(updatedResponse);

            var result = await _service.UpdateAuthorAsync(authorId, updateDto, CancellationToken.None);

            result.Should().NotBeNull();
            result.FirstName.Should().Be("NewBob");
            result.LastName.Should().Be("NewGreat");
        }

        [Fact]
        public async Task UpdateAuthorAsync_WhenAuthorDoesNotExists_ThrowsNotFoundException()
        {
            var authorId = Guid.NewGuid();

            var updatedDto = new UpdateAuthorDto("Bob", "Great", null);

            _authorRepositoryMock
                .Setup(r => r.GetById(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Author?)null);

            Func<Task> act = async () =>
            {
                await _service.UpdateAuthorAsync(authorId, updatedDto, CancellationToken.None);
            };


            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Author with Id: {authorId} not found");
        }

        [Fact]
        public async Task UpdateAuthorAsync_WhenAuthorExists_CallsRepositoryUpdate()
        {
            var authorId = Guid.NewGuid();

            var author = new Author
            {
                Id = authorId,
                Biography = null,
                FirstName = "OldBob",
                LastName = "OldGreat"
            };

            var updateDto = new UpdateAuthorDto("NewBob", "NewGreat", null);

            var updatedResponse = new AuthorResponseDto(authorId, DateTime.UtcNow, "NewBob", "NewGreat", null);

            _authorRepositoryMock
                .Setup(r => r.GetById(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            _mapperMock
                .Setup(m => m.Map<AuthorResponseDto>(author))
                .Returns(updatedResponse);

            await _service.UpdateAuthorAsync(authorId, updateDto, CancellationToken.None);

            _authorRepositoryMock
                .Verify(r => r.Update(It.IsAny<Author>()), Times.Once);

            _authorRepositoryMock
                .Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
