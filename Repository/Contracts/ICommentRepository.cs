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

        List<Comment> GetAllCommentsByRealEstateId(CommentsParameters commentsParameter, int id, bool trackChanges);

        public void CreateComment(Comment comment);
    }
}
