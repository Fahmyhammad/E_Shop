using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.Entities.Repository;
using myshop.myshop.DataAccess.Data;
using myshop.myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class AppUserRepoistory : GenericRepoistory<AppUser>,IAppUserRepository
    {
        private AppDbContext _db;
        public AppUserRepoistory(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public List<AppUser> Search(string? entity)
        {
            return _db.appUsers.Where(x => x.Name.ToUpper().Contains(entity)).ToList();
        }
    }
}
