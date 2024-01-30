using System;
using System.Linq;

namespace TaskManager.MVC5.Model
{
    public interface IRepository : IDisposable
    {
        void Insert<T>(T entity) where T : class;
        IQueryable<T> GetAll<T>() where T : class;
        void Delete<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
    }
}
