﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class RatingDto
    {
        public Guid UserId { get; set; }
        public int RealEstateId { get; set; }
        public int Comments { get; set; }
        public double Rating { get; set; }
    }
}
