using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface ICommentRepository
    {

        List<Comment> GetAllCommentsByUserId(Guid id, bool trackChanges);

        List<Comment> GetAllCommentsByRealEstateId(int id, bool trackChanges);

        List<Comment> GetAllCommentsByRealEstateIdSkipTake(int id, int skip, int take, bool trackChanges);
        
    }
}
