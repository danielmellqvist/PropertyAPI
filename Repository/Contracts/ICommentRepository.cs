using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface ICommentRepository
    {

        List<Comment> GetAllCommentsByUserId(CommentsParameters commentsParameters ,int id, bool trackChanges);

        Task<IEnumerable<Comment>> GetAllCommentsByRealEstateIdAsync(CommentsParameters commentsParameter, int id, bool trackChanges);

        void CreateComment(Comment comment);
    }
}
