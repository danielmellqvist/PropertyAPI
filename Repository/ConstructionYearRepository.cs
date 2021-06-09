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
    public class ConstructionYearRepository : RepositoryBase<ConstructionYear>, IConstructionYearRepository
    {
        public ConstructionYearRepository(PropertyContext context) : base(context)
        {

        }

        public void CreateConstructionYear(ConstructionYear constructionYear) => Create(constructionYear);

        public int GetYearFromId(int id, bool trackChanges)
        {
            return FindByCondition(x => x.Id.Equals(id), trackChanges).SingleOrDefault().Year;
        }
    }
}
