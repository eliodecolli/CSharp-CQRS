namespace BeeGees_WriteNode.Facade.Repository
{
    public interface IRepository<T> where T : class, new()
    {
        T InsertNew(T inData);
        T? Find(object id);
        T[] Get();
        T[] Get(Func<T, bool> predicate);
        void Update(T entity);
    }
}
