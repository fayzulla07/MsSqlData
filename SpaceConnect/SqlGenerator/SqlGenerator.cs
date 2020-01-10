using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SqlData.SqlGenerator
{
    public class SqlGenerator: ExecutableMethods, ISqlData
    {
        public SqlGenerator()
        {
        }
        // ----------------------------  Strong Type  ---------------------------- //
        // Get data from the database

        // Get everything from the database
        public IEnumerable<T> GetAll<T>(string param = "*")
        {
            var select = method.GenerateSelect<T>(param);
            return GetData<T>(select);
        }
        public async Task<IEnumerable<T>> GetAllAsync<T>(string param = "*")
        {
            var select = method.GenerateSelect<T>(param);
            return await GetDataAsync<T>(select);
        }

        // Find from the database if allowNulls = false then do not generate a request with empty properties
        public IEnumerable<T> Find<T>(T pksFields, string param = "*", bool allowNulls = false)
        {
            string qry = method.GenerateFind<T>(pksFields, param, allowNulls);
            return GetData<T>(qry, pksFields);
        }
        public async Task<IEnumerable<T>> FindAsync<T>(T pksFields, string param = "*", bool allowNulls = false)
        {
            string qry = method.GenerateFind<T>(pksFields, param, allowNulls);
            return await GetDataAsync<T>(qry, pksFields);
        }
        public T GetById<T>(int key, string param = "*")
        {
            if (key != 0)
            {
                T identity = (T)Activator.CreateInstance(typeof(T));
                var info = identity.GetType().GetProperty(method.GetKeyName<T>());
                info.SetValue(identity, Convert.ChangeType(key, info.PropertyType), null);
                string qry = method.GenerateSelect<T>();
                qry += method.GenerateById<T>();
                return GetData<T>(qry, identity).FirstOrDefault();
            }
            else return default(T);

        }
        // get by [key] id from database
        public async Task<T> GetByIdAsync<T>(int key)
        {
            if (key != 0)
            {
                T identity = (T)Activator.CreateInstance(typeof(T));
                var info = identity.GetType().GetProperty(method.GetKeyName<T>());
                info.SetValue(identity, Convert.ChangeType(key, info.PropertyType), null);
                string qry = method.GenerateSelect<T>();
                qry += method.GenerateById<T>();
                var result = await GetDataAsync<T>(qry, identity);
                return result.FirstOrDefault();
            }
            else return default(T);

        }
        // get by any from database may be from Id or name...
        public IEnumerable<T> GetByAny<T>(string ColumnName, object value)
        {
            T identity = (T)Activator.CreateInstance(typeof(T));
            var info = identity.GetType().GetProperty(ColumnName);
            info.SetValue(identity, Convert.ChangeType(value, info.PropertyType), null);
            string qry = method.GenerateSelect<T>();
            qry += method.GenerateById<T>(ColumnName);
            return GetData<T>(qry, identity);
        }
        public async Task<IEnumerable<T>> GetByAnyAsync<T>(string ColumnName, object value)
        {
            T identity = (T)Activator.CreateInstance(typeof(T));
            var info = identity.GetType().GetProperty(ColumnName);
            info.SetValue(identity, Convert.ChangeType(value, info.PropertyType), null);
            string qry = method.GenerateSelect<T>();
            qry += method.GenerateById<T>(ColumnName);
            return await GetDataAsync<T>(qry, identity);
        }

        // add a new item to database if allowNulls = false then do not generate a request with empty properties
        public int Add<T>(T entity, bool allowNulls = false)
        {
            string qry = method.GeneratePartInsert(entity, allowNulls);
            return Execute(qry, entity);
        }
        public async Task<int> AddAsync<T>(T entity, bool allowNulls = false)
        {
            string qry = method.GeneratePartInsert<T>(entity, allowNulls);
            return await ExecuteAsync(qry, entity);

        }

        public int Add<T>(IEnumerable<T> entity, bool allowNulls = false)
        {
            string qry = method.GeneratePartInsert<T>(entity.FirstOrDefault(), allowNulls);
            int result = Execute(qry, entity);
            return result;

        }
        public async Task<int> AddAsync<T>(IEnumerable<T> entity, bool allowNulls = false)
        {
            string qry = method.GeneratePartInsert(entity.FirstOrDefault(), allowNulls);
            var result = await ExecuteAsync(qry, entity);
            return result;
        }

        // delete item in database by id
        public int Remove<T>(int Id)
        {
            if (Id != 0)
            {
                T identity = (T)Activator.CreateInstance(typeof(T));
                var info = identity.GetType().GetProperty(method.GetKeyName<T>());
                info.SetValue(identity, Convert.ChangeType(Id, info.PropertyType), null);
                string qry = method.GenerateDelete<T>(method.GetKeyName<T>());
                var result = Execute(qry, identity);
                return result;
            }
            else
                return 0;

        }
        public async Task<int> RemoveAsync<T>(int Id)
        {
            if (Id != 0)
            {
                T identity = (T)Activator.CreateInstance(typeof(T));
                var info = identity.GetType().GetProperty(method.GetKeyName<T>());
                info.SetValue(identity, Convert.ChangeType(Id, info.PropertyType), null);
                string qry = method.GenerateDelete<T>(method.GetKeyName<T>());
                var result = await ExecuteAsync(qry, identity);
                return result;
            }
            else
                return 0;

        }

        // update item in database
        public int Update<T>(T entity, bool allowNulls = false)
        {
            string qry = method.GenerateUpdate<T>(entity, allowNulls);
            return Execute(qry, entity);
        }
        public async Task<int> UpdateAsync<T>(T entity, bool allowNulls = false)
        {
            string qry = method.GenerateUpdate<T>(entity, allowNulls);
            return await ExecuteAsync(qry, entity);
        }

        // if the item exists then update else we will add a new item
        public int InstertOrUpdate<T>(T entity)
        {
            var qry = method.GenerateSelect<T>();
            var ob = GetData<T>(qry);
            if (ob == null)
            {
                qry = method.GeneratePartInsert<T>(entity, false);
                return Execute(qry, entity);
            }
            else
            {
                qry = method.GenerateUpdate<T>(entity);
                return Execute(qry, entity);
            }
        }
        public async Task<int> InstertOrUpdateAsync<T>(T entity)
        {
            var qry = method.GenerateSelect<T>();
            var ob = await GetDataAsync<T>(qry);
            if (ob == null)
            {
                qry = method.GeneratePartInsert(entity, false);
                return await ExecuteAsync(qry, entity);
            }
            else
            {
                qry = method.GenerateUpdate<T>(entity);
                return await ExecuteAsync(qry, entity);
            }
        }
        // Write your own sql query code
        public IEnumerable<T> SqlQuery<T>(string query, object parameters = null)
        {
            if (parameters == null)
            {
               return GetData<T>(query);
            }
            else
            {
                return GetData<T>(query, parameters);
            }
        }
        // Write your own sql execute code
        public int SqlExecute(string query, object parameters = null)
        {
            if (parameters == null)
            {
                return Execute(query);
            }
            else
            {
                return Execute(query, parameters);
            }
        }
        // Write your own sql query code with parameters
        public async Task<IEnumerable<T>> SqlQueryAsync<T>(string query, object parameters = null)
        {
            if (parameters == null)
            {
                return await GetDataAsync<T>(query);
            }
            else
            {
                return await GetDataAsync<T>(query, parameters);
            }
        }
        public async Task<int> SqlExecuteAsync(string query, object parameters = null)
        {
            if (parameters == null)
            {
                return await ExecuteAsync(query);
            }
            else
            {
                return await ExecuteAsync(query, parameters);
            }
        }
        // ----------------------------  Strong Type  ---------------------------- //
        // ----------------------------  dynamic Type  ---------------------------- //
        // Query
        public IEnumerable<dynamic> SqlQueryDynamic(string query, object parameters = null)
        {
            if (parameters == null)
            {
                return GetDataDynamic(query);
            }
            else
            {
                return GetDataDynamic(query, parameters);
            }
        }
        public async Task<IEnumerable<dynamic>> SqlQueryAsyncDynamic(string query, object parameters = null)
        {
            if (parameters == null)
            {
                return await GetDataAsyncDynamic(query);
            }
            else
            {
                return await GetDataAsyncDynamic(query, parameters);
            }
        }
        // ----------------------------  dynamic Type  ---------------------------- //
        // ----------------------------  Object Type  ---------------------------- //
        public IEnumerable<object> SqlQueryObject(string query, object parameters = null)
        {
            if (parameters == null)
            {
                return GetDataObject(query);
            }
            else
            {
                return GetDataDynamic(query, parameters);
            }
        }
        public async Task<IEnumerable<dynamic>> SqlQueryAsyncObject(string query, object parameters = null)
        {
            if (parameters == null)
            {
                return await GetDataAsyncObject(query);
            }
            else
            {
                return await GetDataAsyncObject(query, parameters);
            }
        }

        // ----------------------------  Object Type  ---------------------------- //

    }
}
