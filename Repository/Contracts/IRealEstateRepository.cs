using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IRealEstateRepository
    {
        Task<IEnumerable<RealEstate>> GetAllRealEstatesAsync(bool trackChanges);
        Task<RealEstate> GetRealEstateAsync(int realEstateId, bool trackChanges);
    }
}
