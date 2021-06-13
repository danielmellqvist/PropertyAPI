using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class RealEstate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is a required field.")]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is a required field.")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Street is a required field.")]
        [MaxLength(50)]
        public string Street { get; set; }

        public int ZipCode { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Contact is a required field.")]
        public int ContactId { get; set; }
        public int ConstructionYearId { get; set; }
        public uint? SellingPrice { get; set; }
        public int? RentingPrice { get; set; }
        public bool CanBeSold { get; set; }
        public bool CanBeRented { get; set; }
        public int RealEstateTypeId { get; set; }
        public DateTime CreatedUtc { get; set; }

        //Relational
        [ForeignKey(nameof(ContactId))]
        public Contact Contact { get; set; }

        [ForeignKey(nameof(ConstructionYearId))]
        public ConstructionYear ConstructionYear { get; set; }

        [ForeignKey(nameof(RealEstateTypeId))]
        public RealEstateType RealEstateType { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
