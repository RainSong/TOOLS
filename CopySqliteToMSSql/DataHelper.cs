using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Configuration;

namespace CopySqliteToMSSql
{
    public static class DataHelper
    {
        public static SQLiteConnection GetSqliteConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        public static SqlConnection GetSqlConnection(string connnectionString)
        {
            return new SqlConnection(connnectionString);
        }

        public static IDataReader GetReader(DbConnection con, string sql, params DbParameter[] paras)
        {
            var cmd = con.CreateCommand();
            cmd.CommandText = sql;
            if (paras != null && paras.Length > 0)
            {
                cmd.Parameters.AddRange(paras);
            }
            DataTable dt = new DataTable();
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                return cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw new Exception("获取DataReader失败", ex);
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="con"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static DataTable GetTable(DbConnection con, string sql, params DbParameter[] paras)
        {
            DataTable dt = new DataTable();
            try
            {
                var reader = GetReader(con, sql, paras);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    DataColumn dc = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
                    dt.Columns.Add(dc);
                }
                while (reader.Read())
                {
                    var record = (IDataRecord)reader;
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dr[i] = reader.GetValue(i);
                    }
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("数据读取失败", ex);
            }

            return dt;
        }

        public static void CopyData(IDataReader sourceReader, SqlConnection targetConnection,string targetTableName)
        {
            SqlTransaction tran = null;
            try
            {
                if (targetConnection.State != ConnectionState.Open)
                {
                    targetConnection.Open();
                }
                tran = targetConnection.BeginTransaction();
                using (SqlBulkCopy copy = new SqlBulkCopy(targetConnection,SqlBulkCopyOptions.Default, tran))
                {
                    copy.DestinationTableName = targetTableName;
                    copy.WriteToServer(sourceReader);
                }
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw new Exception("数据复制失败", ex);
            }
            
        }
    }
}
