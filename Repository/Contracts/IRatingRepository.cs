using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetRatingsByUserId(int userId, bool trackChanges);

        Task CreateNewRating(Rating rating);
        double GetAverageRating(IEnumerable<Rating> rating);
        Task<bool> CheckMultipleRatingsFromUser(RatingAddNewRatingDto ratingAddNewRatingDto);
    }
}
