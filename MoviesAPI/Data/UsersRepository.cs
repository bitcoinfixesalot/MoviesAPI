using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data.Entities;

namespace MoviesAPI.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly MoviesContext _ctx;

        public UsersRepository(MoviesContext ctx)
        {
            this._ctx = ctx;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _ctx.Users.FirstOrDefaultAsync(a => a.Id == userId);
        }
    }
}
