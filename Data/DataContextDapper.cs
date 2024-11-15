using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Data
{
    class DataContextDapper
    {
        private readonly IConfiguration _config;
        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sql);
        }

        public bool ExecuteData(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql);
        }

        public bool ExecuteSqlWithParameter(string sql, List<SqlParameter> parameters)
        {
            SqlCommand commandWithParameters = new SqlCommand(sql);

            foreach (SqlParameter parameter in parameters)
            {
                commandWithParameters.Parameters.Add(parameter);
            }

            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            dbConnection.Open();

            commandWithParameters.Connection = (SqlConnection)dbConnection;

            int rowsAffected = commandWithParameters.ExecuteNonQuery();

            dbConnection.Close();

            return rowsAffected > 0;
        }
    }
}