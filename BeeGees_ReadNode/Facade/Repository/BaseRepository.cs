using Microsoft.EntityFrameworkCore;

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
            dbSet.Add(inData);
            return inData;
        }

        public T? Find(object id)
        {
            return dbSet.Find(id);
        }

        public T[] Get()
        {
            return dbSet.ToArray();
        }

        public T[] Get(Func<T, bool> predicate)
        {
            return dbSet.AsEnumerable().Where(predicate).ToArray();
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
