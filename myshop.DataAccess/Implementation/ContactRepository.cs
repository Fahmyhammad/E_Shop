using myshop.Entities.Models;
using myshop.Entities.Repository;
using myshop.myshop.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class ContactRepository : GenericRepoistory<ContactUs>,IContactRepository
    {
        private readonly AppDbContext _db;
        public ContactRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public List<ContactUs> Search(string? entity)
        {
            return _db.contactUs.Where(x => x.FullName.ToUpper().Contains(entity) || x.Message.ToUpper().Contains(entity)).ToList();
        }
    }
    
}
