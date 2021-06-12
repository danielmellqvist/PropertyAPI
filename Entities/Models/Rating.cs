using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }

        public int ByUserId { get; set; }
        public int AboutUserId { get; set; }

        // Relational
        [ForeignKey(nameof(ByUserId))]
        public User ByUser { get; set; }

        [ForeignKey(nameof(AboutUserId))]
        public User AboutUser { get; set; }
    }
}
