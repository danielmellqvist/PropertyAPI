using Entities;
using Entities.Models;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RealEstateRepository : RepositoryBase<RealEstate>, IRealEstateRepository
    {
        public RealEstateRepository(PropertyContext context) : base(context)
        {

        }

        public IEnumerable<RealEstate> GetAllRealEstates(bool trackChanges)
        {
            return FindAll(trackChanges)
                .OrderByDescending(x => x.CreatedUtc)
                .ToList();
        }

        public RealEstate GetRealEstate(int realEstateId, bool trackChanges)
        {
            return FindByCondition(x => x.Id.Equals(realEstateId), trackChanges)
                .SingleOrDefault();
        }
    }
}
