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
    public class RatingRepository : RepositoryBase<Rating>, IRatingRepository
    {
        public RatingRepository(PropertyContext context) : base(context)
        {

        }

        public IEnumerable<Rating> GetAllRatingsAverage(string username, bool trackChanges) =>
            FindAll(trackChanges)
            .Where(x => x.AboutUserId == ...(username))
            .ToList();
        //Average();

    }
}