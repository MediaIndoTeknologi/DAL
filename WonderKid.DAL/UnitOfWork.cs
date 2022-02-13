using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WonderKid.DAL.Interface;

namespace WonderKid.DAL
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        public UnitOfWork(TDbContext context)
        {
            _context = context;
        }
        public IQueryable<TEntity> Entity<TEntity>() where TEntity : class,IEntity 
        {
            return _context.Set<TEntity>();
        }
        public async Task<(bool Success, string Message)> Commit()
        {
            try
            {
                await _context.SaveChangesAsync();
                return (true, "success");
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public void Dispose()
        {
            System.Threading.Thread.Sleep(1000);
            _context.Dispose();
        }

        #region Command

        #region Add
        public void Add<TEntity>(TEntity entity) where TEntity : class,IEntity
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Add<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity
        {
            _context.Set<TEntity>().AddRange(items);
        }

        public async Task<(bool Success, string Message)> AddSave<TEntity>(TEntity entity) where TEntity : class,IEntity
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync();
                return (true, "success");
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> AddSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class,IEntity
        {
            try
            {
                _context.Set<TEntity>().AddRange(items);
                await _context.SaveChangesAsync();
                return (true, "success");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        #endregion

        #region Update
        public void Update<TEntity>(TEntity entity) where TEntity :class, IEntity
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void Update<TEntity>(IEnumerable<TEntity> items) where TEntity :class, IEntity
        {
            _context.Set<TEntity>().UpdateRange(items);
        }

        public async Task<(bool Success, string Message)> UpdateSave<TEntity>(TEntity entity) where TEntity :class, IEntity
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                await _context.SaveChangesAsync();
                return (true, "success");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> UpdateSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity
        {
            try
            {
                _context.Set<TEntity>().UpdateRange(items);
                await _context.SaveChangesAsync();
                return (true, "success");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        #endregion

        #region Delete
        public void Delete<TEntity>(TEntity entity) where TEntity :class, IEntity
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Delete<TEntity>(IEnumerable<TEntity> items) where TEntity :class, IEntity
        {
            _context.Set<TEntity>().RemoveRange(items);
        }

        public async Task<(bool Success, string Message)> DeleteSave<TEntity>(TEntity entity) where TEntity :class, IEntity
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
                return (true, "success");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> DeleteSave<TEntity>(IEnumerable<TEntity> items) where TEntity :class, IEntity
        {
            try
            {
                _context.Set<TEntity>().RemoveRange(items);
                await _context.SaveChangesAsync();
                return (true, "success");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        #endregion

        #region Query
        public void ExecuteQuery(string query)
        {
            _context.Database.ExecuteSqlRaw(query);
        }

        public async Task<(bool Success, string Message)> ExecuteQuerySave(string query)
        {
            try
            {
                _context.Database.ExecuteSqlRaw(query);
                await _context.SaveChangesAsync();
                return (true, "success");
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }
        #endregion

        #endregion

        #region Query

        public async Task<List<TEntity>> List<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity
        {
            return await query.ToListAsync();
        }
        public async Task<TEntity> Single<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity
        {
            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> Count<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity
        {
            return await query.CountAsync();
        }
        public async Task<bool> Any<TEntity>(IQueryable<TEntity> query) where TEntity : IEntity
        {
            return await query.AnyAsync();
        }
        public async Task<(bool Success, string Message, List<T> Result)> ListQuery<T>(string query) where T : class
        {
            try
            {
                IDbConnection db = _context.Database.GetDbConnection();
                if (db == null)
                {
                    _context.Database.OpenConnection();
                    db = _context.Database.GetDbConnection();

                }

                var result = await db.QueryAsync<T>(query, null, commandType: CommandType.Text);
                return (true, "success", result.ToList());
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool Success, string Message, T Result)> SingleQuery<T>(string query) where T : class
        {
            try
            {
                IDbConnection db = _context.Database.GetDbConnection();
                if (db == null)
                {
                    _context.Database.OpenConnection();
                    db = _context.Database.GetDbConnection();

                }

                var result = await db.QueryFirstOrDefaultAsync<T>(query, null, commandType: CommandType.Text);
                return (true, "success", result);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
        #endregion


    }
}
