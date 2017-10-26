using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;

namespace DataAccess
{
    public interface IRepository<T>
    {
        Task<IEnumerable<Book>> GetAll();
        Task<IEnumerable<Book>> Query(Expression<Func<T, bool>> predicate);
        Task<T> GetById(int id);
        Task InsertOrUpdate(T entity);
        Task Delete(int id);
    }
}