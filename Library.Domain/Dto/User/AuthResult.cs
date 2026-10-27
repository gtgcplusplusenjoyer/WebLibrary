namespace Library.Domain.Dto.User
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
        public TokenPair? TokenPair { get; set; }

        public static AuthResult Success(TokenPair tokenPair, Guid userId)
        {
            return new AuthResult
            {
                IsSuccess = true,
                TokenPair = tokenPair,
                UserId = userId,
                ErrorMessage = string.Empty
            };
        }
        public static AuthResult Success(Guid userId)
        {
            return new AuthResult
            {
                IsSuccess = true,
                UserId = userId,
                ErrorMessage = string.Empty
            };
        }
        public static AuthResult Failure(string message)
        {
            return new AuthResult
            {
                IsSuccess = false,
                ErrorMessage = message,
            };
        }


    }
}
