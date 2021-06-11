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

        public IEnumerable<Rating> GetAllRatings(bool trackChanges) =>
             FindAll(trackChanges)
            .OrderBy(c => c.RatingValue)
            .ToList();


    }
}
