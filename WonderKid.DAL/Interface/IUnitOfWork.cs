using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderKid.DAL.Interface
{
    public interface IUnitOfWork<TDbContext>:IDisposable where TDbContext : DbContext
    {
        IQueryable<TEntity> Entity<TEntity>() where TEntity : class,IEntity;
        void Add<TEntity>(TEntity entity) where TEntity : class,IEntity;
        void Add<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        Task<(bool Success, string Message,Exception? ex)> AddSave<TEntity>(TEntity entity) where TEntity : class, IEntity;

        Task<(bool Success, string Message, Exception? ex)> AddSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class,IEntity;

        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;
        void Update<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        Task<(bool Success, string Message, Exception? ex)> UpdateSave<TEntity>(TEntity entity) where TEntity : class, IEntity;
        Task<(bool Success, string Message, Exception? ex)> UpdateSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;
        void Delete<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        Task<(bool Success, string Message, Exception? ex)> DeleteSave<TEntity>(TEntity entity) where TEntity : class, IEntity;
        Task<(bool Success, string Message, Exception? ex)> DeleteSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        Task<List<TEntity>> List<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        Task<TEntity> Single<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        Task<int> Count<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        Task<bool> Any<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        void ExecuteQuery(string query);
        Task<(bool Success, string Message, Exception? ex)> ExecuteQuerySave(string query);
        Task<(bool Success, string Message, T Result, Exception? ex)> SingleQuery<T>(string query) where T : class;
        Task<(bool Success, string Message, List<T> Result, Exception? ex)> ListQuery<T>(string query) where T : class;
        Task<(bool Success, string Message, List<Dictionary<string, string>> Result, Exception ex)> DynamicQuery(string query);
        Task<(bool Success, string Message, Exception? ex)> Commit();

    }
}
