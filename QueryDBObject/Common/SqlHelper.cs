using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace QueryDBObject
{
    public class SqlHelper
    {

        public static SqlConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
        public static SqlConnection GetOpenConnection(string connectionString)
        {
            var con = GetConnection(connectionString);
            if (con.State != ConnectionState.Open)
            {
                try
                {
                    con.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("打开数据库连接失败", ex);
                }
            }
            return con;
        }

        public static int ExecuteNonQuery(string connectionString, string sqlCommandText, params SqlParameter[] parameters)
        {
            using (var conn = GetOpenConnection(connectionString))
            {
                var cmd = new SqlCommand(sqlCommandText, conn);
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }
        public static object ExecuteScalar(string connectionString, string sqlCommandText, params SqlParameter[] parameters)
        {
            using (var conn = GetOpenConnection(connectionString))
            {
                var cmd = new SqlCommand(sqlCommandText, conn);
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
                //return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        public static T ExecuteScalar<T>(string connectionString, string sqlCommandText, params SqlParameter[] parameters)
        {
            var objValue = ExecuteScalar(connectionString, sqlCommandText, parameters);
            if (objValue == null || DBNull.Value.Equals(objValue)) return default(T);
            var targetType = typeof(T);
            object value = Convert.ChangeType(objValue, targetType);
            return (T)value;
        }

        public static DataTable ExecuteQuery(string connectionString,
            string sqlCommentText,
            CommandType commandType = CommandType.Text,
            params SqlParameter[] paramters)
        {
            IDictionary dic = new Dictionary<string, object>();
            return ExecuteQuery(connectionString, sqlCommentText, commandType, out dic, paramters);
        }

        public static DataTable ExecuteQuery(string connectionString,
            string sqlCommentText,
            CommandType commandType,
            out IDictionary statistics,
            params SqlParameter[] paramters)
        {
            using (var conn = GetOpenConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                conn.StatisticsEnabled = true;
                conn.ResetStatistics();

                var cmd = new SqlCommand(sqlCommentText, conn);
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(paramters);
                var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);

                statistics = conn.RetrieveStatistics();

                return dt;
            }
        }

        public static DataTable ExecuteQuery(string connectionString, string sqlCommentText, out IDictionary statistics, params SqlParameter[] paramters)
        {
            return ExecuteQuery(connectionString, sqlCommentText, CommandType.Text, out statistics, paramters);
        }

        public static List<T> ExecuteQuery<T>(string connectionString, string sqlCommentText, out IDictionary statistics, params SqlParameter[] paramters)
        {
            var dt = ExecuteQuery(connectionString, sqlCommentText, out statistics, paramters);
            return dt.ToList<T>();
        }
    }

    public static class DataTableHelper
    {
        public static List<T> ToList<T>(this DataTable dt)
        {
            var list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr.ToEntity<T>());
            }
            return list;
        }
        public static T ToEntity<T>(this DataRow row)
        {
            var entity = Activator.CreateInstance<T>();
            var type = entity.GetType();
            var properties = type.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (row.Table.Columns.Contains(pi.Name))
                {
                    var value = row[pi.Name];
                    try
                    {
                        value = Convert.ChangeType(value, pi.PropertyType);
                        pi.SetValue(entity, value);
                        continue;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return entity;
        }
    }
}
