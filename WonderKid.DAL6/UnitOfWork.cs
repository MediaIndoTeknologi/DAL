using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WonderKid.DAL6.Interface;

namespace WonderKid.DAL6
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        public UnitOfWork(TDbContext context)
        {
            _context = context;
        }
        public IQueryable<TEntity> Entity<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }
        public async Task<(bool Success, string Message, Exception? ex)> Commit()
        {
            try
            {
                await _context.SaveChangesAsync();
                return (true, "success", null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, ex);
            }
        }
        public void Dispose()
        {
            System.Threading.Thread.Sleep(1000);
            _context.Dispose();
        }

        #region Command

        #region Add
        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Add(entity);
            _context.Entry(entity).State = EntityState.Added;
        }

        public void Add<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            _context.Set<TEntity>().AddRange(items);
            _context.Entry(items).State = EntityState.Added;
        }
        public async Task<(bool Success, string Message, Exception? ex)> AddSave<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                _context.Entry(entity).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return (true, "success", null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, ex);
            }
        }
        public async Task<(bool Success, string Message, Exception? ex)> AddSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            try
            {
                foreach (var item in items)
                {
                    _context.Set<TEntity>().Add(item);
                    _context.Entry(item).State = EntityState.Added;
                }
                await _context.SaveChangesAsync();
                return (true, "success", null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, ex);
            }
        }
        #endregion

        #region Update
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Update<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            foreach (var item in items)
            {
                _context.Set<TEntity>().Attach(item);
                _context.Entry(item).State = EntityState.Modified;
            }
        }
        public async Task<(bool Success, string Message, Exception? ex)> UpdateSave<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _context.Set<TEntity>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return (true, "success", null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, ex);
            }
        }

        public async Task<(bool Success, string Message, Exception? ex)> UpdateSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            try
            {
                foreach (var item in items)
                {
                    _context.Set<TEntity>().Attach(item);
                    _context.Entry(item).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
                return (true, "success", null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, ex);
            }
        }
        #endregion

        #region Delete
        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Delete<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            _context.Set<TEntity>().RemoveRange(items);
        }
        public async Task<(bool Success, string Message, Exception? ex)> DeleteSave<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
                return (true, "success", null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, ex);
            }
        }
        public async Task<(bool Success, string Message, Exception? ex)> DeleteSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class
        {
            try
            {
                _context.Set<TEntity>().RemoveRange(items);
                await _context.SaveChangesAsync();
                return (true, "success", null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, ex);
            }
        }
        #endregion

        #region Query
        public async Task<(bool Success, string Message, Exception? ex)> ExecuteQuery(string query)
        {
            try
            {
                _context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, query);
                return (true, "success", null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, ex);
            }
        }
        #endregion

        #endregion

        #region Query

        public async Task<List<TEntity>> List<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            return await query.ToListAsync();
        }
        public async Task<TEntity> Single<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            return await query.FirstOrDefaultAsync();
        }
        public async Task<int> Count<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            return await query.CountAsync();
        }
        public async Task<bool> Any<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            return await query.AnyAsync();
        }
        public async Task<(bool Success, string Message, List<T> Result, Exception? ex)> ListQuery<T>(string query) where T : class
        {
            try
            {
                var result = await _context.Database.SqlQuery<T>(query).ToListAsync();
                return (true, "success", result.ToList(), null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null, ex);
            }
        }

        public async Task<(bool Success, string Message, T Result, Exception? ex)> SingleQuery<T>(string query) where T : class
        {
            try
            {
                var result = await _context.Database.SqlQuery<T>(query).SingleOrDefaultAsync();
                return (true, "success", result, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null, ex);
            }
        }
        #endregion

    }
}
