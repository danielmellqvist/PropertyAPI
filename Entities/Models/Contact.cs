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
        public Guid? UserId { get; set; }
        public string Telephone { get; set; }

        // Relational
        //[ForeignKey(nameof(UserId))]
        //public User User { get; set; }
    }
}
