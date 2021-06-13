using Entities;
using Entities.Models;
using Entities.RequestFeatures;
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

        public async Task<IEnumerable<RealEstate>> GetAllRealEstatesAsync(RealEstateParameters realEstateParameters, bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderByDescending(x => x.CreatedUtc)
                .Skip((realEstateParameters.Skip) * realEstateParameters.Take)
                .Take(realEstateParameters.Take)
                .ToListAsync();
        }

        public async Task<RealEstate> GetRealEstateAsync(int realEstateId, bool trackChanges)
        {
            var realEstate = await FindByCondition(x => x.Id.Equals(realEstateId), trackChanges).SingleOrDefaultAsync();
            if (realEstate != null)
            {
                realEstate.ConstructionYear = await _context.ConstructionYears.Where(x => x.Id == realEstate.ConstructionYearId).FirstOrDefaultAsync();
                realEstate.RealEstateType = await _context.RealEstateTypes.Where(x => x.Id == realEstate.RealEstateTypeId).FirstOrDefaultAsync();
            }
            return realEstate;
        }

        // Marcus Added

        public async Task<IEnumerable<RealEstate>> GetAllRealEstatesByContactId(int contactId, bool trackChanges) =>
            await FindAll(trackChanges)
                    .Where(x => x.ContactId == contactId)
                    .ToListAsync();
        public async Task CreateRealEstateAsync(RealEstate realEstate)
        {
            await CreateAsync(realEstate);
        }

        public void DeleteRealEstate(RealEstate realEstate)
        {
            Delete(realEstate);
        }
    }
}
