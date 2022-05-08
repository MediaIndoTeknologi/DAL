using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WonderKid.DAL6.Interface
{
    public interface IUnitOfWork<TDbContext>:IDisposable where TDbContext : DbContext
    {
        IQueryable<TEntity> Entity<TEntity>() where TEntity : class;
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void Add<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
        Task<(bool Success, string Message, Exception? ex)> AddSave<TEntity>(TEntity entity) where TEntity : class;
        Task<(bool Success, string Message, Exception? ex)> AddSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
        Task<(bool Success, string Message, Exception? ex)> UpdateSave<TEntity>(TEntity entity) where TEntity : class;
        Task<(bool Success, string Message, Exception? ex)> UpdateSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
        Task<(bool Success, string Message, Exception? ex)> DeleteSave<TEntity>(TEntity entity) where TEntity : class;
        Task<(bool Success, string Message, Exception? ex)> DeleteSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class;
        Task<List<TEntity>> List<TEntity>(IQueryable<TEntity> query) where TEntity : class;
        Task<TEntity> Single<TEntity>(IQueryable<TEntity> query) where TEntity : class;
        Task<int> Count<TEntity>(IQueryable<TEntity> query) where TEntity : class;
        Task<bool> Any<TEntity>(IQueryable<TEntity> query) where TEntity : class;
        Task<(bool Success, string Message, Exception? ex)> ExecuteQuery(string query);
        Task<(bool Success, string Message, T Result, Exception? ex)> SingleQuery<T>(string query) where T : class;
        Task<(bool Success, string Message, List<T> Result, Exception? ex)> ListQuery<T>(string query) where T : class;
        Task<(bool Success, string Message, Exception? ex)> Commit();

    }
}
