using ScintillaNET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QueryDBObject
{
    public partial class FormQueryTable : Form
    {

        #region fields
        public int DataBaseID { get; set; }
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
        private List<Models.DBTable> objects = null;

        #endregion

        #region constructor

        public FormQueryTable(SqlConnectionStringBuilder conStringBuilder)
        {
            InitializeComponent();
            //this.StartPosition = FormStartPosition.CenterScreen;
            //this.WindowState = FormWindowState.Maximized;

            this.connectionString = conStringBuilder.ToString();
            this.server = conStringBuilder.DataSource;
            this.uid = conStringBuilder.UserID;
            this.dbName = conStringBuilder.InitialCatalog;

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
            var columnName = this.txtColumnNameKeyWord.Text.Trim();
            var objectName = this.txtNameKeyWord.Text.Trim();
            var sortField = (this.cboSort.SelectedValue ?? string.Empty).ToString();
            var sortType = (this.cboSort.SelectedValue ?? string.Empty).ToString();
            System.Collections.IDictionary dic = new Dictionary<string, object>();
            this.objects = Common.DataService.GetTables(this.connectionString, objectName, columnName, this.igrones, sortField, sortType, out dic);
            BindObjectGrid();

        }
        private void FormQueryTable_Load(object sender, EventArgs e)
        {
            //GetConnectInfo();
            ShowStipLabel();
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
                Models.DBTable obj = (sender as DataGridView).Rows[e.RowIndex].DataBoundItem as Models.DBTable;
                if (obj == null) return;
                if (obj.Columns == null || !obj.Columns.Any())
                {
                    try
                    {
                        var columns = Common.DataService.GetColumns(this.connectionString, obj.ObjectID);
                        var descriptions = Common.DataService.GetDescriptions(this.DataBaseID, obj.ObjectID);

                        var cols = (from c in columns
                                    join d in descriptions on c.ColumnID equals d.ColumnID into tmp_desc
                                    from ds in tmp_desc.DefaultIfEmpty()
                                    select new Models.DBColumn
                                    {
                                        ObjectID = c.ObjectID,
                                        ColumnID = c.ColumnID,
                                        Name = c.Name,
                                        DataType = c.DataType,
                                        MaxLength = c.MaxLength,
                                        Identity = c.Identity,
                                        IsPrimary = c.IsPrimary,
                                        Default = c.Default,
                                        Description = ds == null ? string.Empty : ds.Description

                                    }).ToList();

                        obj.Columns = cols;
                    }
                    catch (Exception ex)
                    {
                        MessageHelper.ShowMessage(ex.Message, "Error");
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
            IQueryable<Models.DBTable> objs = null;
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

        private void dgvColumnInfo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null) return;
            var data = dgv.Rows[e.RowIndex].DataBoundItem as Models.DBColumn;
            if (data == null) return;
            //var item = data[e.RowIndex];
            var tableId = data.ObjectID;
            var columnId = data.ColumnID;
            var content = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;
            try
            {
                Common.DataService.SaveDescription(this.DataBaseID, tableId, columnId, content);
            }
            catch (Exception ex)
            {
                Logger.Error("保存备注失败", ex);
            }
        }
    }
}
