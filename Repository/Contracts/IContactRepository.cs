using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IContactRepository
    {
        void CreateContact(Contact contact);
        Task<Contact> GetContactByTelephoneAsync(string telephone, bool trackChanges);
    }
}
