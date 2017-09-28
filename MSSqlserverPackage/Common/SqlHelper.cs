using MSSqlserverPackage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;

namespace MSSqlserverPackage.Common
{
    public class SqlHelper
    {
        public static SqlConnection GetConnection(string connectionString = null)
        {
            ObjectCache cache = MemoryCache.Default;
            if (connectionString == null)
                connectionString = cache["ConnectionString"] as string;
            return new SqlConnection(connectionString);
        }
        public static SqlConnection GetOpenConnection(string connectionString = null)
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

        public static DataTable GetTable(string sqlCommandText)
        {
            try
            {
                using (var con = GetOpenConnection())
                {
                    SqlCommand cmd = new SqlCommand(sqlCommandText, con);
                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("数据查询失败", ex);
            }
        }

        public static DataTable GetTable(string connectionString, string sqlCommandText, params SqlParameter[] parameters)
        {
            try
            {
                using (var con = GetOpenConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sqlCommandText, con);
                    if (parameters != null && parameters.Any())
                        cmd.Parameters.AddRange(parameters);
                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("数据查询失败", ex);
            }
        }
        public static DataTable GetTable(SqlConnection conn, string sqlCommandText, params SqlParameter[] parameters)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sqlCommandText, conn);
                if (parameters != null && parameters.Any())
                    cmd.Parameters.AddRange(parameters);
                var adapter = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("数据查询失败", ex);
            }
        }
        public static int ExecuteNonQuery(string sqlCommandText, params SqlParameter[] parameters)
        {
            using (var conn = GetOpenConnection())
            {
                var cmd = new SqlCommand(sqlCommandText, conn);
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }
        public static object ExecuteScalar(string sqlCommandText, params SqlParameter[] parameters)
        {
            using (var conn = GetOpenConnection())
            {
                var cmd = new SqlCommand(sqlCommandText, conn);
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
                //return (T)Convert.ChangeType(result, typeof(T));
            }
        }
        public static T ConverRowToEntity<T>(DataRow dr)
        {
            var entity = Activator.CreateInstance<T>();
            var type = entity.GetType();
            var properties = type.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                var attrs = pi.GetCustomAttributes<FieldAttribute>();
                if (attrs != null || attrs.Any())
                {
                    foreach (FieldAttribute fa in attrs)
                    {
                        if (dr.Table.Columns.Contains(fa.Field))
                        {
                            var value = dr[fa.Field];
                            try
                            {
                                value = Convert.ChangeType(value, pi.PropertyType);
                                pi.SetValue(entity, value);
                                break;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            return entity;
        }
        public static List<T> GetList<T>(DataTable dt)
        {
            var list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(ConverRowToEntity<T>(dr));
            }
            return list;
        }

        public static List<T> GetList<T>(string sqlCommandText)
        {
            var dt = GetTable(sqlCommandText);
            return GetList<T>(dt); ;
        }

        public static List<DBObject> GetObjects()
        {
            var sqlCommandText =
                "SELECT  object_id ," +
                "        parent_object_id ," +
                "        name ," +
                "        type ," +
                "        type_desc ," +
                "        create_date ," +
                "        modify_date ," +
                "        is_ms_shipped ," +
                "        schema_id "+
                "FROM    sys.objects";
            return GetList<DBObject>(sqlCommandText);
        }

        public static List<Schema> GetSchemas()
        {
            var sqlCommandText =
                "SELECT  name ," +
                "        schema_id " +
                "FROM    sys.schemas";
            return GetList<Schema>(sqlCommandText);
        }

        public static List<Column> GetColumns()
        {
            var sqlCommandText = "SELECT  object_id ," +
                                "        column_id ," +
                                "        sys.columns.name ," +
                                "        sys.columns.is_nullable ," +
                                "        sys.columns.max_length ," +
                                "        sys.types.name AS data_type " +
                                "FROM    sys.columns" +
                                "        LEFT JOIN sys.types ON sys.columns.user_type_id = sys.types.user_type_id";
            return GetList<Column>(sqlCommandText);
        }


        public static IEnumerable<Identity> GetIdentities()
        {
            var sqlCommandText = "SELECT  object_id ," +
                                "        column_id ," +
                                "        is_identity," +
                                "        seed_value ," +
                                "        increment_value " +
                                "FROM    sys.identity_columns";
            return GetList<Identity>(sqlCommandText);
        }

        internal static IEnumerable<ExtendedProperty> GetExtendedProperties()
        {
            var sqlCommandText = "SELECT  major_id ," +
                                "        minor_id ," +
                                "        name ," +
                                "        value " +
                                "FROM    sys.extended_properties";
            return GetList<ExtendedProperty>(sqlCommandText);
        }

        public static IEnumerable<PrimaryKey> GetPrimaryKeys()
        {
            var sqlCommandText = "SELECT  sys.objects.object_id AS [object_id] ," +
                                "        sys.objects.name AS[object_name]," +
                                "        sys.key_constraints.name AS[key_name]," +
                                "        sys.columns.column_id ," +
                                "        sys.columns.name AS[column_name]" +
                                "FROM sys.objects" +
                                "     LEFT JOIN sys.key_constraints ON sys.objects.object_id = sys.key_constraints.parent_object_id" +
                                "        LEFT JOIN sys.indexes ON sys.objects.object_id = sys.indexes.object_id" +
                                "                                 AND sys.indexes.is_primary_key = 1" +
                                "        LEFT JOIN sys.index_columns ON sys.indexes.index_id = sys.index_columns.index_id" +
                                "                                       AND sys.indexes.object_id = sys.index_columns.object_id" +
                                "        LEFT JOIN sys.columns ON sys.index_columns.object_id = sys.columns.object_id" +
                                "                                 AND sys.index_columns.column_id = sys.columns.column_id " +
                                "WHERE sys.key_constraints.type = 'pk'";
            return GetList<PrimaryKey>(sqlCommandText);
        }

        public static List<ReferenceKey> GetReferenceKeys()
        {
            var sqlCommandText = "SELECT  sys.foreign_keys.name AS [key_name] ," +
                                "       sys.foreign_keys.parent_object_id AS[object_id]," +
                                "       sys.columns.column_id ," +
                                "       sys.columns.name AS parent_column_name ," +
                                "       referenced_object.object_id AS referenced_object_id ," +
                                "       referenced_object.name AS referenced_object_name ," +
                                "       referenced_columns.column_id AS referenced_column_id ," +
                                "       referenced_columns.name AS referenced_column_name " +
                                "FROM    sys.foreign_keys" +
                                "        LEFT JOIN sys.foreign_key_columns ON sys.foreign_keys.object_id = sys.foreign_key_columns.constraint_object_id" +
                                "        LEFT JOIN sys.columns ON sys.foreign_key_columns.parent_object_id = sys.columns.object_id" +
                                "                                 AND sys.foreign_key_columns.parent_column_id = sys.columns.column_id" +
                                "        LEFT JOIN sys.objects ON sys.foreign_key_columns.parent_object_id = sys.objects.object_id" +
                                "        LEFT JOIN sys.columns AS referenced_columns ON sys.foreign_key_columns.referenced_object_id = referenced_columns.object_id" +
                                "                                                       AND sys.foreign_key_columns.referenced_column_id = referenced_columns.column_id" +
                                "        LEFT JOIN sys.objects AS referenced_object ON sys.foreign_key_columns.referenced_object_id = referenced_object.object_id";
            return GetList<ReferenceKey>(sqlCommandText);
        }


        internal static IEnumerable<Index> GetIndexes()
        {
            var sqlCommandText = "SELECT  sys.indexes.object_id ," +
                                 "        sys.indexes.index_id ," +
                                 "        sys.indexes.is_unique_constraint ," +
                                 "        sys.indexes.name ," +
                                 "        sys.index_columns.column_id " +
                                 "FROM    sys.indexes" +
                                 "        LEFT JOIN sys.index_columns ON sys.indexes.object_id = sys.index_columns.object_id" +
                                 "                                       AND sys.indexes.index_id = sys.index_columns.index_id";
            return GetList<Index>(sqlCommandText);
        }
    }
}
