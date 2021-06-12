﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class RealEstateForCreationDto
    {
        [Required(ErrorMessage ="Real estate title is a required field")]
        [MaxLength(50, ErrorMessage ="The title can only be 50 characters long")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Real estate description is a required field")]
        [MaxLength(250, ErrorMessage ="The description field can only be 250 characters long")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Real estate address is a required field")]
        [MaxLength(110, ErrorMessage ="The address field can only be 110 characters long")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Contact is a required field")]
        public string Contact { get; set; }

        [Range(1600, 2100, ErrorMessage ="Construction year is required and cannot be before 1600")]
        public int ConstructionYear { get; set; }
        public int? SellingPrice { get; set; }
        public int? RentingPrice { get; set; }

        [Required(ErrorMessage = "Property Type is a required field")]

        public int Type { get; set; }
    }
}
