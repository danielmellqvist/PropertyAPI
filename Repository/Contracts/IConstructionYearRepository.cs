using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IConstructionYearRepository
    {
        void CreateConstructionYear(ConstructionYear constructionYear);
        Task<int> GetYearFromIdAsync(int id, bool trackChanges);
        Task<ConstructionYear> GetConstructionYearFromIdAsync(int id, bool trackChanges);
        Task<ConstructionYear> GetFromYearAsync(int year, bool trackChanges);
    }
}
