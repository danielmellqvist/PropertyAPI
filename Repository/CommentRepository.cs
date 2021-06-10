using Entities;
using Entities.Models;
using Entities.RequestFeatures;
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


        public List<Comment> GetAllCommentsByRealEstateId(CommentsParameters commentsParameters, int id, bool trackChanges) =>
                FindAll(trackChanges)
                .Where(x => x.RealEstateId == id)
                .OrderBy(c => c.CreatedOn)
                .Skip(commentsParameters.Skip)
                .Take(commentsParameters.Take)
                .ToList();


    }
}
