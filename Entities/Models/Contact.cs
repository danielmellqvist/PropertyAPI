using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public uint? Telephone { get; set; }

        // Relational
        //[ForeignKey("UserId")]
        //public User User { get; set; }
    }
}
