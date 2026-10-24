using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User> 
    {  
        Task<User?> GetUserByEmail(string email,CancellationToken cancellationToken);
    }
}
