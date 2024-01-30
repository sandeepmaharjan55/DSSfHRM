using System.Data.Entity;
using System.Linq;

namespace TaskManager.MVC5.Model
{
    public class Repository : IRepository
    {
        private readonly TaskManagerDbContext context = new TaskManagerDbContext();
       
        public void Dispose()
        {
            context.Dispose();
        }
        /// <summary>
        /// insert entry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Insert<T>(T entity) where T : class
        {
            if (entity == null) return;
            context.Entry(entity).State = EntityState.Added;
            //context.Set<T>().Add(entity);
            context.SaveChanges();
        }
        /// <summary>
        /// get entryes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> GetAll<T>() where T : class
        {
            return context.Set<T>();
        }
        /// <summary>
        /// delete entry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(T entity) where T : class
        {
            if (entity == null) return;
            context.Entry(entity).State = EntityState.Deleted;
            //context.Set<T>().Remove(entity);
            context.SaveChanges();
        }
        /// <summary>
        /// update entry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(T entity) where T : class
        {
            if (entity == null) return;
            context.Set<T>().Attach(entity);
            context.Entry(entity).State=EntityState.Modified;
            context.SaveChanges();
        }
    }
}
