using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Rating>> GetRatingsByUserId(int userId, bool trackChanges) =>
            await FindAll(trackChanges)
            .Where(x => x.AboutUserId == userId)
            .ToListAsync();

        public double GetAverageRating(IEnumerable<Rating> rating)
        {
            double average = ((GetAllRatings(rating)).Sum()) / rating.Count();
            return average;
        }

        public List<double> GetAllRatings(IEnumerable<Rating> rating)
        {
            List<double> ratings = new();
            foreach (var item in rating)
            {
                ratings.Add(item.RatingValue);
            }
            return ratings;
        }

    }
}