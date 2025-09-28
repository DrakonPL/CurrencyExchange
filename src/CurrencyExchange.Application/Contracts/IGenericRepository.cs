namespace CurrencyExchange.Application.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IReadOnlyList<T>> GetAll();
        Task<T> Add(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
