using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class ConstructionYear
    {
        [Key]
        public int Id { get; set; }

        [Range(1599, double.PositiveInfinity)]
        public int Year { get; set; }
    }
}
