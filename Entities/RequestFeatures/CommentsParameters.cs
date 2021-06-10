using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class CommentsParameters : RequestParameters
    {
        // Parameters for comments
        // S == Skip    How many comments to skip
        // default value is 0 
        // T == Take    How many to take
        // default value is 10 and maximum value is 100
    }
}
