using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Rating is a required field")]
        [Range(1, 5, ErrorMessage = "Rate a user from 1 to 5")]
        public string Value { get; set; }

        public int OutputValue { get; set; }


    }
}
