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
        Task<(bool Success, string Message)> AddSave<TEntity>(TEntity entity) where TEntity : class, IEntity;

        Task<(bool Success, string Message)> AddSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class,IEntity;

        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;
        void Update<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        Task<(bool Success, string Message)> UpdateSave<TEntity>(TEntity entity) where TEntity : class, IEntity;
        Task<(bool Success, string Message)> UpdateSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;
        void Delete<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        Task<(bool Success, string Message)> DeleteSave<TEntity>(TEntity entity) where TEntity : class, IEntity;
        Task<(bool Success, string Message)> DeleteSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity;
        Task<List<TEntity>> List<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        Task<TEntity> Single<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        Task<int> Count<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        Task<bool> Any<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity;
        void ExecuteQuery(string query);
        Task<(bool Success, string Message)> ExecuteQuerySave(string query);
        Task<(bool Success, string Message, T Result)> SingleQuery<T>(string query) where T : class;
        Task<(bool Success, string Message, List<T> Result)> ListQuery<T>(string query) where T : class;
        Task<(bool Success, string Message)> Commit();

    }
}
