using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ATM.Data;

public class DataContextDapper {
    private readonly IConfiguration _config;
    public DataContextDapper(IConfiguration configuration) {
        _config = configuration;
    }

    public IEnumerable<T> LoadData<T>(string sql) {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql);
    }
    public IEnumerable<T> LoadDataWithParam<T>(string sql , DynamicParameters parameters) {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Query<T>(sql , parameters);
    }
    public T LoadDataSingle<T>(string sql) {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sql);
    }
    public T LoadDataSingleWithParam<T>(string sql , DynamicParameters parameters) {
        DbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.QuerySingle<T>(sql , parameters);
    }
    public bool ExecuteBool(string sql) {
        IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql) > 0;
    }
    public int ExecuteWithRowCount(string sql) {
        IDbConnection dbConnection = new SqlConnection(sql);
        return dbConnection.Execute(sql);
    }
    public bool ExecuteBoolWithParam(string sql , DynamicParameters parameters) {
        DbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return dbConnection.Execute(sql , parameters) > 0;
    }
    // public int ExecuteRowCount(string sql , DynamicParameters parameters) {
    //     DbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
    //     return dbConnection.Execute(sql , parameters);
    // }

}