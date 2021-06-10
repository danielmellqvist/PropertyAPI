using Entities;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private PropertyContext _context;
        private ICommentRepository _commentRepository;
        private IConstructionYearRepository _constructionYearRepository;
        private IContactRepository _contactRepository;
        private IRatingRepository _ratingRepository;
        private IRealEstateRepository _realEstateRepository;
        private IRealEstateTypeRepository _realEstateTypeReposityry;
        private IUserRepository _userRepository;

        public RepositoryManager(PropertyContext context)
        {
            _context = context;
        }




        public ICommentRepository Comment
        {
            get
            {
                if(_commentRepository == null)
                {
                    _commentRepository = new CommentRepository(_context);
                }
                return _commentRepository;
            }
        }

        public IConstructionYearRepository ConstructionYear
        {
            get
            {
                if (_constructionYearRepository == null)
                {
                    _constructionYearRepository = new ConstructionYearRepository(_context);
                }
                return _constructionYearRepository;
            }
        }

        public IContactRepository Contact
        {
            get
            {
                if (_contactRepository == null)
                {
                    _contactRepository = new ContactRepository(_context);
                }
                return _contactRepository;
            }
        }

        public IRatingRepository Rating
        {
            get
            {
                if (_ratingRepository == null)
                {
                    _ratingRepository = new RatingRepository(_context);
                }
                return _ratingRepository;
            }
        }

        public IRealEstateRepository RealEstate 
        {
            get
            {
                if (_realEstateRepository == null)
                {
                    _realEstateRepository = new RealEstateRepository(_context);
                }
                return _realEstateRepository;
            }
        }

        public IRealEstateTypeRepository RealEstateType
        {
            get
            {
                if (_realEstateTypeReposityry == null)
                {
                    _realEstateTypeReposityry = new RealEstateTypeRepository(_context);
                }
                return _realEstateTypeReposityry;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        public Task SaveAsync() => _context.SaveChangesAsync();
    }
}
