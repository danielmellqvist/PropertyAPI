using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ConstructionYear
    {
        [Key]
        public int Id { get; set; }

        [Range(1600, double.PositiveInfinity)]
        public int Year { get; set; }
    }
}
