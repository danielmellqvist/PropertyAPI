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

        public List<Comment> GetAllCommentsByUserId(Guid id, bool trackChanges) =>
            FindAll(trackChanges)
            .Where(x => x.UserId == id)
            .OrderBy(c => c.CreatedOn)
            .Take(10).ToList();


        public List<Comment> GetAllCommentsByRealEstateId(int id, bool trackChanges) =>
                FindAll(trackChanges)
                .Where(x => x.RealEstateId == id)
                .OrderByDescending(c => c.CreatedOn)
                .Take(10).ToList();


        public List<Comment> GetAllCommentsByRealEstateIdSkipTake(int id, int skip, int take, bool trackChanges)
        {
            if (skip != 0 &&)
            {

            }


        }
                FindAll(trackChanges)
                .Where(x => x.RealEstateId == id)
                .OrderByDescending(c => c.CreatedOn)
                .Take(10).ToList();


    }
}
