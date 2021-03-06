using Entities.Models;
using LoggerService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Initializer
{
    public class RealEstateInitializer
    {
        /// <summary>
        /// This method fills the Properties database with data.
        /// </summary>
        /// <param name="_context"></param>
        public static void Initialize(PropertyContext _context, ILoggerManager logger)
        {
            // Adds the realestate types (we added two of our own).
            if (!_context.RealEstateTypes.Any())
            {
                var realEstateTypes = new RealEstateType[]
                {
                    new RealEstateType { Type = "House" },
                    new RealEstateType{ Type = "Appartment" },
                    new RealEstateType {Type = "Office" },
                    new RealEstateType { Type= "Fort" },
                    new RealEstateType { Type = "Hobbit hole" },
                    new RealEstateType { Type = "Warehouse" }
                };
                _context.AddRange(realEstateTypes);
                _context.SaveChanges();
            }
            // Adds the different construction years
            if (!_context.ConstructionYears.Any())
            {
                var constructionYears = new ConstructionYear[]
                {
                    new ConstructionYear { Year = 1650 },
                    new ConstructionYear { Year = 1957 },
                    new ConstructionYear { Year = 1890 },
                    new ConstructionYear { Year = 1995 },
                    new ConstructionYear { Year = 1800 },
                    new ConstructionYear { Year = 1976 },
                    new ConstructionYear { Year = 1998 }
                };
                _context.AddRange(constructionYears);
                _context.SaveChanges();
            }
            // Creates contacts, connects to the users in the database
            if (!_context.Contacts.Any())
            {
                var contacts = new Contact[]
                {
                    new Contact { 
                        UserId = _context.Users.First(e => e.UserName.Contains("tyke")).Id,
                        Telephone = "6595-1235"
                    },
                    new Contact { 
                        UserId = _context.Users.First(e => e.UserName.Contains("roach")).Id,
                        Telephone = "8555-64498"
                    },
                    new Contact { 
                        UserId = _context.Users.First(e => e.UserName.Contains("bis")).Id,
                        Telephone = "123456789636"
                    },
                    new Contact { 
                        UserId = _context.Users.First(e => e.UserName.Contains("syak")).Id,
                        Telephone = "5555-5555"
                    },
                    new Contact { 
                        UserId = _context.Users.First(e => e.UserName.Contains("man")).Id,
                        Telephone = "9982111112"
                    },
                    new Contact { 
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        Telephone = "911"
                    },
                    new Contact { Telephone = "8156-644-497"},
                    new Contact { Telephone = "2025-550-470"},
                    new Contact { Telephone = "5074-045-838"},
                    new Contact { Telephone = "5072-400-020"}
                };
                _context.AddRange(contacts);
                _context.SaveChanges();
            }
            // Adds the realestates, finds the correct foreign keys throu lambda expressions
            if (!_context.RealEstates.Any())
            {
                var realEstates = new RealEstate[]
                {
                    new RealEstate
                    {
                        Title = "Fantastic Fort for sale!",
                        Description = "Seize this oppertunity to procure a fort that is as old as the city itself!",
                        Street = "Leijonsparres Väg 15",
                        ZipCode = 41304,
                        City = "Gothenburg",
                        Country = "Sweden",
                        ContactId = 1,
                        ConstructionYearId = _context.ConstructionYears.First(x => x.Year == 1650).Id,
                        SellingPrice = 8000000,
                        CanBeSold = true,
                        CanBeRented = false,
                        RealEstateTypeId = _context.RealEstateTypes.First(x => x.Type == "Fort").Id,
                        CreatedUtc = new DateTime(2021,05,01,12,00,00)
                    },
                    new RealEstate
                    {
                        Title = "Large Apartment with gracious sea view!",
                        Description = "The apartment of your dreams can become yours! Please contact for more information.",
                        Street = "Fictionary Address 27",
                        ZipCode = 22222,
                        City = "Gothenburg",
                        Country = "Sweden",
                        ContactId = 2,
                        ConstructionYearId = _context.ConstructionYears.First(x => x.Year == 1957).Id,
                        RentingPrice = 22000,
                        CanBeSold = false,
                        CanBeRented = true,
                        RealEstateTypeId = _context.RealEstateTypes.First(x => x.Type == "Appartment").Id,
                        CreatedUtc = new DateTime(2021,04,01,13,00,00)
                    },
                    new RealEstate
                    {
                        Title = "Spacious Apartment for sale",
                        Description = "The apartment of your dreams can become yours! Please contact for more information.",
                        Street = "Fake Avenue 87",
                        ZipCode = 55555,
                        City = "Stockholm",
                        Country = "Sweden",
                        ContactId = 3,
                        ConstructionYearId = _context.ConstructionYears.First(x => x.Year == 1890).Id,
                        SellingPrice = 7000000,
                        CanBeSold = true,
                        CanBeRented = false,
                        RealEstateTypeId = _context.RealEstateTypes.First(x => x.Type == "Appartment").Id,
                        CreatedUtc = new DateTime(2021,01,10,11,00,00)
                    },
                    new RealEstate
                    {
                        Title = "Office space for rent",
                        Description = "The six-story Property was originally constructed in 1976 and there were significant capital improvements over the last several years including replacement of the roof, re-piping of the water and drain lines, new fire panel, and new HVAC fans.​ ",
                        Street = "1959  Denver Avenue",
                        ZipCode = 91761,
                        City = "Ontario",
                        Country = "Canada",
                        ContactId = 4,
                        ConstructionYearId = _context.ConstructionYears.First(x => x.Year == 1976).Id,
                        RentingPrice = 20000,
                        CanBeSold = false,
                        CanBeRented = true,
                        RealEstateTypeId = _context.RealEstateTypes.First(x => x.Type == "Office").Id,
                        CreatedUtc = new DateTime(2020,12,31,23,00,00)
                    },
                    new RealEstate
                    {
                        Title = "House for sale in central Nocross",
                        Description = "Incredible 4 bedroom, 2.5 bathroom, 2-story home featuring a 2-car attached garage, a shaded front porch, and a sparkling pool!",
                        Street = "366  Layman Court",
                        ZipCode = 30071,
                        City = "Norcross",
                        Country = "USA",
                        ContactId = 5,
                        ConstructionYearId = _context.ConstructionYears.First(x => x.Year == 1995).Id,
                        SellingPrice = 2000000,
                        CanBeSold = true,
                        CanBeRented = false,
                        RealEstateTypeId = _context.RealEstateTypes.First(x => x.Type == "House").Id,
                        CreatedUtc = new DateTime(2021,03,12,17,30,00)
                    },
                    new RealEstate
                    {
                        Title = "New lovely home in Baltimore for sale",
                        Description = "Charming 3 bedroom, 2 bathroom, single-family home located in the community of Baltimore. ",
                        Street = "3157  Hickory Heights Drive",
                        ZipCode = 21202,
                        City = "Baltimore",
                        Country = "USA",
                        ContactId = 6,
                        ConstructionYearId = _context.ConstructionYears.First(x => x.Year == 1957).Id,
                        SellingPrice = 2500000,
                        RentingPrice = 8000,
                        CanBeSold = true,
                        CanBeRented = true,
                        RealEstateTypeId = _context.RealEstateTypes.First(x => x.Type == "House").Id,
                        CreatedUtc = new DateTime(2021,02,01,11,55,00)
                    },
                    new RealEstate
                    {
                        Title = "House for sale just outside of Bloomfield",
                        Description = "Charming 4 bed, 2 bath home in Trillium Village. This home has a formal living and dining room, as well as family room.  The kitchen boasts ample cabinet and counter space, including closet pantry.",
                        Street = "2420  D Street",
                        ZipCode = 48302,
                        City = "Bloomfield Township",
                        Country = "USA",
                        ContactId = 7,
                        ConstructionYearId = _context.ConstructionYears.First(x => x.Year == 1890).Id,
                        SellingPrice = 2300000,
                        CanBeSold = true,
                        CanBeRented = false,
                        RealEstateTypeId = _context.RealEstateTypes.First(x => x.Type == "House").Id,
                        CreatedUtc = new DateTime(2021,04,18,08,30,00)
                    },
                    new RealEstate
                    {
                        Title = "You will love this Casa Blanca!",
                        Description = "Rent the White House east wing, pretend that you are the president of the United States for a month at a time",
                        Street = "1600 Pennsylvania Avenue",
                        ZipCode = 20500,
                        City = "Washington D.C",
                        Country = "USA",
                        ContactId = 8,
                        ConstructionYearId = _context.ConstructionYears.First(x => x.Year == 1800).Id,
                        RentingPrice = 70000,
                        CanBeSold = false,
                        CanBeRented = true,
                        RealEstateTypeId = _context.RealEstateTypes.First(x => x.Type == "House").Id,
                        CreatedUtc = new DateTime(2021,01,31,19,45,45)
                    },
                    new RealEstate
                    {
                        Title = "Beautiful House For Sale",
                        Description = "Impressive 5 bedroom house with 3 bathrooms and a large garden.",
                        Street = "4143  Raintree Boulevard",
                        ZipCode = 46225,
                        City = "Indianapolis",
                        Country = "USA",
                        ContactId = 9,
                        ConstructionYearId = _context.ConstructionYears.First(x => x.Year == 1995).Id,
                        SellingPrice = 5000000,
                        CanBeSold = true,
                        CanBeRented = false,
                        RealEstateTypeId = _context.RealEstateTypes.First(x => x.Type == "House").Id,
                        CreatedUtc = new DateTime(2020,12,23,12,00,00)
                    },
                    new RealEstate
                    {
                        Title = "Want to have a second breakfast?",
                        Description = "In this Hobbot hole you can live put the full Tolkien fantasy",
                        Street = "ShireRoad 5",
                        ZipCode = 58788,
                        City = "Hobbiton",
                        Country = "Shire",
                        ContactId = 10,
                        ConstructionYearId = _context.ConstructionYears.First(x => x.Year == 1998).Id,
                        SellingPrice = 9000000,
                        RentingPrice = 90000,
                        CanBeSold = true,
                        CanBeRented = true,
                        RealEstateTypeId = _context.RealEstateTypes.First(x => x.Type == "Hobbit hole").Id,
                        CreatedUtc = new DateTime(2021,05,27,10,36,18)
                    }
                };
                _context.AddRange(realEstates);
                _context.SaveChanges();
            }
            // Adds ratings, finds the correct foreign keys throu lambda expressions
            if (!_context.Ratings.Any())
            {
                var ratings = new Rating[]
                {
                    new Rating
                    {
                        Value = 5,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("Tyke")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id
                    },
                    new Rating
                    {
                        Value = 4,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("Tyke")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("kitten")).Id
                    },
                    new Rating
                    {
                        Value = 3,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("roach")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("Tyke")).Id
                    },
                    new Rating
                    {
                        Value = 4,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("tibis")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("geek")).Id
                    },
                    new Rating
                    {
                        Value = 4,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("geek")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("cur")).Id
                    },
                    new Rating
                    {
                        Value = 5,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("man")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("fink")).Id
                    },
                    new Rating
                    {
                        Value = 3,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("geek")).Id
                    },
                    new Rating
                    {
                        Value = 2,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("syak")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("fink")).Id
                    },
                    new Rating
                    {
                        Value = 3,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("fink")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("syak")).Id
                    },
                    new Rating
                    {
                        Value = 3,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("bis")).Id,
                        AboutUserId  = _context.Users.First(e => e.UserName.Contains("cur")).Id
                    }
                };
                _context.AddRange(ratings);
                _context.SaveChanges();
            }
            // Adds comments to the realestates
            if (!_context.Comments.Any())
            {
                var comments = new Comment[]
                {
                    new Comment
                    {
                        Content = "Love it! Give me a crossbow and I will defend it!",
                        CreatedOn = new DateTime(2021, 05, 05, 09, 30, 44, 58),
                        UserId = _context.Users.First(e => e.UserName.Contains("pony")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 41304).Id
                    },
                    new Comment
                    {
                        Content = "The bit about seaview was a bit overrated, but it was lovely!",
                        CreatedOn = new DateTime(2021, 04, 04, 10, 30, 40, 40),
                        UserId = _context.Users.First(e => e.UserName.Contains("roach")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 22222).Id
                    },
                    new Comment
                    {
                        Content = "Looks good! ",
                        CreatedOn = new DateTime(2021, 01, 27, 23, 47, 50, 10),
                        UserId = _context.Users.First(e => e.UserName.Contains("bis")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 55555).Id
                    },
                    new Comment
                    {
                        Content = "Nice, when can I come by and look at the place next year? Happy new year!",
                        CreatedOn = new DateTime(2020, 12, 31, 23, 59, 59, 10),
                        UserId = _context.Users.First(e => e.UserName.Contains("yak")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 91761).Id
                    },
                    new Comment
                    {
                        Content = "I was walking by that area yesterday when I saw this building, I was thinking it would be a great place to live in!",
                        CreatedOn = new DateTime(2021, 03, 14, 08, 14, 15, 16),
                        UserId = _context.Users.First(e => e.UserName.Contains("fink")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 30071).Id
                    },
                    new Comment
                    {
                        Content = "Looks like my dream house!",
                        CreatedOn = new DateTime(2021, 02, 14, 21, 25, 59, 23),
                        UserId = _context.Users.First(e => e.UserName.Contains("cur")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 21202).Id
                    },
                    new Comment
                    {
                        Content = "I think I should place a bid!",
                        CreatedOn = new DateTime(2021, 02, 14, 21, 26, 00, 23),
                        UserId = _context.Users.First(e => e.UserName.Contains("cur")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 21202).Id
                    },
                    new Comment
                    {
                        Content = "I placed a bid!",
                        CreatedOn = new DateTime(2021, 02, 14, 21, 26, 30, 23),
                        UserId = _context.Users.First(e => e.UserName.Contains("cur")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 21202).Id
                    },
                    new Comment
                    {
                        Content = "I placed another bid about twice as high as my first one... Nice!!",
                        CreatedOn = new DateTime(2021, 02, 14, 21, 27, 30, 23),
                        UserId = _context.Users.First(e => e.UserName.Contains("cur")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 21202).Id
                    },
                    new Comment
                    {
                        Content = "Why didnt anybody contact me yet???",
                        CreatedOn = new DateTime(2021, 02, 14, 21, 28, 01, 23),
                        UserId = _context.Users.First(e => e.UserName.Contains("cur")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 21202).Id
                    },
                    new Comment
                    {
                        Content = "...Someone is knockin on my door...",
                        CreatedOn = new DateTime(2021, 02, 14, 21, 28, 20, 23),
                        UserId = _context.Users.First(e => e.UserName.Contains("cur")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 21202).Id
                    },
                    new Comment
                    {
                        Content = "...HELP!!...",
                        CreatedOn = new DateTime(2021, 02, 14, 21, 29, 00, 23),
                        UserId = _context.Users.First(e => e.UserName.Contains("cur")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 21202).Id
                    },
                    new Comment
                    {
                        Content = "I dont want to go back to the hospital!!!",
                        CreatedOn = new DateTime(2021, 02, 14, 21, 30, 59, 23),
                        UserId = _context.Users.First(e => e.UserName.Contains("cur")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 21202).Id
                    },
                    new Comment
                    {
                        Content = "This doesn't fancy me at all.",
                        CreatedOn = new DateTime(2021, 04, 19, 12, 12, 12, 12),
                        UserId = _context.Users.First(e => e.UserName.Contains("kitten")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 48302).Id
                    },
                    new Comment
                    {
                        Content = "Trump has left his things all over the place!",
                        CreatedOn = new DateTime(2021, 02, 20, 11, 50, 40, 40),
                        UserId = _context.Users.First(e => e.UserName.Contains("man")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 20500).Id
                    },
                    new Comment
                    {
                        Content = "Looks fantastic, I would like to go there now!",
                        CreatedOn = new DateTime(2020, 12, 24, 15, 00, 00, 00),
                        UserId = _context.Users.First(e => e.UserName.Contains("geek")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 46225).Id
                    },
                    new Comment
                    {
                        Content = "I just wan to go there, have an ale and then go out on adventures! ",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Love it! Give me a crossbow and I will defend it!",
                        CreatedOn = new DateTime(2021, 05, 05, 09, 30, 44, 58),
                        UserId = _context.Users.First(e => e.UserName.Contains("pony")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 41304).Id
                    },
                    new Comment
                    {
                        Content = "The bit about seaview was a bit overrated, but it was lovely!",
                        CreatedOn = new DateTime(2021, 04, 04, 10, 30, 40, 40),
                        UserId = _context.Users.First(e => e.UserName.Contains("roach")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 22222).Id
                    },
                    new Comment
                    {
                        Content = "Looks good! ",
                        CreatedOn = new DateTime(2021, 01, 27, 23, 47, 50, 10),
                        UserId = _context.Users.First(e => e.UserName.Contains("bis")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 55555).Id
                    },
                    new Comment
                    {
                        Content = "Nice, when can I come by and look at the place next year? Happy new year!",
                        CreatedOn = new DateTime(2020, 12, 31, 23, 59, 59, 10),
                        UserId = _context.Users.First(e => e.UserName.Contains("yak")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 91761).Id
                    },
                    new Comment
                    {
                        Content = "I was walking by that area yesterday when I saw this building, I was thinking it would be a great place to live in!",
                        CreatedOn = new DateTime(2021, 03, 14, 08, 14, 15, 16),
                        UserId = _context.Users.First(e => e.UserName.Contains("fink")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 30071).Id
                    },
                    new Comment
                    {
                        Content = "This doesn't fancy me at all.",
                        CreatedOn = new DateTime(2021, 04, 19, 12, 12, 12, 12),
                        UserId = _context.Users.First(e => e.UserName.Contains("kitten")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 48302).Id
                    },
                    new Comment
                    {
                        Content = "Trump has left his things all over the place!",
                        CreatedOn = new DateTime(2021, 02, 20, 11, 50, 40, 40),
                        UserId = _context.Users.First(e => e.UserName.Contains("man")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 20500).Id
                    },
                    new Comment
                    {
                        Content = "Looks fantastic, I would like to go there now!",
                        CreatedOn = new DateTime(2020, 12, 24, 15, 00, 00, 00),
                        UserId = _context.Users.First(e => e.UserName.Contains("geek")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 46225).Id
                    },
                    new Comment
                    {
                        Content = "I just wan to go there, have an ale and then go out on adventures! ",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Love it! Give me a crossbow and I will defend it!",
                        CreatedOn = new DateTime(2021, 05, 05, 09, 30, 44, 58),
                        UserId = _context.Users.First(e => e.UserName.Contains("pony")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 41304).Id
                    },
                    new Comment
                    {
                        Content = "The bit about seaview was a bit overrated, but it was lovely!",
                        CreatedOn = new DateTime(2021, 04, 04, 10, 30, 40, 40),
                        UserId = _context.Users.First(e => e.UserName.Contains("roach")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 22222).Id
                    },
                    new Comment
                    {
                        Content = "Looks good! ",
                        CreatedOn = new DateTime(2021, 01, 27, 23, 47, 50, 10),
                        UserId = _context.Users.First(e => e.UserName.Contains("bis")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 55555).Id
                    },
                    new Comment
                    {
                        Content = "Nice, when can I come by and look at the place next year? Happy new year!",
                        CreatedOn = new DateTime(2020, 12, 31, 23, 59, 59, 10),
                        UserId = _context.Users.First(e => e.UserName.Contains("yak")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 91761).Id
                    },
                    new Comment
                    {
                        Content = "I was walking by that area yesterday when I saw this building, I was thinking it would be a great place to live in!",
                        CreatedOn = new DateTime(2021, 03, 14, 08, 14, 15, 16),
                        UserId = _context.Users.First(e => e.UserName.Contains("fink")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 30071).Id
                    },
                    new Comment
                    {
                        Content = "This doesn't fancy me at all.",
                        CreatedOn = new DateTime(2021, 04, 19, 12, 12, 12, 12),
                        UserId = _context.Users.First(e => e.UserName.Contains("kitten")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 48302).Id
                    },
                    new Comment
                    {
                        Content = "Trump has left his things all over the place!",
                        CreatedOn = new DateTime(2021, 02, 20, 11, 50, 40, 40),
                        UserId = _context.Users.First(e => e.UserName.Contains("man")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 20500).Id
                    },
                    new Comment
                    {
                        Content = "Looks fantastic, I would like to go there now!",
                        CreatedOn = new DateTime(2020, 12, 24, 15, 00, 00, 00),
                        UserId = _context.Users.First(e => e.UserName.Contains("geek")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 46225).Id
                    },
                    new Comment
                    {
                        Content = "I just wan to go there, have an ale and then go out on adventures! ",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Love it! Give me a crossbow and I will defend it!",
                        CreatedOn = new DateTime(2021, 05, 05, 09, 30, 44, 58),
                        UserId = _context.Users.First(e => e.UserName.Contains("pony")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 41304).Id
                    },
                    new Comment
                    {
                        Content = "The bit about seaview was a bit overrated, but it was lovely!",
                        CreatedOn = new DateTime(2021, 04, 04, 10, 30, 40, 40),
                        UserId = _context.Users.First(e => e.UserName.Contains("roach")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 22222).Id
                    },
                    new Comment
                    {
                        Content = "Looks good! ",
                        CreatedOn = new DateTime(2021, 01, 27, 23, 47, 50, 10),
                        UserId = _context.Users.First(e => e.UserName.Contains("bis")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 55555).Id
                    },
                    new Comment
                    {
                        Content = "Nice, when can I come by and look at the place next year? Happy new year!",
                        CreatedOn = new DateTime(2020, 12, 31, 23, 59, 59, 10),
                        UserId = _context.Users.First(e => e.UserName.Contains("yak")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 91761).Id
                    },
                    new Comment
                    {
                        Content = "I was walking by that area yesterday when I saw this building, I was thinking it would be a great place to live in!",
                        CreatedOn = new DateTime(2021, 03, 14, 08, 14, 15, 16),
                        UserId = _context.Users.First(e => e.UserName.Contains("fink")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 30071).Id
                    },
                    new Comment
                    {
                        Content = "Polarised impactful local area network",
                        CreatedOn = new DateTime(2021, 04, 19, 12, 12, 12, 12),
                        UserId = _context.Users.First(e => e.UserName.Contains("kitten")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 48302).Id
                    },
                    new Comment
                    {
                        Content = "Future-proofed foreground success",
                        CreatedOn = new DateTime(2021, 02, 20, 11, 50, 40, 40),
                        UserId = _context.Users.First(e => e.UserName.Contains("man")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 20500).Id
                    },
                    new Comment
                    {
                        Content = "Looks fantastic, I would like to go there now!",
                        CreatedOn = new DateTime(2020, 12, 24, 15, 00, 00, 00),
                        UserId = _context.Users.First(e => e.UserName.Contains("geek")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 46225).Id
                    },
                    new Comment
                    {
                        Content = "Synergistic systemic Graphic Interface",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Re-contextualized responsive capacity",
                        CreatedOn = new DateTime(2021, 05, 05, 09, 30, 44, 58),
                        UserId = _context.Users.First(e => e.UserName.Contains("pony")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 41304).Id
                    },
                    new Comment
                    {
                        Content = "The bit about seaview was a bit overrated, but it was lovely!",
                        CreatedOn = new DateTime(2021, 04, 04, 10, 30, 40, 40),
                        UserId = _context.Users.First(e => e.UserName.Contains("roach")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 22222).Id
                    },
                    new Comment
                    {
                        Content = "Looks good! ",
                        CreatedOn = new DateTime(2021, 01, 27, 23, 47, 50, 10),
                        UserId = _context.Users.First(e => e.UserName.Contains("bis")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 55555).Id
                    },
                    new Comment
                    {
                        Content = "Nice, when can I come by and look at the place next year? Happy new year!",
                        CreatedOn = new DateTime(2020, 12, 31, 23, 59, 59, 10),
                        UserId = _context.Users.First(e => e.UserName.Contains("yak")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 91761).Id
                    },
                    new Comment
                    {
                        Content = "Grass-roots leading edge array",
                        CreatedOn = new DateTime(2021, 03, 14, 08, 14, 15, 16),
                        UserId = _context.Users.First(e => e.UserName.Contains("fink")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 30071).Id
                    },
                    new Comment
                    {
                        Content = "This doesn't fancy me at all.",
                        CreatedOn = new DateTime(2021, 04, 19, 12, 12, 12, 12),
                        UserId = _context.Users.First(e => e.UserName.Contains("kitten")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 48302).Id
                    },
                    new Comment
                    {
                        Content = "Programmable intangible Graphical User Interface",
                        CreatedOn = new DateTime(2021, 02, 20, 11, 50, 40, 40),
                        UserId = _context.Users.First(e => e.UserName.Contains("man")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 20500).Id
                    },
                    new Comment
                    {
                        Content = "Looks fantastic, I would like to go there now!",
                        CreatedOn = new DateTime(2020, 12, 24, 15, 00, 00, 00),
                        UserId = _context.Users.First(e => e.UserName.Contains("geek")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 46225).Id
                    },
                    new Comment
                    {
                        Content = "Reduced fault-tolerant analyzer",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Re-engineered interactive superstructure",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Profit-focused hybrid installation",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Sharable global approach",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Synergistic well-modulated analyzer",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Multi-tiered client-server hub",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Decentralized upward-trending function",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Proactive fresh-thinking throughput",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "User-friendly dedicated Graphical User Interface",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Business-focused bifurcated success",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Diverse transitional process improvement",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Re-contextualized directional encoding",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Streamlined upward-trending time-frame",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    },
                    new Comment
                    {
                        Content = "Multi-tiered 24/7 ability",
                        CreatedOn = new DateTime(2021, 05,28, 14, 16, 50, 50),
                        UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        RealEstateId = _context.RealEstates.First(x => x.ZipCode == 58788).Id
                    }
                };
                _context.AddRange(comments);
                _context.SaveChanges();
            }
        }
    }
}
