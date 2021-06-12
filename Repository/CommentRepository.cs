using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
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

        public List<Comment> GetAllCommentsByUserId(CommentsParameters commentsParameters, Guid id, bool trackChanges) =>
            FindAll(trackChanges)
            .Where(x => x.UserId == id)
            .OrderBy(c => c.CreatedOn)
            .Skip(commentsParameters.Skip)
            .Take(commentsParameters.Take)
            .ToList();


        public void CreateComment(Comment comment) => Create(comment);

        public async Task<IEnumerable<Comment>> GetAllCommentsByRealEstateIdAsync(CommentsParameters commentsParameter, int id, bool trackChanges)
        {
            var comments = await FindAll(trackChanges)
                .Where(x => x.RealEstateId == id)
                .Include(x => x.User)
                .OrderBy(c => c.CreatedOn)
                .Skip((commentsParameter.Skip) * commentsParameter.Take)
                .Take(commentsParameter.Take)
                .ToListAsync();
            return comments;
        }
    }
}
