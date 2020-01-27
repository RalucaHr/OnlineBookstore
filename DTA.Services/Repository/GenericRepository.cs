using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DTA.Database;

namespace DTA.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private LibraryDbContext _context = null;
        private DbSet<T> table = null;
        public GenericRepository()
        {
            this._context = new LibraryDbContext();
            table = _context.Set<T>();
        }
        public GenericRepository(LibraryDbContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] properties)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            var query = table as IQueryable<T>;

            query = properties
                       .Aggregate(query, (current, property) => current.Include(property));

            return query.AsNoTracking().ToList();
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

    }
}
