using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DTA.Services
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] properties);
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
    }
}
