using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IRepositoryManager
    {
        ICommentRepository Comment { get; }
        IConstructionYearRepository ConstructionYear { get; }
        IContactRepository Contact { get; }
        IRatingRepository Rating { get; }
        IRealEstateRepository RealEstate { get; }
        IRealEstateTypeRepository RealEstateType { get; }
        IUserRepository User { get; }

        void Save();
    }
}
