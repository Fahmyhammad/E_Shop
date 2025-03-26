using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.Entities.Repository;
using myshop.myshop.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class GenericRepoistory<T> : IGenericRepoistory<T> where T : class
    {
        private readonly AppDbContext _db;
        private DbSet<T> _dbSet;
        public GenericRepoistory(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? perdicate = null, string? IncludeWord = null, string? IncludeWord2 = null)
        {
            IQueryable<T> query = _dbSet;
            if(perdicate != null)
            {
                query = query.Where(perdicate);
            }
            if(IncludeWord != null)
            {
                foreach(var item in IncludeWord.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            if (IncludeWord2 != null)
            {
                foreach (var item in IncludeWord2.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }

        public T GetById(Expression<Func<T, bool>>? predicate = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }


        public void Remove(T entity)
        {
           _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

      
    }
}
