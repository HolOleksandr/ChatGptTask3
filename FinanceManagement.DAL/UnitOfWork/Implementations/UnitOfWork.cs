using FinanceManagement.DAL.Data;
using FinanceManagement.DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace FinanceManagement.DAL.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinanceDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, object> _repositories;

        public UnitOfWork(FinanceDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<Type, object>();
            _serviceProvider = serviceProvider;
        }

        public T GetRepository<T>()
        {
            var type = typeof(T);
            _repositories.GetOrAdd(type, (_) => _serviceProvider.GetService<T>()
                                ?? throw new ArgumentNullException($"Repository {type.Name} doesn't exist"));
            return (T)_repositories[type];
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
