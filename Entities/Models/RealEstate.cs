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
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        public int AddressId { get; set; }
        public int ContactId { get; set; }
        public int ConstructionYearId { get; set; }
        public uint? SellingPrice { get; set; }
        public int? RentingPrice { get; set; }
        public bool CanBeSold { get; set; }
        public bool CanBeRented { get; set; }
        public int RealEstateTypeId { get; set; }

        //Relational
        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        //[ForeignKey("ContactId")]
        //public Contact Contact { get; set; }

        [ForeignKey("ConstructionYearId")]
        public ConstructionYear ConstructionYear { get; set; }

        [ForeignKey("RealEstateTypeId")]
        public RealEstateType RealEstateType { get; set; }
    }
}
