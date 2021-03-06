using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }

        [RegularExpression(@"^[0-9+-]$", ErrorMessage = "Number must not contain illegal characters")]
        public string Telephone { get; set; }

        // Relational
        //[ForeignKey(nameof(UserId))]
        //public User User { get; set; }
    }
}
