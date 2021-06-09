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
        /// <summary>
        /// This method ensures that the data base is created and that it contains data from the start.
        /// </summary>
        /// <param name="_context"></param>
        public static void Initialize(PropertyContext _context)
        {
            _context.Database.EnsureCreated();

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

            // TODO! Droppa databasen och koppla identiteterna på IdentityDatabase istället!
            if (!_context.Users.Any())
            {
                var users = new User[]
                {
                    new User { UserName = "Tykepony" },
                    new User { UserName = "Dumbocockroach" },
                    new User { UserName = "Anarchistibis" },
                    new User { UserName = "Doofusyak" },
                    new User { UserName = "Squirrelfink" },
                    new User { UserName = "Bogeyman" },
                    new User { UserName = "Gasbagcur" },
                    new User { UserName = "Mutantkitten" },
                    new User { UserName = "kangaroogeek" },
                    new User { UserName = "gorillaunicorn" }
                };
                _context.AddRange(users);
                _context.SaveChanges();
            }

            if (!_context.Contacts.Any())
            {
                var contacts = new Contact[]
                {
                    new Contact { UserId = _context.Users.First(e => e.UserName.Contains("Tyke")).Id },
                    new Contact { UserId = _context.Users.First(e => e.UserName.Contains("roach")).Id },
                    new Contact { UserId = _context.Users.First(e => e.UserName.Contains("bis")).Id },
                    new Contact { UserId = _context.Users.First(e => e.UserName.Contains("syak")).Id },
                    new Contact { UserId = _context.Users.First(e => e.UserName.Contains("man")).Id },
                    new Contact { UserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id },
                    new Contact { Telephone = 8156644497},
                    new Contact { Telephone = 2025550470},
                    new Contact { Telephone = 5074045838},
                    new Contact { Telephone = 5072400020}
                };
                _context.AddRange(contacts);
                _context.SaveChanges();
            }

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
                        CreatedUtc = new DateTime(2021,01,10,08,00,00, DateTimeKind.Utc)
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
                        CreatedUtc = new DateTime(2021,05,20,17,30,00, DateTimeKind.Utc)
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
                        CreatedUtc = new DateTime(2020,09,10,06,00,00, DateTimeKind.Utc)
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
                        CreatedUtc = new DateTime(2020,12,24,13,00,00, DateTimeKind.Utc)
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
                        CreatedUtc = new DateTime(2021,06,10,09,00,00, DateTimeKind.Utc)
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
                        CreatedUtc = new DateTime(2021,03,10,08,00,00, DateTimeKind.Utc)
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
                        CreatedUtc = new DateTime(2006,01,10,08,00,00, DateTimeKind.Utc)
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
                        CreatedUtc = new DateTime(2021,01,25,18,00,00, DateTimeKind.Utc)
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
                        CreatedUtc = new DateTime(2021,05,05,08,00,00, DateTimeKind.Utc)
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
                        CreatedUtc = new DateTime(1995,01,01,08,00,00, DateTimeKind.Utc)
                    }
                };
                _context.AddRange(realEstates);
                _context.SaveChanges();
            }
            if (!_context.Ratings.Any())
            {
                var ratings = new Rating[]
                {
                    new Rating
                    {
                        RatingValue = 5,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("Tyke")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id
                    },
                    new Rating
                    {
                        RatingValue = 4,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("Tyke")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("kitten")).Id
                    },
                    new Rating
                    {
                        RatingValue = 3,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("roach")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("Tyke")).Id
                    },
                    new Rating
                    {
                        RatingValue = 4,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("tibis")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("geek")).Id
                    },
                    new Rating
                    {
                        RatingValue = 4,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("geek")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("cur")).Id
                    },
                    new Rating
                    {
                        RatingValue = 5,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("man")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("fink")).Id
                    },
                    new Rating
                    {
                        RatingValue = 3,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("unicorn")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("geek")).Id
                    },
                    new Rating
                    {
                        RatingValue = 2,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("syak")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("fink")).Id
                    },
                    new Rating
                    {
                        RatingValue = 3,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("fink")).Id,
                        AboutUserId = _context.Users.First(e => e.UserName.Contains("syak")).Id
                    },
                    new Rating
                    {
                        RatingValue = 3,
                        ByUserId = _context.Users.First(e => e.UserName.Contains("bis")).Id,
                        AboutUserId  = _context.Users.First(e => e.UserName.Contains("cur")).Id
                    }
                };
                _context.AddRange(ratings);
                _context.SaveChanges();
            }
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
                    }
                };
                _context.AddRange(comments);
                _context.SaveChanges();
            }
        }
    }
}
