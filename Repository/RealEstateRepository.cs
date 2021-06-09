using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<RealEstate>> GetAllRealEstatesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderByDescending(x => x.CreatedUtc)
                .ToListAsync();
        }

        public async Task<RealEstate> GetRealEstateAsync(int realEstateId, bool trackChanges)
        {
            return await FindByCondition(x => x.Id.Equals(realEstateId), trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}
