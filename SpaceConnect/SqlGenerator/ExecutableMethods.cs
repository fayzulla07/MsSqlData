using Dapper;
using SqlData;
using SqlData.SqlGenerator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SqlData.SqlGenerator
{
    public class ExecutableMethods 
    {
        protected GenerateMethods method;
        string conn { get; set; }
        public ExecutableMethods()
        {
            conn = Cs.CsStr;
            method = new GenerateMethods();
        }
        // ----------------------------  Strong Types  ---------------------------- //

            // Get data from database 
        public IEnumerable<T> GetData<T>(string qry)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = db.Query<T>(qry);
                return result;
            }

        }
        // Get data from database with async
        public async Task<IEnumerable<T>> GetDataAsync<T>(string qry)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = await db.QueryAsync<T>(qry);
                return result;
            }

        }

        // Get data from the database with the parameter
        public IEnumerable<T> GetData<T>(string qry, object parameters)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = db.Query<T>(qry, parameters);
                return result;
            }

        }
        public async Task<IEnumerable<T>> GetDataAsync<T>(string qry, object parameters)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = await db.QueryAsync<T>(qry, parameters);
                return result;
            }

        }
        // Execute the qry
        public int Execute(string qry)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = db.Execute(qry);
                return result;
            }

        }
        // Execute the qry with parameters
        public int Execute(string qry, object parameters)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = db.Execute(qry, parameters);
                return result;
            }

        }
        public async Task<int> ExecuteAsync(string qry)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = await db.ExecuteAsync(qry);
                return result;
            }

        }
        public async Task<int> ExecuteAsync(string qry, object parameters)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = await db.ExecuteAsync(qry, parameters);
                return result;
            }

        }
        // ----------------------------  Strong Types  ---------------------------- //
        // ***********************************************************************//
        // ----------------------------  Dynamic Type  ---------------------------- //
        public IEnumerable<dynamic> GetDataDynamic(string qry)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = db.Query<dynamic>(qry);
                return result;
            }

        }
        public Task<IEnumerable<dynamic>> GetDataAsyncDynamic(string qry)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = db.QueryAsync<dynamic>(qry);
                return result;
            }

        }

        // Получаем данные из базы данных с параметром
        public IEnumerable<dynamic> GetDataDynamic(string qry, object parameters)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = db.Query(qry, parameters);
                return result;
            }

        }
        public async Task<IEnumerable<dynamic>> GetDataAsyncDynamic(string qry, object parameters)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = await db.QueryAsync(qry, parameters);
                return result;
            }

        }

        // ----------------------------  Dynamic Type  ---------------------------- //
        // ***********************************************************************//
        // ----------------------------  Object Type  ---------------------------- //
        public IEnumerable<object> GetDataObject(string qry)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = db.Query<object>(qry);
                return result;
            }

        }
        public Task<IEnumerable<object>> GetDataAsyncObject(string qry)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = db.QueryAsync<object>(qry);
                return result;
            }

        }

        // 
        public IEnumerable<object> GetDataObject(string qry, object parameters)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = db.Query<object>(qry, parameters);
                return result;
            }

        }
        public async Task<IEnumerable<object>> GetDataAsyncObject(string qry, object parameters)
        {
            using (var db = new SqlConnection(conn))
            {
                var result = await db.QueryAsync<object>(qry, parameters);
                return result;
            }

        }

        // ----------------------------  Object Type  ---------------------------- //

    }
}
