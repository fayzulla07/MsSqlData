using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SqlData.SqlGenerator
{
    public interface ISqlData
    {   
         IEnumerable<T> GetAll<T>(string param = "*");
         Task<IEnumerable<T>> GetAllAsync<T>(string param = "*");
         IEnumerable<T> Find<T>(T pksFields, string param = "*", bool allowNulls = false);
         Task<IEnumerable<T>> FindAsync<T>(T pksFields, string param = "*", bool allowNulls = false);
         T GetById<T>(int key, string param = "*");
         Task<T> GetByIdAsync<T>(int key);
         IEnumerable<T> GetByAny<T>(string ColumnName, object value);
         Task<IEnumerable<T>> GetByAnyAsync<T>(string ColumnName, object value);
         int Add<T>(T entity, bool allowNulls = false);
         Task<int> AddAsync<T>(T entity, bool allowNulls = false);
         int Add<T>(IEnumerable<T> entity, bool allowNulls = false);
          Task<int> AddAsync<T>(IEnumerable<T> entity, bool allowNulls = false);
         int Remove<T>(int Id);
         Task<int> RemoveAsync<T>(int Id);
         int Update<T>(T entity, bool allowNulls = false);
         Task<int> UpdateAsync<T>(T entity, bool allowNulls = false);
         int InstertOrUpdate<T>(T entity);
         Task<int> InstertOrUpdateAsync<T>(T entity);

        IEnumerable<T> SqlQuery<T>(string query, object parameters = null);
        int SqlExecute(string query, object parameters = null);
        Task<IEnumerable<T>> SqlQueryAsync<T>(string query, object parameters = null);
         Task<int> SqlExecuteAsync(string query, object parameters = null);

        // Sql query
        IEnumerable<dynamic> SqlQueryDynamic(string query, object parameters = null);
        Task<IEnumerable<dynamic>> SqlQueryAsyncDynamic(string query, object parameters = null);
        IEnumerable<object> SqlQueryObject(string query, object parameters = null);
        Task<IEnumerable<dynamic>> SqlQueryAsyncObject(string query, object parameters = null);


    }
}
