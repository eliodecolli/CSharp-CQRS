using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BeeGees_WriteNode.Facade.Repository
{
    public class BaseRepository<T> where T : class, new()
    {
        private readonly DbContext context;

        public BaseRepository(DbContext context)
        {
            this.context = context;
        }

        public T InsertNew(T inData)
        {
           return context.Set<T>().Add(inData);
        }

        public T Find(object id)
        {
            return context.Set<T>().Find(id);
        }

        public T[] Get()
        {
            return context.Set<T>().AsQueryable().ToArray();
        }

        public T[] Get(Func<T, bool> predicate)
        {
            return context.Set<T>().AsQueryable().Where(predicate).ToArray();
        }

        public void Update(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Modified;
        }
    }
}
