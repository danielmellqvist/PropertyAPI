﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class RealEstatePublicDto
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ConstructionYear { get; set; }
        public string Address { get; set; }
        public string RealEstateType { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public uint SellingPrice { get; set; }
        public int RentingPrice { get; set; }
        public bool CanBeSold { get; set; }
        public bool CanBeRented { get; set; }
    }
}
