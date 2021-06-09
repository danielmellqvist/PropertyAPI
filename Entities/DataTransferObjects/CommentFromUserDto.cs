using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CommentFromUserDto
    {
        // TODO Create list of the content and dates
        public string UserName { get; set; }

        public List<string> Content { get; set; } = new List<string>();

        public List<DateTime> CreatedOn { get; set; } = new List<DateTime>();

    }
}
