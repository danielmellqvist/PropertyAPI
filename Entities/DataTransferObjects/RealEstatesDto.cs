using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class RealEstatesDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Real estate title is a required field")]
        [MinLength(5, ErrorMessage = "Title must be over 5 characters long")]
        [MaxLength(50, ErrorMessage = "Title can only be 50 characters long")]
        public string Title { get; set; }

        [Range(0, uint.MaxValue, ErrorMessage = "Price can not be minus")]
        public uint? SellingPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price can not be minus")]
        public int? RentingPrice { get; set; }
        public bool CanBeSold { get; set; }
        public bool CanBeRented { get; set; }
    }
}
