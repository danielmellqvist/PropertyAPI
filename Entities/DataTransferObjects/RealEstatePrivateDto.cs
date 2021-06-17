using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class RealEstatePrivateDto : RealEstateInfoDto
    {

        public new string Contact { get; set; }
        public new IEnumerable<CommentsForRealEstateDto> Comments { get; set; }
        public new string RealEstateType { get; set; }

        





    }
}
