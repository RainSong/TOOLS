using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QueryDBObject
{
    public partial class FormQueryTable : Form
    {

        #region fields
        public Form LoginForm;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 数据库服务器名/IP
        /// </summary>
        private string server;
        /// <summary>
        /// 登录数据的用户名
        /// </summary>
        private string uid;
        /// <summary>
        /// 连接的数据库名
        /// </summary>
        private string dbName;
        /// <summary>
        /// 执行时间
        /// </summary>
        private string executeTime = "00:00:00";
        /// <summary>
        /// 受影响行数
        /// </summary>
        private string rowCount = "0";

        private List<Models.Igrone> igrones = null;
        private List<Models.DBObject> objects = null;

        #endregion

        #region constructor

        public FormQueryTable(SqlConnectionStringBuilder conStringBuilder)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            this.connectionString = conStringBuilder.ToString();
            this.server = conStringBuilder.DataSource;
            this.uid = conStringBuilder.UserID;
            this.dbName = conStringBuilder.InitialCatalog;

            InitializeComponent();
            this.grid.AutoGenerateColumns = false;
            this.dgvColumnInfo.AutoGenerateColumns = false;
            //this.txtNotContains.Text = "test,backup,bak";

            InitCobobox();

            igrones = new List<Models.Igrone>
            {
                new Models.Igrone("test"),
                new Models.Igrone("backup"),
                new Models.Igrone("bak")
            };
            BindIgronesGrid();

            
        }
        #endregion

        #region event handles
        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.objects = GetObjects(this.txtNameKeyWord.Text.Trim(), this.txtColumnNameKeyWord.Text.Trim());
            BindObjectGrid();

        }
        private void Main_Load(object sender, EventArgs e)
        {
            //GetConnectInfo();
            ShowStipLabel();
        }
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            var dialogResult = MessageBox.Show("是否返回重新连接服务器？\r\n是：重新连接，否：退出程序！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {

                if (this.LoginForm != null)
                {
                    this.LoginForm.Show();
                }
                else
                {
                    var lf = new LoginServer();
                    lf.StartPosition = FormStartPosition.CenterScreen;
                    lf.Show();
                }
            }
            else
            {
                Application.Exit();
            }
        }
        /// <summary>
        /// 在行标头中添加编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // https://stackoverflow.com/questions/9581626/show-row-number-in-row-header-of-a-datagridview
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null) return;
            if (dgv.Columns[e.ColumnIndex] == this.colModefiyDate 
                || dgv.Columns[e.ColumnIndex] == this.colCreateDate)
            {
                var time = e.Value as Nullable<DateTime>;
                var value = string.Empty;
                if (time != null)
                {
                    value = time.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                e.Value = value;
            }
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                Models.DBObject obj = (sender as DataGridView).Rows[e.RowIndex].DataBoundItem as Models.DBObject;
                if (obj == null) return;
                if (obj.Columns == null || !obj.Columns.Any())
                {
                    try
                    {
                        var columns = GetColumns(obj.ObjectID);
                        obj.Columns = columns;
                    }
                    catch (Exception ex)
                    {
                        Common.ShowMessage(ex.Message, "Error");
                        Logger.Error(string.Format("查询对象{0}的列信息失败", obj.Name), ex);
                        return;
                    }
                }
                BindingSource bs = new BindingSource();
                bs.DataSource = obj.Columns;
                this.dgvColumnInfo.DataSource = bs;
            }
        }
        /// <summary>
        /// 查询内容输入框按钮按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnQuery.PerformClick();
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// 为列表查询绑定数据方法
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="nameKeyWord"></param>
        private List<Models.DBObject> GetObjects(string objectName, string columnName)
        {
            var sqlCommand = new StringBuilder("SELECT		DISTINCT SYS.OBJECTS.OBJECT_ID		AS ObjectID               " +
                                                "            , sys.objects.name                 AS Name                   " +
                                                "            , sys.objects.create_date          AS CreateDate             " +
                                                "            , sys.objects.modify_date          AS ModifyDate             " +
                                                "            , CASE sys.objects.type                                      " +
                                                "                    WHEN 'U'    THEN 'Table'                             " +
                                                "                    WHEN 'V'    THEN 'View'                              " +
                                                "                    ELSE sys.objects.type                                " +
                                                "                                                                         " +
                                                "            END                                AS ObjectType             " +
                                                "FROM        SYS.columns                                                  " +
                                                "LEFT JOIN   SYS.objects ON sys.objects.object_id = sys.columns.object_id " +
                                                "WHERE (TYPE = 'U' OR      TYPE = 'V') ");
            //text LIKE '%' + @KeyWord + '%'
            var paras = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(objectName))
            {
                sqlCommand.Append(" AND sys.objects.name LIKE @ObjectName");
                paras.Add(new SqlParameter
                {
                    ParameterName = "@ObjectName",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = "%" + objectName + "%"
                });
            }
            if (!string.IsNullOrEmpty(columnName))
            {
                sqlCommand.Append(" AND sys.columns.name LIKE @ColumnName");
                paras.Add(new SqlParameter
                {
                    ParameterName = "@ColumnName",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = "%" + columnName + "%"
                });
            }
            if (this.igrones != null && this.igrones.Any())
            {
                var igronKeyWords = this.igrones.Where(o => string.Equals(o.IgroneType, "KeyWord"))
                                                .Select(o => o.Content)
                                                .AsEnumerable();
                if (igronKeyWords.Any())
                {
                    foreach (string kw in igronKeyWords)
                    {
                        var para = new SqlParameter
                        {
                            ParameterName = "@NotLike_" + kw,
                            SqlDbType = SqlDbType.VarChar,
                            Value = "%" + kw + "%"
                        };
                        sqlCommand.Append(" AND sys.objects.name NOT LIKE " + para.ParameterName);
                        paras.Add(para);
                    }
                }
                var names = this.igrones.Where(o => string.Equals(o.IgroneType, "Name"))
                                        .Select(o => o.Content)
                                        .AsEnumerable();
                if (names.Any())
                {
                    var nameParas = new List<SqlParameter>();
                    foreach (string name in names)
                    {
                        var para = new SqlParameter
                        {
                            ParameterName = "@NotEquals_" + name,
                            SqlDbType = SqlDbType.VarChar,
                            Value = name
                        };
                        nameParas.Add(para);
                    }
                    paras.AddRange(nameParas);
                    sqlCommand.AppendFormat(" sys.objects.name NOT IN ({0})", string.Join(",", nameParas.Select(p => p.ParameterName).ToArray()));
                }

            }

            var sort = string.Empty;

            var item = this.cboSort.SelectedItem as dynamic;
            if (item != null)
            {
                sort = item.Value;
            }
            if (!string.IsNullOrEmpty(sort))
            {
                item = this.cboSortType.SelectedItem as dynamic;
                if (item != null)
                {
                    sort = " sys.objects." + sort + " " + item.Value;
                }
                sqlCommand.Append(" order by ");
                sqlCommand.Append(sort);
            }
            
            try
            {
                IDictionary dic = new Dictionary<object, object>();
                var list = SqlHelper.ExecuteQuery<Models.DBObject>(this.connectionString, sqlCommand.ToString(), out dic, paras.ToArray());
                this.rowCount = list.Count.ToString();

                var et = (long)dic["ExecutionTime"];
                ComputeTime(et);
                this.rowCount = dic["SelectRows"].ToString();
                ShowStipLabel();
                return list;
            }
            catch (Exception ex)
            {
                Common.ShowMessage("查询失败：" + ex.Message, "Error");
                Logger.Error("根据关键字查询存储过程事变", ex);
            }
            return new List<Models.DBObject>();
        }
        /// <summary>
        /// 状态栏显示
        /// </summary>
        private void ShowStipLabel()
        {
            if (string.IsNullOrEmpty(this.server))
            {
                this.tssLblServer.Text =
                    this.tssLblSplit1.Text = string.Empty;
            }
            else
            {
                this.tssLblServer.Text = server;
                this.tssLblSplit1.Text = " | ";
            }
            if (string.IsNullOrEmpty(this.dbName))
            {
                this.tssLblDataBase.Text =
                    this.tssLblSplit2.Text = string.Empty;
            }
            else
            {
                this.tssLblDataBase.Text = dbName;
                this.tssLblSplit2.Text = " | ";
            }
            if (string.IsNullOrEmpty(this.uid))
            {
                this.tssLblUserID.Text =
                    this.tssLblSplit3.Text = string.Empty;
            }
            else
            {
                this.tssLblUserID.Text = uid;
                this.tssLblSplit3.Text = " | ";
            }
            if (string.IsNullOrEmpty(this.executeTime))
            {
                this.tssLblExecuteTime.Text =
                    this.tssLblSplit4.Text = string.Empty;
            }
            else
            {
                this.tssLblExecuteTime.Text = executeTime;
                this.tssLblSplit4.Text = " | ";
            }
            if (string.IsNullOrEmpty(this.rowCount))
            {
                this.tssLblRowCount.Text = string.Empty;
            }
            else
            {
                this.tssLblRowCount.Text = rowCount + "行";
            }
        }
        /// <summary>
        /// 将数据库执行时间由秒数转换为0：00：00的格式
        /// </summary>
        /// <param name="seconds"></param>
        private void ComputeTime(long seconds)
        {
            seconds = seconds / 1000;
            int hour = 0;
            int min = 0;
            if (seconds >= 3600)
            {
                hour = (int)seconds / 3600;
                seconds = seconds % 3600;
            }
            if (seconds >= 60)
            {
                min = (int)seconds / 60;
                seconds = seconds % 60;
            }
            this.executeTime = hour.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// 查询存储过程脚本
        /// </summary>
        /// <param name="prodecureName"></param>
        /// <returns></returns>
        private List<Models.DBColumn> GetColumns(int objectId)
        {
            var sqlCommand = "SELECT		obj.object_id                                                                                              " +
                             "            , col.column_id AS ID                                                                                        " +
                             "            ,col.[name]             AS[Name]                                                                             " +
                             "            ,[type].name AS DataType                                                                                     " +
                             "            ,col.max_length AS[MaxLength]                                                                                " +
                             "            , col.is_nullable AS Nullable                                                                                " +
                             "            ,CASE WHEN col.is_identity = 0 THEN NULL                                                                     " +
                             "                  ELSE '('+ CONVERT(VARCHAR, [identity].seed_value)+','+CONVERT(VARCHAR, [identity].increment_value)+')' " +
                             "            END AS[Identity]                                                                                             " +
                             "            ,[index].is_primary_key AS IsPrimary                                                                         " +
                             "            ,EP.[Description] " +
                             "            ,[Default].[definition] AS [Default]                                                                                            " +
                             "FROM        sys.objects AS obj                                                                                           " +
                             "LEFT JOIN sys.columns             AS col          ON obj.[object_id] = col.[object_id]                                   " +
                             "LEFT JOIN   sys.types AS [type]       ON col.user_type_id = [type].user_type_id                                          " +
                             "LEFT JOIN   sys.identity_columns AS [identity]   ON col.column_id = [identity].column_id                                 " +
                             "                                                 AND[identity].[object_id] = obj.[object_id]                             " +
                             "LEFT JOIN   sys.index_columns AS index_column ON  obj.object_id = index_column.object_id                                 " +
                             "                                        AND col.column_id = index_column.column_id                                       " +
                             "LEFT JOIN   sys.indexes AS [index]      ON obj.object_id = [index].object_id                                             " +
                             "                                        AND index_column.index_id = [index].index_id                                     " +
                             "LEFT JOIN ( SELECT     major_id AS [OBJECT_ID]                                                                           " +
                             "                     , minor_id AS [COLUMN_ID]                                                                           " +
                             "                     , [value]  AS [Description]                                                                         " +
                             "            FROM sys.extended_properties                                                                                 " +
                             "            WHERE [name] = 'MS_Description') AS EP ON obj.object_id = EP.OBJECT_ID                                       " +
                             "                                                   AND  col.column_id = EP.COLUMN_ID                                     " +
                             "LEFT JOIN sys.sysconstraints AS Constraints ON	obj.object_id =  Constraints.id                                        " +
                             "                                            AND   col.column_id = Constraints.colid                                      " +
                             "LEFT JOIN   sys.default_constraints AS[Default] ON Constraints.constid = [Default].object_id                             " +
                             "where obj.object_id = @ObjectID";
            try
            {
                IDictionary dic = new Dictionary<object, object>();
                var paraObjName = new SqlParameter
                {
                    ParameterName = "@ObjectID",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objectId
                };
                return SqlHelper.ExecuteQuery(this.connectionString, sqlCommand, CommandType.Text, paraObjName).ToList<Models.DBColumn>();
            }
            catch (Exception ex)
            {
                throw new Exception("查询存储过程脚本错误", ex);
            }
        }

        private void InitCobobox()
        {
            var sortItems = new object[]
            {
                new { Text = "名称", Value = "name"},
                new { Text = "创建时间",Value = "create_date"},
                new { Text = "最后修改时间", Value = "modify_date"}
            };
            this.cboSort.Items.Clear();
            this.cboSort.Items.AddRange(sortItems);
            this.cboSort.DisplayMember = "Text";
            this.cboSort.ValueMember = "Value";
            this.cboSort.SelectedIndex = 2;

            var sortTypeItems = new object[]
            {
                new { Text = "升序", Value = "asc"},
                new { Text = "降序",Value = "desc"}
            };
            this.cboSortType.Items.Clear();
            this.cboSortType.Items.AddRange(sortTypeItems);
            this.cboSortType.DisplayMember = "Text";
            this.cboSortType.ValueMember = "Value";
            this.cboSortType.SelectedIndex = 1;
        }

        private void BindIgronesGrid()
        {
            BindingSource source = new BindingSource();
            source.DataSource = this.igrones;
            this.dgvIgrone.DataSource = source;
        }

        private void BindObjectGrid()
        {
            IQueryable<Models.DBObject> objs = null;
            if (this.objects != null)
            {
                var nc = this.igrones.Where(o => string.Equals(o.IgroneType, "KeyWord")).Select(o => o.Content).AsQueryable();
                objs = this.objects.AsQueryable();
                if (nc.Any())
                    objs = objs.Where(o => !nc.Any(c => o.Name.Contains(c)));
                var names = this.igrones.Where(o => string.Equals(o.IgroneType, "Name")).Select(o => o.Content).AsQueryable();
                if (names.Any())
                    objs = objs.Where(o => !names.Contains(o.Name));
            }
            BindingSource bs = new BindingSource();
            if (objs != null)
            {
                bs.DataSource = objs.ToList();
            }
            this.grid.DataSource = bs;
        }

        #endregion

        private void grid_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            this.grid.ContextMenuStrip = this.dgvIgrone.ContextMenuStrip = null;
            if (e.RowIndex >= 0)
            {
                if (sender == this.grid
                    && this.grid.SelectedCells[0].RowIndex == e.RowIndex
                    && this.grid.SelectedCells[0].ColumnIndex == e.ColumnIndex)
                {
                    this.grid.ContextMenuStrip = this.cmsGrid;
                }
                else if (sender == this.dgvIgrone
                    && this.dgvIgrone.SelectedCells[0].RowIndex == e.RowIndex
                    && this.dgvIgrone.SelectedCells[0].ColumnIndex == e.ColumnIndex)
                {
                    this.dgvIgrone.ContextMenuStrip = this.cmsGridIgrone;
                }
            }
        }

        private void tsmiAddIgrone_Click(object sender, EventArgs e)
        {
            var igrone = this.grid.SelectedCells.Count == 0 ? string.Empty : this.grid.SelectedCells[0].Value as string;
            if (string.IsNullOrEmpty(igrone)) return;
            if (this.igrones.Any(o => string.Equals(o.Content, igrone))) return;
            this.igrones.Add(new Models.Igrone(igrone, "Name"));
            BindIgronesGrid();
            BindObjectGrid();
        }

        private void tsmiRemoveIgrone_Click(object sender, EventArgs e)
        {
            var igrone = this.dgvIgrone.SelectedCells.Count == 0 ? string.Empty : this.dgvIgrone.SelectedCells[0].Value as string;
            if (string.IsNullOrEmpty(igrone)) return;
            var item = this.igrones.FirstOrDefault(o => string.Equals(o.Content, igrone));
            this.igrones.Remove(item);

            BindIgronesGrid();
            BindObjectGrid();
        }

        private void dgvIgrone_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var igrone = this.dgvIgrone.Rows[e.RowIndex].DataBoundItem as Models.Igrone;
            if (igrone == null) igrone = new Models.Igrone();
            igrone.Content = this.dgvIgrone.Rows[e.RowIndex].Cells[1].Value as string;
            igrone.IgroneType = this.dgvIgrone.Rows[e.RowIndex].Cells[2].Value as string;
            if (!this.igrones.Any(o => string.Equals(o.ID, igrone.ID)))
            {
                this.igrones.Add(igrone);
                BindIgronesGrid();
            }
            if (string.IsNullOrEmpty(igrone.Content) || string.IsNullOrEmpty(igrone.IgroneType)) return;

            BindObjectGrid();
        }

        private void dgvColumnInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv.Columns[e.ColumnIndex] == this.colColumnNullAble 
                || dgv.Columns[e.ColumnIndex] == this.colColumnPrimary)
            {
                var bl = e.Value as Nullable<bool>;
                var value = string.Empty;
                if (bl != null)
                {
                    if (bl.Value)
                    {
                        value = "是";
                    }
                    else
                    {
                        value = "否";
                    }
                }
                e.Value = value;
            }
        }
    }
}
