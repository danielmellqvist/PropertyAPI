using Entities;
using Entities.Models;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(PropertyContext context) : base(context)
        {

        }

        public List<Comment> GetAllCommentsById(Guid id, bool trackChanges) => 
            FindAll(trackChanges)
            .Where(x => x.UserId == id)
            .OrderBy(c => c.CreatedOn)
            .Take(10).ToList();
    }
}
