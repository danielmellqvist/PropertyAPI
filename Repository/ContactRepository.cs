using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var contact = await FindByCondition(x => x.Telephone.Equals(telephone), trackChanges).SingleOrDefaultAsync();
            return contact;
        }

        public void CreateContact(Contact contact) => Create(contact);

        // marcus added
        public async Task<Contact> GetContactByUserId(int contactId, bool trackChanges)
        {
            return await FindByCondition(x => x.Id == contactId, trackChanges).SingleOrDefaultAsync();
        }
        
        
    }
}
