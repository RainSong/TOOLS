using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI;
using System.IO;

namespace MSSqlserverPackage
{
    public partial class DataResultForm : DataBaseObjectForm
    {
        
        #region 字段
        /// <summary>
        /// DataGridView分页时页大小
        /// </summary>
        private int pageSize = 200;
        /// <summary>
        /// DataGridView分页时页码
        /// </summary>
        private int pageIndex = 1;
        /// <summary>
        /// DataGridView是否需要分页
        /// </summary>
        private bool needPageer = false;
        /// <summary>
        /// DataGridView展示给用户的行数
        /// </summary>
        private int displayCount = 0;
        private DataTable dtDataResult;
        private bool appding = false;
        #endregion
        #region 事件
        /// <summary>
        /// 窗体载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormData_Load(object sender, EventArgs e)
        {
            this.dtDataResult = GetData();
            this.needPageer = this.dtDataResult.Rows.Count > this.pageSize;
            SetDataGridColumns();

            BindDataToGrid();
        }
        /// <summary>
        /// 在行标头中添加编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
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
        /// <summary>
        /// DataGridView滚动条滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int times = 0;
        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (this.appding) return;
            if (e.ScrollOrientation != ScrollOrientation.VerticalScroll) return;
            if (e.NewValue <= e.OldValue) return;
            if (this.dataGridView1.RowCount >= this.dtDataResult.Rows.Count) return;
            var undisplayCount = this.dataGridView1.RowCount - this.dataGridView1.FirstDisplayedScrollingRowIndex - this.dataGridView1.DisplayedRowCount(true);
            if (undisplayCount <= 50)
            {
                System.Diagnostics.Debug.WriteLine("触发一次");
                this.appding = true;
                var taskUp = Task.Factory.StartNew(() =>
                {
                    Invoke(new Action(() =>
                    {
                        BindDataToGrid();
                    }));
                });
                return;
            }
        }
        #endregion
        #region 方法
        public DataResultForm()
        {
            InitializeComponent();
        }
        public DataResultForm(string typeName, string tableName, string connectionString)
        {
            InitializeComponent();


            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;

            this.Text = "数据：" + this.ObjectType + this.dbObject.Name;
            this.Name = "FromDataResultOf" + this.ObjectType + this.dbObject.Name;

            //this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BorderStyle = BorderStyle.None;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            var strCommandText = "select * from " + this.dbObject.Name;
            DataTable dt = null;
            try
            {
                dt = Common.SqlHelper.GetTable(strCommandText);
            }
            catch (Exception ex)
            {
                throw new Exception("获取" + this.ObjectType + this.dbObject.Name + "数据失败", ex);
            }
            return dt;
        }
        /// <summary>
        /// 根据数据源设置DataGridView设置列
        /// </summary>
        /// <param name="dt"></param>
        private void SetDataGridColumns()
        {
            foreach (DataColumn col in this.dtDataResult.Columns)
            {
                this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.ColumnName,
                    HeaderText = col.ColumnName,
                    DataPropertyName = col.ColumnName,
                    ReadOnly = true
                });
            }
        }
        /// <summary>
        /// 给DataGridView绑定数据
        /// </summary>
        /// <param name="dt"></param>
        private void BindDataToGrid()
        {
            if (!this.needPageer)
            {
                for (var i = 0; i < this.dtDataResult.Rows.Count; i++)
                {
                    var data = new List<object>();
                    foreach (DataGridViewColumn col in this.dataGridView1.Columns)
                    {
                        data.Add(this.dtDataResult.Rows[i][col.DataPropertyName]);
                    }
                    this.dataGridView1.Rows.Add(data.ToArray());
                }
            }
            else
            {
                var startIndex = (pageIndex - 1) * pageSize;
                var endIndex = pageIndex * pageSize;
                if (endIndex > this.dtDataResult.Rows.Count)
                {
                    endIndex = this.dtDataResult.Rows.Count;
                }
                for (; startIndex < endIndex; startIndex++)
                {
                    var data = new List<object>();
                    foreach (DataGridViewColumn col in this.dataGridView1.Columns)
                    {
                        data.Add(this.dtDataResult.Rows[startIndex][col.DataPropertyName]);
                    }
                    this.dataGridView1.Rows.Add(data.ToArray());
                }
                pageIndex++;
            }
            this.appding = false;
        }
        #endregion

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportToFile();
        }

        private void ExportToFile()
        {
            if (DialogResult.OK == this.folderBrowserDialog1.ShowDialog())
            {
                var path = this.folderBrowserDialog1.SelectedPath;
                NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
                var sheet = workbook.CreateSheet(this.dbObject.Name);
                var headerRow = sheet.CreateRow(0);
                for (var i = 0; i < this.dtDataResult.Columns.Count; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(this.dtDataResult.Columns[i].ColumnName);
                }
                for (var i = 0; i < this.dtDataResult.Rows.Count; i++)
                {
                    var newRow = sheet.CreateRow(i + 1);
                    for (var j = 0; j < this.dtDataResult.Columns.Count; j++)
                    {
                        newRow.CreateCell(j).SetCellValue(this.dtDataResult.Rows[i][j].ToString());
                    }
                }
                var filePath = Path.Combine(path, this.dbObject.Name + ".xls");
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                {
                    workbook.Write(fs);
                }
            }
        }
    }
}
