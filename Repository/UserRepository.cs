using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>,  IUserRepository
    {
        public UserRepository(PropertyContext context) : base(context)
        {

        }

        public async Task<User> GetUserName(Guid id, bool trackchanges)
        {
            var username = FindByCondition(x => x.Id == id, trackchanges).SingleOrDefaultAsync();
            return await username;
        }
    }
}
