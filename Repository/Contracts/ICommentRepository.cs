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

        Task<IEnumerable<Comment>> GetAllCommentsByUserIdWithParameters(CommentsParameters commentsParameters ,int id, bool trackChanges);

        // marcus added
        Task<IEnumerable<Comment>> GetAllCommentsByUserId(int UserId, bool trackChanges);
        Task<IEnumerable<Comment>> GetAllCommentsByRealEstateIdAsync(CommentsParameters commentsParameter, int id, bool trackChanges);

        void CreateComment(Comment comment);
    }
}
