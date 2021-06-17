using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class RealEstateInfoDto
    {
        public string Contact { get; set; }
        public IEnumerable<CommentsForRealEstateDto> Comments { get; set; }
        public DateTime CreatedOn { get; set; }
        [Range(1600, 2500, ErrorMessage = "ConstructionYear is required and cannot be before 1600")]
        public int ConstructionYear { get; set; }
        [MaxLength(110, ErrorMessage = "The address field can only be 110 characters long")]
        public string Address { get; set; }
        public string RealEstateType { get; set; }
        [MinLength(10, ErrorMessage = "Description must be over 10 characters")]
        [MaxLength(1000, ErrorMessage = "Description can only be 1000 characters")]
        public string Description { get; set; }
        public int Id { get; set; }
        [MinLength(5, ErrorMessage = "Title must be over 5 characters long")]
        [MaxLength(50, ErrorMessage = "Title can only be 50 characters long")]
        public string Title { get; set; }

        [Range(0, uint.MaxValue, ErrorMessage = "Price can not be minus")]
        public uint SellingPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price can not be minus")]
        public int RentingPrice { get; set; }
        public bool CanBeSold { get; set; }
        public bool CanBeRented { get; set; }
    }
}
