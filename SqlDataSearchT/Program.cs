using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataSearchT
{
    class Program
    {
        static string ConnectionString = "server=192.168.40.3;database=ConcreteGOVPlatform;uid=sa;pwd=1";
        static void Main(string[] args)
        {
            DataSet ds = new DataSet();
            var keyWord = "123456";
            var tableInfo = GetTable("SELECT name,object_id FROM sys.objects WHERE type = 'U' ORDER BY object_id;");
            var columnInfo = GetTable(@"SELECT  object_id ,
                                                column_id ,
                                                name
                                                FROM    sys.columns
                                                WHERE   object_id IN ( SELECT   object_id
                                                FROM     sys.objects
                                                WHERE    type = 'u' )
                                                ORDER BY column_id");
            var sbSql = new StringBuilder();
            var sbCurrent = new StringBuilder();
            if (tableInfo.Rows.Count > 0)
            {
                sbCurrent.Clear();
                foreach (DataRow drT in tableInfo.Rows)
                {
                    var blFirstCol = true;
                    var tableId = drT["object_id"].ToString();
                    var tableName = drT["name"].ToString();
                    var colRows = columnInfo.Select(string.Format("object_id = {0}", tableId));
                    sbCurrent.AppendFormat("SELECT * FROM {0} WHERE", tableName);
                    foreach (DataRow drC in colRows)
                    {
                        if (blFirstCol)
                        {
                            sbCurrent.AppendFormat(" {0} LIKE '%{1}%'", drC["name"], keyWord);
                            blFirstCol = false;
                        }
                        sbCurrent.AppendFormat(" OR {0} LIKE '%{1}%'", drC["name"], keyWord);
                    }
                    sbCurrent.Append(";");
                    if ((sbSql.Length + sbCurrent.Length) < 3900)
                    {
                        sbSql.Append(sbCurrent.ToString());
                    }
                    else
                    {
                        FillDataSet(ref ds, sbSql.ToString());
                        sbSql.Clear();
                        sbSql.Append(sbCurrent.ToString());
                    }
                }
                if (sbSql.Length > 0)
                {
                    FillDataSet(ref ds, sbSql.ToString());
                }
            }

        }
        static SqlConnection GetConnection(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            return connection;
        }

        static DataTable GetTable(string sqlCommend)
        {
            var con = GetConnection(ConnectionString);
            DataTable dt = null;
            try
            {
                var adapter = new SqlDataAdapter(sqlCommend, con);
                dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            finally
            {
                con.Close();
            }
        }

        static void FillDataSet(ref DataSet ds, string sqlCommend)
        {
            var con = GetConnection(ConnectionString);
            try
            {
                var adapter = new SqlDataAdapter(sqlCommend, con);
                ds = new DataSet();
                adapter.Fill(ds);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
