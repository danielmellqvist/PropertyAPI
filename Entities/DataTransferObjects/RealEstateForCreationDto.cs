using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class RealEstateForCreationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public int ConstructionYear { get; set; }
        public int? SellingPrice { get; set; }
        public int? RentingPrice { get; set; }
        public int Type { get; set; }
    }
}
