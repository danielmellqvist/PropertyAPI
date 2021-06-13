using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Areas.Identity.Data
{
    public class IdentityInitializer
    {
        /// <summary>
        /// Method that creates users in the Identity database
        /// </summary>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public async static Task<List<WebAPIUser>> AddUsersAsync(UserManager<WebAPIUser> userManager)
        {
            List<WebAPIUser> users = new()
            {
                new WebAPIUser
                {
                    Email = "tykepony@mail.com",
                    UserName = "tykepony@mail.com",
                    EmailConfirmed = true
                },
                new WebAPIUser
                {
                    Email = "dumbocockroach@mail.com",
                    UserName = "dumbocockroach@mail.com",
                    EmailConfirmed = true,
                },
                new WebAPIUser
                {
                    Email = "anarchistibis@mail.com",
                    UserName = "anarchistibis@mail.com",
                    EmailConfirmed = true
                },
                new WebAPIUser
                {
                    Email = "doofusyak@mail.com",
                    UserName = "doofusyak@mail.com",
                    EmailConfirmed = true
                },
                new WebAPIUser
                {
                    Email = "squirrelfink@mail.com",
                    UserName = "squirrelfink@mail.com",
                    EmailConfirmed = true
                },
                new WebAPIUser
                {
                    Email = "bogeyman@mail.com",
                    UserName = "bogeyman@mail.com",
                    EmailConfirmed = true
                },
                new WebAPIUser
                {
                    Email = "gasbagcur@mail.com",
                    UserName = "gasbagcur@mail.com",
                    EmailConfirmed = true
                },
                new WebAPIUser
                {
                    Email = "mutantkitten@mail.com",
                    UserName = "mutantkitten@mail.com",
                    EmailConfirmed = true
                },
                new WebAPIUser
                {
                    Email = "kangaroogeek@mail.com",
                    UserName = "kangaroogeek@mail.com",
                    EmailConfirmed = true
                },
                new WebAPIUser
                {
                    Email = "gorillaunicorn@mail.com",
                    UserName = "gorillaunicorn@mail.com",
                    EmailConfirmed = true
                }
            };
            foreach (var user in users)
            {
                if (userManager.Users.All(x => x.Id != user.Id))
                {
                    var newUser = await userManager.FindByEmailAsync(user.Email);
                    if (newUser is null)
                    {
                        await userManager.CreateAsync(user, "EveryOneHasTheSame!123");

                    }
                }
            }
            return users;
        }
        /// <summary>
        /// Method that takes the users of the Identity database and adds them to the Propery database table users
        /// </summary>
        /// <param name="context"></param>
        /// <param name="users"></param>
        public static void AddUsersToEstateDb(PropertyContext context, List<WebAPIUser> users)
        {

            foreach (var user in users)
            {
                var existingUser = context.Users.FirstOrDefault(x => x.UserName == user.Email);
                if (existingUser is null)
                {
                    var newUser = new User
                    {
                        UserName = user.Email,
                        IdentityUserId = Guid.Parse(user.Id)
                    };
                    context.Users.Add(newUser);
                }
            }
            context.SaveChanges();
        }
    }
}

