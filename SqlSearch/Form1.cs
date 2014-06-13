using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlSearch
{
    public partial class Form1 : Form
    {
        private List<string> errorMsgs;
        public Form1()
        {
            InitializeComponent();
            errorMsgs = new List<string>();

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                var connectionString = string.Format("server={0};database=master;uid={1};pwd={2};", this.txtServer.Text.Trim(),
                    this.txtUid.Text.Trim(),
                    this.txtPwd.Text.Trim());

                var sqlCommand = "SELECT name FROM sys.databases WHERE database_id >4 ORDER BY name;";
                try
                {
                    var table = GetTable(sqlCommand, connectionString);
                    this.comboBox1.DataSource = table;
                    this.comboBox1.DisplayMember = "name";
                    MessageBox.Show("连接成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("连接失败！\r\n失败原因：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            errorMsgs.Clear();
            if (!CheckInput(true)) return;
            var ds = SearhData();
            ShowResultMulitGrid(ds);
        }

        private void CheckServerLogin()
        {
            if (string.IsNullOrEmpty(this.txtServer.Text.Trim()))
            {
                errorMsgs.Add("请输入服务器名！");
            }
            if (string.IsNullOrEmpty(this.txtUid.Text.Trim()))
            {
                errorMsgs.Add("请输入用户名！");
            }
            if (string.IsNullOrEmpty(this.txtPwd.Text.Trim()))
            {
                errorMsgs.Add("请输入密码！");
            }
        }

        private bool CheckInput(bool checkKeyWord = false)
        {
            errorMsgs.Clear();
            CheckServerLogin();
            if (checkKeyWord)
            {
                if (string.IsNullOrEmpty(this.comboBox1.Text))
                {
                    errorMsgs.Add("请选择数据库！");
                }
                if (string.IsNullOrEmpty(this.txtkeyWord.Text.Trim()))
                {
                    errorMsgs.Add("请输入要检索的关键字！");
                }
            }
            if (errorMsgs.Any())
            {
                MessageBox.Show(string.Join("\r\n", errorMsgs), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private DataTable GetTable(string sqlCommand, string connectionString, string tableName = "")
        {
            var table = new DataTable(tableName);
            using (var adapter = new SqlDataAdapter(sqlCommand, connectionString))
            {
                adapter.Fill(table);
            }
            return table;
        }
        private DataSet SearhData()
        {
            var keyWord = this.txtkeyWord.Text.Trim();
            var connectionString = string.Format("server={0};database={1};uid={2};pwd={3};", this.txtServer.Text.Trim(),
                this.comboBox1.Text,
                    this.txtUid.Text.Trim(),
                    this.txtPwd.Text.Trim());
            var tableInfo = GetTable("SELECT name,object_id FROM sys.objects WHERE type = 'U' ORDER BY object_id;", connectionString);
            var columnInfo = GetTable(@"SELECT  object_id ,
                                                column_id ,
                                                name
                                                FROM    sys.columns
                                                WHERE   object_id IN ( SELECT   object_id
                                                FROM     sys.objects
                                                WHERE    type = 'u' )
                                                ORDER BY column_id", connectionString);
            return FeachTable(tableInfo, columnInfo, connectionString, keyWord);
        }
        private DataSet FeachTable(DataTable tableInfo, DataTable columnInfo, string connectionString, string keyWord)
        {
            DataSet ds = new DataSet();
            var sbSql = new StringBuilder();
            if (tableInfo.Rows.Count > 0)
            {
                foreach (DataRow drT in tableInfo.Rows)
                {
                    var blFirstCol = true;
                    var tableId = drT["object_id"].ToString();
                    var tableName = drT["name"].ToString();
                    var colRows = columnInfo.Select(string.Format("object_id = {0}", tableId));
                    sbSql.AppendFormat("SELECT * FROM {0} WHERE", tableName);
                    foreach (DataRow drC in colRows)
                    {
                        if (blFirstCol)
                        {
                            sbSql.AppendFormat(" {0} LIKE '%{1}%'", drC["name"], keyWord);
                            blFirstCol = false;
                        }
                        sbSql.AppendFormat(" OR {0} LIKE '%{1}%'", drC["name"], keyWord);
                    }

                    var table = GetTable(sbSql.ToString(), connectionString, tableName);
                    sbSql.Clear();
                    if (table.Rows.Count > 0)
                    {
                        ds.Tables.Add(table);
                    }
                }
            }
            return ds;
        }

        private void ShowResultMulitGrid(DataSet ds)
        {
            var tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            this.panelMain.Controls.Add(tabControl);
            for (var i = 0; i < ds.Tables.Count; i++)
            {

                var grid = new DataGridView();
                grid.ClearSelection();

                grid.ReadOnly = true;
                grid.Dock = DockStyle.Fill;
                grid.DataSource = ds.Tables[i];
                grid.RowHeadersVisible = false;
                grid.AllowUserToAddRows = false;
                grid.CellPainting += Grid_CellPainting;

                grid.ClearSelection();

                var tabPage = new System.Windows.Forms.TabPage();
                tabPage.Text = ds.Tables[i].TableName;
                tabPage.Controls.Add(grid);
                tabControl.TabPages.Add(tabPage);
            }
        }

        private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.Value != null &&
                e.Value.ToString().Contains(this.txtkeyWord.Text.Trim()))
            {
                e.CellStyle.ForeColor = System.Drawing.Color.Red;
            }
        }

    }
}
