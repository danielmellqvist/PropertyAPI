using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public abstract class RequestParameters
    {

        //Returns paged real estates, added previously from users.
        //Skip and Take are optional query string parameters.
        //If they are missing,
        //default skip is 0 and
        //default take is 10.
        //Take cannot be more than 100. 


        const int maxTake = 100;

        private int _skip = 1;
        public int Skip
        {
            get
            {
                return _skip;
            }
            set
            {
                _skip = (value <= 0) ? 1 : value;
            }
        }




        private int _take = 10;
        public int Take 
        { 
            get
            {
                return _take;
            }
            set
            {
                _take = (value > maxTake) ? maxTake : value;
            } 
        }
    }
}
