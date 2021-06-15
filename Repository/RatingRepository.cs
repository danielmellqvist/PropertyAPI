using Entities;
using Entities.DataTransferObjects;
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

        public async Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(int userId, bool trackChanges) =>
            await FindAll(trackChanges)
            .Where(x => x.AboutUserId == userId)
            .ToListAsync();

        // marcus added
        public async Task CreateNewRating(Rating rating) => await CreateAsync(rating);


        //marcus added
        public async Task<bool> CheckMultipleRatingsFromUserAsync(RatingAddNewRatingDto ratingAddNewRatingDto)
        {
            bool checkSpam = false;
            var aboutRatings = await GetRatingsByUserIdAsync(ratingAddNewRatingDto.AboutUserId, trackChanges:false);
            foreach (var rating in aboutRatings)
            {
                if (rating.ByUserId == ratingAddNewRatingDto.ByUserId && rating.AboutUserId == ratingAddNewRatingDto.AboutUserId)
                {
                    checkSpam = true;
                    return checkSpam;
                }
            }
            return checkSpam;
        }
        public double GetAverageRating(IEnumerable<Rating> rating)
        {
            double average = ((GetAllRatingValues(rating)).Sum()) / rating.Count();
            return average;
        }

        public List<double> GetAllRatingValues(IEnumerable<Rating> rating)
        {
            List<double> ratings = new();
            foreach (var item in rating)
            {
                ratings.Add(item.Value);
            }
            return ratings;
        }

    }
}