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
                // Metod för att fylla på realestatetypes

                // save till databasen
            }
            if (!_context.Users.Any())
            {
                // Metod för att fylla på användare
            }
            if (!_context.ConstructionYears.Any())
            {
                // Metod för att fylla på byggår
            }
            if (!_context.Contacts.Any())
            {
                // Metod för att fylla på kontakter
            }
            if (!_context.Ratings.Any())
            {
                // Metod för att fylla på Ratings
            }
            if (!_context.RealEstates.Any())
            {
                // Metod för att fylla på RealEstates
            }
            if (!_context.Comments.Any())
            {
                // Metod för att fylla på kommentarer
            }

            //if (!_context.RealEstateTypes.Any())
            //{
            //    var realEstateTypes = new RealEstateType[]
            //    {
            //        new RealEstateType { Type = "Appartment" },
            //        new RealEstateType{ Type = "House" },
            //        new RealEstateType {Type = "Office" },
            //        new RealEstateType { Type= "Warehouse" }
            //    };
            //    _context.AddRange(realEstateTypes);
            //    _context.SaveChanges();
            //}

        }
    }
}
