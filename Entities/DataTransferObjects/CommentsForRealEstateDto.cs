﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CommentsForRealEstateDto
    {
        // Denis
        public string UserName { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
