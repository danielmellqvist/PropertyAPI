using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string UserName { get; set; }

        //Relations
        public ICollection<Rating> Ratings { get; set; }

        public ICollection<Rating> RatingsByMe { get; set; }

    }
}
