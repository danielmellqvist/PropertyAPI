using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class RatingAddNewRatingDto
    {
        public Guid UserId { get; set; }

        public Guid ByUserId { get; set; }

        public double Rating { get; set; }

    }
}
