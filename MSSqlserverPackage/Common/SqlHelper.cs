using MSSqlserverPackage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Caching;
using System.Text;

namespace MSSqlserverPackage.Common
{
    public class SqlHelper
    {
        private static string GetConnectionString()
        {
            var connectionString = string.Empty;
            ObjectCache cache = MemoryCache.Default;
            if (connectionString == null)
                connectionString = cache["ConnectionString"] as string;
            return connectionString;
        }
        public static List<Parameter> GetParameters()
        {
            var sqlCommantText = "SELECT  object_id ," +
                            "        sys.parameters.name ," +
                            "        sys.types.name AS [data_type]," +
                            "        sys.parameters.max_length ," +
                            "        is_output ," +
                            "        has_default_value ," +
                            "        default_value " +
                            "FROM    sys.parameters" +
                            "        LEFT JOIN sys.types ON sys.parameters.user_type_id = sys.types.user_type_id " +
                            "WHERE LEN(sys.parameters.name) > 0";
            return RainSong.Common.SqlHelper.GetList<Parameter>(sqlCommantText);
        }
        public static List<DBObject> GetObjects()
        {
            var sqlCommandText =
                "SELECT  object_id ," +
                "        parent_object_id ," +
                "        name ," +
                "        RTRIM(LTRIM([type])) AS [type] ," +
                "        type_desc ," +
                "        create_date ," +
                "        modify_date ," +
                "        is_ms_shipped ," +
                "        schema_id " +
                "FROM    sys.objects";
            return RainSong.Common.SqlHelper.GetList<DBObject>(sqlCommandText);
        }
        public static List<Schema> GetSchemas()
        {
            var sqlCommandText =
                "SELECT  name ," +
                "        schema_id " +
                "FROM    sys.schemas";
            return RainSong.Common.SqlHelper.GetList<Schema>(sqlCommandText);
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
            return RainSong.Common.SqlHelper.GetList<Column>(sqlCommandText);
        }
        public static IEnumerable<Identity> GetIdentities()
        {
            var sqlCommandText = "SELECT  object_id ," +
                                "        column_id ," +
                                "        is_identity," +
                                "        seed_value ," +
                                "        increment_value " +
                                "FROM    sys.identity_columns";
            return RainSong.Common.SqlHelper.GetList<Identity>(sqlCommandText);
        }
        public static IEnumerable<ExtendedProperty> GetExtendedProperties()
        {
            var sqlCommandText = "SELECT  major_id ," +
                                "        minor_id ," +
                                "        name ," +
                                "        value " +
                                "FROM    sys.extended_properties";
            return RainSong.Common.SqlHelper.GetList<ExtendedProperty>(sqlCommandText);
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
            return RainSong.Common.SqlHelper.GetList<PrimaryKey>(sqlCommandText);
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
            return RainSong.Common.SqlHelper.GetList<ReferenceKey>(sqlCommandText);
        }
        public static IEnumerable<Index> GetIndexes()
        {
            var sqlCommandText = "SELECT  sys.indexes.object_id ," +
                                 "        sys.indexes.index_id ," +
                                 "        sys.indexes.is_unique_constraint ," +
                                 "        sys.indexes.name ," +
                                 "        sys.index_columns.column_id " +
                                 "FROM    sys.indexes" +
                                 "        LEFT JOIN sys.index_columns ON sys.indexes.object_id = sys.index_columns.object_id" +
                                 "                                       AND sys.indexes.index_id = sys.index_columns.index_id";
            return RainSong.Common.SqlHelper.GetList<Index>(sqlCommandText);
        }

        public static string ExecuteSqlHelpText(string objectName)
        {
            var procedureName = "sp_helptext";
            var para = new SqlParameter
            {
                ParameterName = "@objname",
                Value = objectName
            };
            var stringBuild = new StringBuilder();
            using (var conn = RainSong.Common.SqlHelper.GetConnection(GetConnectionString()))
            {
                var cmd = new SqlCommand
                {
                    CommandText = procedureName,
                    Connection = conn,
                    CommandType = CommandType.StoredProcedure
                };
                var reader = cmd.ExecuteReader();
                var sb = new StringBuilder();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var value = reader.GetString(0);
                        if (!string.IsNullOrEmpty(value))
                        {
                            sb.AppendLine(value);
                        }
                    }
                }
                return string.Empty;
            }
        }
    }
}
