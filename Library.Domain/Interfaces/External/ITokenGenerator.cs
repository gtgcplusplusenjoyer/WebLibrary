using Library.Domain.Dto.User;
using Library.Domain.Entities;

namespace Library.Domain.Interfaces.External
{
    public interface ITokenGenerator
    {
        TokenPair GenerateTokenPair(User user);
    }
}
