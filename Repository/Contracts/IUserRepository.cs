using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserId(int id, bool trackChanges);

        Task<User> GetUserByUserNameAsync(string username, bool trackChanges);

        Task<User> GetUserByGuidId(Guid id, bool trackChanges);
    }
}
