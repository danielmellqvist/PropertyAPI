using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CommentForCreationDto 
    {
        [Required(ErrorMessage = "Comment is a required field.")]
        [MaxLength(500, ErrorMessage = "Comments can be max 500 characters")]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "RealEstateId is a required field")]
        [Range(0, int.MaxValue, ErrorMessage = "RealEstateId must be an integer over 0")]
        public string RealEstateId { get; set; }
    }
}
