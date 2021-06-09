using Entities;
using Entities.Models;
using Initializer;
using LoggerService.Contracts;
using Newtonsoft.Json;
using Repository;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Initializer
{
    public class RealEstateInitializer 
    {
        public static void Initialize(PropertyContext _context, ILoggerManager logger)
        {
           
            _context.Database.EnsureCreated();
            logger.LogInfo("Database Creation initiated");

            if (!_context.ConstructionYears.Any())
            {
                logger.LogInfo("Construction Year data inserting");
                string json = File.ReadAllText(@"D:\Programming\School Programming\Handelsakademin\BED20GB\WEB API\Final Project\FinalVsProject\Initializer\Initializer\JsonFiles\constructionyear.json");
                List<ConstructionYear> constructionYears = JsonConvert.DeserializeObject<List<ConstructionYear>>(json);
                foreach (var item in constructionYears)
                {
                    _context.ConstructionYears.Add(item);
                }
                _context.SaveChanges();
                logger.LogInfo("Construction Year Finished");
            }

            if (!_context.RealEstateTypes.Any())
            {
                logger.LogInfo("Real estate type data inserting");
                string json = File.ReadAllText(@"D:\Programming\School Programming\Handelsakademin\BED20GB\WEB API\Final Project\FinalVsProject\Initializer\Initializer\JsonFiles\realestatetype.json");
                List<RealEstateType> realEstateTypes = JsonConvert.DeserializeObject<List<RealEstateType>>(json);
                foreach (var item in realEstateTypes)
                {
                    _context.RealEstateTypes.Add(item);
                }
                _context.SaveChanges();
                logger.LogInfo("Real estate type data finished");
            }

            if (!_context.Users.Any())
            {
                logger.LogInfo("Real estate type data inserting");
                string json = File.ReadAllText(@"D:\Programming\School Programming\Handelsakademin\BED20GB\WEB API\Final Project\FinalVsProject\Initializer\Initializer\JsonFiles\users.json");
                List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                foreach (var item in users)
                {
                    _context.Users.Add(item);
                }
                _context.SaveChanges();
                logger.LogInfo("Real estate type data finished");
            }

            if (!_context.Contacts.Any())
            {
                logger.LogInfo("Contact data inserting");
                string json = File.ReadAllText(@"D:\Programming\School Programming\Handelsakademin\BED20GB\WEB API\Final Project\FinalVsProject\Initializer\Initializer\JsonFiles\contacts.json");
                List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
                foreach (var item in contacts)
                {
                    _context.Contacts.Add(item);
                }
                _context.SaveChanges();
                logger.LogInfo("Contact Data Finished");
            }

            if (!_context.Ratings.Any())
            {
                logger.LogInfo("Rating data inserting");
                string json = File.ReadAllText(@"D:\Programming\School Programming\Handelsakademin\BED20GB\WEB API\Final Project\FinalVsProject\Initializer\Initializer\JsonFiles\ratings.json");
                List<Rating> ratings = JsonConvert.DeserializeObject<List<Rating>>(json);
                foreach (var item in ratings)
                {
                    _context.Ratings.Add(item);
                }
                _context.SaveChanges();
                logger.LogInfo("Rating Data Finished");
            }

            if (!_context.RealEstates.Any())
            {
                logger.LogInfo("Realestate data inserting");
                string json = File.ReadAllText(@"D:\Programming\School Programming\Handelsakademin\BED20GB\WEB API\Final Project\FinalVsProject\Initializer\Initializer\JsonFiles\realestates.json");
                List<RealEstate> realestates = JsonConvert.DeserializeObject<List<RealEstate>>(json);
                foreach (var item in realestates)
                {
                    _context.RealEstates.Add(item);
                }
                _context.SaveChanges();
                logger.LogInfo("Realestate Data Finished");
            }
            if (!_context.Comments.Any())
            {
                logger.LogInfo("Comment data inserting");
                string json = File.ReadAllText(@"D:\Programming\School Programming\Handelsakademin\BED20GB\WEB API\Final Project\FinalVsProject\Initializer\Initializer\JsonFiles\comments.json");
                List<Comment> comments = JsonConvert.DeserializeObject<List<Comment>>(json);
                foreach (var item in comments)
                {
                    _context.Comments.Add(item);
                }
                _context.SaveChanges();
                logger.LogInfo("Comment data finished");

            }

           

            

            

            

            

            
        }
    }
}
