using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Initializer
{
    public class RealEstateInitializer
    {
        //private readonly PropertyContext _context;

        //public RealEstateInitializer(PropertyContext context)
        //{
        //    _context = context;
        //}

        public static void Initialize(PropertyContext _context)
        {
            _context.Database.EnsureCreated();

            if (!_context.RealEstateTypes.Any())
            {
                var realEstateTypes = new RealEstateType[]
                {
                    new RealEstateType { Type = "Appartment" },
                    new RealEstateType{ Type = "House" },
                    new RealEstateType {Type = "Office" },
                    new RealEstateType { Type= "Warehouse" }
                };
                _context.AddRange(realEstateTypes);
                _context.SaveChanges();
            }

        }
    }
}
