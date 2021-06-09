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
        int GetYearFromId(int id, bool trackChanges);

    }
}
