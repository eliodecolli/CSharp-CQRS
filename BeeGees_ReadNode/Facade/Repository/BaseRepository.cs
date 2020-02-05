using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BeeGees_ReadNode.Facade.Repository
{
    public class BaseRepository<T> where T : class, new()
    {
        private readonly DbSet<T> dbSet;
        private readonly DbContext context;

        public BaseRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public T InsertNew(T inData)
        {
           return dbSet.Add(inData);
        }

        public T Find(object id)
        {
            return dbSet.Find(id);
        }

        public T[] Get()
        {
            return dbSet.AsQueryable().ToArray();
        }

        public T[] Get(Func<T, bool> predicate)
        {
            return dbSet.AsQueryable().Where(predicate).ToArray();
        }

        public void Update(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Modified;
        }
    }
}
