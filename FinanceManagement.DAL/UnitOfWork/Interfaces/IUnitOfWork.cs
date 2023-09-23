namespace FinanceManagement.DAL.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        T GetRepository<T>();
        Task SaveAsync();
    }
}
