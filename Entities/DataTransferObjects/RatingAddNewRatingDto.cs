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

        public Guid ByUserGuidId { get; set; }

        public int ByUserId { get; set; }
        public int AboutUserId { get; set; }

        public int Value { get; set; }

    }
}
