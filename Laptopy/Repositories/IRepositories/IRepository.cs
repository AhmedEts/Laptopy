using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Laptopy.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        public T Create(T entity);
        public void Create(List<T> entities);
        public void Edit(T entity);
        public void Comitt();
        public void Delete(T entity);
        public void Delete(List<T> entities);


        public IQueryable<T> Get(
         Expression<Func<T, bool>>? filter = null,
        //Expression<IIncludableQueryable<T, object>>[]? thenincludes = null,
         Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProps = null,
        Expression<Func<T, object>>[]? includes = null,
        bool tracked = true);

        public T? GetOne(
            Expression<Func<T, bool>>? filter = null,
             //Expression<IIncludableQueryable<T, object>>[]? thenincludes = null,
             Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProps = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true);
    }
}
