﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class RealEstateType
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Type { get; set; }
    }
}
