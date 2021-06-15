using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(PropertyContext context) : base(context)
        {

        }

        public async Task<Contact> GetContactByTelephoneAsync(string telephone, bool trackChanges)
        {
            var cleanTelephone = Regex.Replace(telephone, @"[^0-9]+", "");
            var contacts = await FindAll(trackChanges).ToListAsync();
            foreach (var contact in contacts)
            {
                var cleanNumber = Regex.Replace(contact.Telephone, @"[^0-9]+", "");
                // this checks contains as it is possible that +46...... or vice versa is added in to the database and when cleaned will not take country code into account
                if (cleanNumber.Contains(cleanTelephone) || cleanTelephone.Contains(cleanNumber))
                {
                    return contact;
                }
            }
            return null;
        }

        public void CreateContact(Contact contact) => Create(contact);

        // marcus added
        public async Task<Contact> GetContactByUserIdAsync(int contactId, bool trackChanges)
        {
            return await FindByCondition(x => x.Id == contactId, trackChanges).SingleOrDefaultAsync();
        }
        
        
    }
}
