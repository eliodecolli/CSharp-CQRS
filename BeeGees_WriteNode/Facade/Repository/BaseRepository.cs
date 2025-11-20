using Microsoft.EntityFrameworkCore;

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
            context.Set<T>().Add(inData);
            return inData;
        }

        public T? Find(object id)
        {
            return context.Set<T>().Find(id);
        }

        public T[] Get()
        {
            return context.Set<T>().ToArray();
        }

        public T[] Get(Func<T, bool> predicate)
        {
            return context.Set<T>().AsEnumerable().Where(predicate).ToArray();
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
