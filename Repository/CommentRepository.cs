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

        public async Task<IEnumerable<Comment>> GetAllCommentsByUserIdWithParameters(CommentsParameters commentsParameters, int id, bool trackChanges) =>
           await  FindAll(trackChanges)
            .Where(x => x.UserId == id)
            .OrderBy(c => c.CreatedOn)
            .Skip(commentsParameters.Skip)
            .Take(commentsParameters.Take)
            .ToListAsync();

        // Marcus added
        public async Task<IEnumerable<Comment>> GetAllCommentsByUserIdAsync(int UserId, bool trackChanges) =>
            await FindAll(trackChanges)
            .Where(x => x.UserId == UserId)
            .ToListAsync();

        public void CreateComment(Comment comment) => Create(comment);

        public async Task<IEnumerable<Comment>> GetAllCommentsByRealEstateIdAsync(int id, bool trackChanges)
        {
            var comments = await FindAll(trackChanges)
                .Where(x => x.RealEstateId == id)
                .Include(x => x.User)
                .OrderBy(c => c.CreatedOn)
                .ToListAsync();
            return comments;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsByRealEstateIdParametersAsync(CommentsParameters commentsParameter, int id, bool trackChanges)
        {
            var comments = await FindAll(trackChanges)
                .Where(x => x.RealEstateId == id)
                .Include(x => x.User)
                .OrderBy(c => c.CreatedOn)
                .Skip((commentsParameter.Skip - 1) * commentsParameter.Take)
                .Take(commentsParameter.Take)
                .ToListAsync();
            return comments;
        }
    }
}
