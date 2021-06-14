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


        public async Task<User> GetUserByUserId(int id, bool trackChanges)
        {
            var user = FindByCondition(x => x.Id == id, trackChanges).SingleOrDefaultAsync();
            return await user;
        }

        public async Task<User> GetUserByUserNameAsync(string username, bool trackChanges)
        {
            var user = FindByCondition(x => x.UserName == username, trackChanges).SingleOrDefaultAsync();
            return await user;
        }

        public async Task<User> GetUserByGuidId(Guid id, bool trackChanges)
        {
            var user = FindByCondition(x => x.IdentityUserId == id, trackChanges).SingleOrDefaultAsync();
            return await user;
        }

    }
}
