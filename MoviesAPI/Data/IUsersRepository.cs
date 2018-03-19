using MoviesAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Data
{
    public interface IUsersRepository
    {
        Task<User> GetUserByIdAsync(int userId);
    }
}
