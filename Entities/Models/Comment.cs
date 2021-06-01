using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500, ErrorMessage ="Comments can be max 500 characters")]
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid UserId { get; set; }
        public int RealEstateId { get; set; }
    }
}
