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
        public async Task<(bool Success, string Message,Exception? ex)> Commit()
        {
            try
            {
                await _context.SaveChangesAsync();
                return (true, "success",null);
            }
            catch(Exception ex)
            {
                return (false, ex.Message,ex);
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

        public async Task<(bool Success, string Message,Exception? ex)> AddSave<TEntity>(TEntity entity) where TEntity : class,IEntity
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync();
                return (true, "success",null);
            }
            catch(Exception ex)
            {
                return (false, ex.Message,ex);
            }
        }

        public async Task<(bool Success, string Message, Exception? ex)> AddSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class,IEntity
        {
            try
            {
                _context.Set<TEntity>().AddRange(items);
                await _context.SaveChangesAsync();
                return (true, "success",null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message,ex);
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

        public async Task<(bool Success, string Message, Exception? ex)> UpdateSave<TEntity>(TEntity entity) where TEntity :class, IEntity
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                await _context.SaveChangesAsync();
                return (true, "success",null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message,ex);
            }
        }

        public async Task<(bool Success, string Message, Exception? ex)> UpdateSave<TEntity>(IEnumerable<TEntity> items) where TEntity : class, IEntity
        {
            try
            {
                _context.Set<TEntity>().UpdateRange(items);
                await _context.SaveChangesAsync();
                return (true, "success",null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message,ex);
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

        public async Task<(bool Success, string Message, Exception? ex)> DeleteSave<TEntity>(TEntity entity) where TEntity :class, IEntity
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
                return (true, "success",null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message,ex);
            }
        }

        public async Task<(bool Success, string Message, Exception? ex)> DeleteSave<TEntity>(IEnumerable<TEntity> items) where TEntity :class, IEntity
        {
            try
            {
                _context.Set<TEntity>().RemoveRange(items);
                await _context.SaveChangesAsync();
                return (true, "success",null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message,ex);
            }
        }

        #endregion

        #region Query
        public void ExecuteQuery(string query)
        {
            _context.Database.ExecuteSqlRaw(query);
        }

        public async Task<(bool Success, string Message, Exception? ex)> ExecuteQuerySave(string query)
        {
            try
            {
                _context.Database.ExecuteSqlRaw(query);
                await _context.SaveChangesAsync();
                return (true, "success",null);
            }
            catch(Exception ex)
            {
                return (false, ex.Message,ex);
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
        public async Task<(bool Success, string Message, List<T> Result, Exception? ex)> ListQuery<T>(string query) where T : class
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
                return (true, "success", result.ToList(),null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null,ex);
            }
        }

        public async Task<(bool Success, string Message, T Result, Exception? ex)> SingleQuery<T>(string query) where T : class
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
                return (true, "success", result,null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null,ex);
            }
        }
        public async Task<(bool Success, string Message, List<Dictionary<string, string>> Result, Exception ex)> DynamicQuery(string query)
        {
            try
            {
                List<Dictionary<string, string>> listDictionary = new List<Dictionary<string, string>>();
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                        columns = columns.Distinct().ToList();
                        while (reader.Read())
                        {
                            var dictionary = new Dictionary<string, string>();
                            foreach (var column in columns)
                            {
                                dictionary.Add(column, reader[column].ToString());
                            }
                            listDictionary.Add(dictionary);
                        }
                    }
                    await _context.Database.CloseConnectionAsync();
                }
                return (true, "Success", listDictionary, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null, ex);
            }
        }
        #endregion


    }
}
