﻿using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Laptopy.Repositories.IRepositories;
using Laptopy.Data;

namespace Laptopy.Repositories
{
    public class Repository<T>:IRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            dbSet = dbContext.Set<T>();
        }

        public T Create(T entity)
        {
            dbSet.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }
        public void Create(List<T> entities)
        {
            dbSet.AddRange(entities);
            dbContext.SaveChanges();
        }
        public void Edit(T entity)
        {
            dbSet.Update(entity);
            dbContext.SaveChanges();
        }


        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            dbContext.SaveChanges();
        }
        public void Delete(List<T> entities)
        {
            dbSet.RemoveRange(entities);
            dbContext.SaveChanges();

        }
        //public void Commit() => dbContext.SaveChanges();

        public IQueryable<T> Get(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProps = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProps != null)
            {
                query = includeProps(query);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public T? GetOne(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProps = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true)
        {
            return Get(filter, includeProps, includes, tracked).FirstOrDefault();
        }


        public void Comitt()
        {
            dbContext.SaveChanges();
        }
    }
}
