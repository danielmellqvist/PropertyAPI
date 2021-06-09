using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class ConstructionYear
    {
        [Key]
        public int Id { get; set; }
        public int Year { get; set; }
    }
}
