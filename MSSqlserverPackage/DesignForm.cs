using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MSSqlserverPackage.Common;
using System.Linq;
using MSSqlserverPackage.Models;
using System.Collections.Generic;

namespace MSSqlserverPackage
{
    public partial class DesignForm : DataBaseObjectForm
    {
        private string cellOldValue;
        private Table table = null;
        private Models.View view = null;
        public DesignForm()
        {
            InitializeComponent();

            this.dataGridView1.AutoGenerateColumns = false;


            this.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            //this.Name = "DesignOf" + this.TypeName + this.dbObject.Name;
            this.dataGridView1.BorderStyle = BorderStyle.None;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Design_Load(object sender, EventArgs e)
        {
            this.Text = "设计：" + this.ObjectType + "-" + this.dbObject.Name;
            InitGridColumns();
            BindDataToGrid();
        }

        private void InitGridColumns()
        {
            #region
            if (this.dbObject is IQueryableObject)
            {
                var colName = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "name",
                    Frozen = true,
                    HeaderText = "名称",
                    Name = "colName",
                    ReadOnly = true,
                    Width = 150
                };
                var colDataType = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "dataType",
                    HeaderText = "数据类型",
                    Name = "colDataType",
                    ReadOnly = true
                };
                var colMaxLength = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "maxLength",
                    HeaderText = "最大长度",
                    Name = "colMaxLength",
                    ReadOnly = true
                };
                var colNullAble = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "nullAble",
                    HeaderText = "可空",
                    Name = "colNullAble",
                    ReadOnly = true
                };
                var colIdentity = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "identity",
                    HeaderText = "自增",
                    Name = "colIdentity",
                    ReadOnly = true
                };
                var colPrimaryKey = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "primaryKey",
                    HeaderText = "主键",
                    Name = "colPrimaryKey",
                    ReadOnly = true
                };
                var colFerenceKey = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ferenceKey",
                    HeaderText = "外键",
                    Name = "colFerenceKey",
                    ReadOnly = true,
                    Width = 200
                };
                var colComment = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "comment",
                    HeaderText = "说明",
                    Name = "colComment"
                };

                this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] {
                    colName,
                    colDataType,
                    colMaxLength,
                    colNullAble,
                    colIdentity,
                    colPrimaryKey,
                    colFerenceKey,
                    colComment
                });
            }
            #endregion
            #region
            else if (this.dbObject is IExecuteableObject)
            {
                var colName = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Name",
                    Frozen = true,
                    HeaderText = "名称",
                    Name = "colName",
                    ReadOnly = true,
                    Width = 150
                };
                var colDataType = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "DataType",
                    HeaderText = "数据类型",
                    Name = "colDataType",
                    ReadOnly = true
                };
                var colMaxLength = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "MaxLength",
                    HeaderText = "最大长度",
                    Name = "colMaxLength",
                    ReadOnly = true
                };
                var colNullAble = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "NullAble",
                    HeaderText = "可空",
                    Name = "colNullAble",
                    ReadOnly = true
                };
                var colIsOutPut = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "IsOutPut",
                    HeaderText = "是否输出",
                    Name = "colIsOutPut",
                    ReadOnly = true
                };
                var colHasDefaultValue = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "HasDefaultValue",
                    HeaderText = "是否有默认值",
                    Name = "colHasDefaultValue",
                    ReadOnly = true
                };
                var colDefaultValue = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "DefaultValue",
                    HeaderText = "默认值",
                    Name = "colDefaultValue",
                    ReadOnly = true
                };

                this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    colName,
                    colDataType,
                    colMaxLength,
                    colNullAble,
                    colIsOutPut,
                    colHasDefaultValue,
                    colDefaultValue
                });
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindDataToGrid()
        {
            if (this.dbObject is IQueryableObject)
            {
                IEnumerable<Column> columns = ((IQueryableObject)this.dbObject).Columns;
                if (columns != null)
                {
                    var result = new List<GridDataItemColumn>();
                    var table = this.dbObject as Table;
                    PrimaryKey primaryKey = null;
                    List<ReferenceKey> ferenceKeys = null;
                    if (table != null)
                    {
                        primaryKey = table.PrimaryKey;
                        ferenceKeys = table.ReferenceKeys == null ? null : table.ReferenceKeys.ToList();
                    }
                    columns.ToList().ForEach(c =>
                    {
                        var data = new GridDataItemColumn
                        {
                            ID = c.ID,
                            Name = c.Name,
                            DataType = c.DataType,
                            MaxLength = c.MaxLength,
                            NullAble = c.Nullable ? "是" : "否",
                            Identity = c.Identity == null ? string.Empty : string.Format("{0},{1}", c.Identity.SeedValue, c.Identity.IncrementValue)
                        };
                        if (c.ExtendedProperties != null)
                        {
                            var ep = c.ExtendedProperties.FirstOrDefault(p => !string.IsNullOrEmpty(p.Name) && p.Name.Equals("MS_Description"));
                            if (ep != null)
                                data.Comment = ep.Value;
                        }
                        if (primaryKey != null && primaryKey.ColumnID == c.ID)
                        {
                            data.PrimaryKey = primaryKey.Name;
                        }
                        if (ferenceKeys != null)
                        {
                            var key = ferenceKeys.FirstOrDefault(f => f.ColumnID == c.ID);
                            if (key != null && key.ReferenceObject != null && key.ReferenceColumn != null)
                                data.FerenceKey = string.Format("{0}.{1}", key.ReferenceObject.Name, key.ReferenceColumn.Name);
                        }
                        result.Add(data);
                    });
                    this.dataGridView1.DataSource = result;
                }
            }
            else if (this.dbObject is IExecuteableObject)
            {
                IEnumerable<Parameter> parameters = ((IExecuteableObject)this.dbObject).Parameters;
                if (parameters != null)
                {
                    var result = (from p in parameters
                                  select new
                                  {
                                      name = p.Name,
                                      dataType = p.DataType,
                                      maxLength = p.MaxLength,
                                      isOutPut = p.IsOutPut ? "是":"否",
                                      hasDefaultValue = p.HasDefaultValue? "有":"冇",
                                      defaultValue = p.DefaultValue
                                  }).ToList();
                    this.dataGridView1.DataSource = result;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();
            // https://stackoverflow.com/questions/9581626/show-row-number-in-row-header-of-a-datagridview

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
            var column = (this.dataGridView1.Rows[e.RowIndex].DataBoundItem) as Column;
            if (column != null)
            {
                //var row = this.dataGridView1.Rows[e.RowIndex];

                //row.Cells[this.colNullAble.Name].Value = column.Nullable ? "是" : "否";

                //var extendProperty = GetExtendedProperty(column);
                //row.Cells[this.colComment.Name].Value = extendProperty;

                //var identity = FormatIdentity(column);
                //row.Cells[this.colIdentity.Name].Value = identity;

                //if (this.table != null)
                //{
                //    if (table.PrimaryKey != null && table.PrimaryKey.ColumnID == column.ID)
                //    {
                //        row.Cells[this.colPrimaryKey.Name].Value = table.PrimaryKey.Name;
                //    }
                //    if (table.ReferenceKeys != null)
                //    {
                //        var currentReferenceKey = table.ReferenceKeys.FirstOrDefault(o => o.ColumnID == column.ID);
                //        if (currentReferenceKey != null)
                //        {
                //            row.Cells[this.colFerenceKey.Name].Value = currentReferenceKey.ReferenceObject.Name + '.' + currentReferenceKey.ReferenceColumn.Name;
                //        }
                //    }
                //}
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var value = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (value == null)
            {
                this.cellOldValue = string.Empty;
            }
            else
            {
                this.cellOldValue = value.ToString().Trim();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var value = cell.Value;
            var newValue = string.Empty;
            if (value != null)
            {
                newValue = value.ToString().Trim();
            }
            if (!this.cellOldValue.Equals(newValue))
            {
                var column = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as Column;
                if (column == null) return;
                string message = string.Empty;
                if (string.IsNullOrEmpty(cellOldValue))
                {
                    message = string.Format("确定要为字段“{0}”添加注释“{1}”吗？", column.Name, newValue);
                }
                else
                {
                    message = string.Format("确定要将字段“{0}”的注释有“{1}”改为“{2}”吗？", column.Name, cellOldValue, newValue);
                }
                if (DialogResult.OK == MessageBoxEx.Confrim(this, message))
                {
                    UpdateComment(column.Name, newValue);
                    if (this.table != null && column.ExtendedProperties != null)
                    {
                        var first = column.ExtendedProperties.FirstOrDefault(o => o.Name.Equals("MS_Description"));
                        first.Value = newValue;
                    }
                    cell.Value = newValue;
                }
                else
                {
                    cell.Value = this.cellOldValue;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="newValue"></param>
        private void UpdateComment(string colName, string newValue)
        {
            var sql = @"IF ( ( SELECT   COUNT(1)
                               FROM     sys.extended_properties
                                        LEFT JOIN sys.objects ON sys.extended_properties.major_id = sys.objects.object_id
                                        LEFT JOIN sys.columns ON sys.objects.object_id = sys.columns.object_id
                                                                 AND sys.extended_properties.minor_id = sys.columns.column_id
                               WHERE    sys.objects.name = @ObjectName
                                        AND sys.columns.name = @ColumnName
                                        AND sys.extended_properties.name = 'MS_Description'
                             ) > 0 ) 
                            BEGIN
                                EXEC  @Result = sp_updateextendedproperty @name = N'MS_Description',
                                    @value = @Value, @level0type = N'SCHEMA', @level0name = N'dbo',
                                    @level1type = N'TABLE', @level1name = @ObjectName,
                                    @level2type = N'COLUMN', @level2name = @ColumnName
                            END
                        ELSE 
                            BEGIN
                                EXEC @Result = sys.sp_addextendedproperty @name = N'MS_Description',
                                    @value = @Value, @level0type = N'SCHEMA', @level0name = N'dbo',
                                    @level1type = N'TABLE', @level1name = @ObjectName,
                                    @level2type = N'COLUMN', @level2name = @ColumnName
                            END";
            var paras = new SqlParameter[]
                {
                    new SqlParameter
                    {
                        SqlDbType= SqlDbType.NChar,
                        ParameterName = "@ObjectName",
                        Value = this.dbObject.Name
                    },
                    new SqlParameter
                    {
                        SqlDbType = SqlDbType.NChar,
                        ParameterName = "@ColumnName",
                        Value = colName
                    },
                    new SqlParameter
                    {
                        SqlDbType = SqlDbType.NChar,
                        ParameterName = "@Value",
                        Value = newValue
                    }
                };
            try
            {
                RainSong.Common.SqlHelper.ExecuteNonQuery(sql, paras);
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowError(this, "修改备注失败");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        private object GetComment(string colName)
        {
            var sql = @"SELECT  sys.extended_properties.value
                        FROM sys.objects
                             LEFT JOIN sys.columns ON sys.objects.object_id = sys.columns.object_id
                                LEFT JOIN sys.extended_properties ON sys.objects.object_id = sys.extended_properties.major_id
                                                                     AND sys.columns.column_id = sys.extended_properties.minor_id
                                                                     AND sys.extended_properties.name = 'MS_Description'
                        WHERE sys.objects.name = @ObjectName
                                AND sys.columns.name = @ColumnName";
            var paras = new SqlParameter[]
                {
                    new SqlParameter
                    {
                        SqlDbType= SqlDbType.NChar,
                        ParameterName = "@ObjectName",
                        Value = this.dbObject.Name
                    },
                    new SqlParameter
                    {
                        SqlDbType = SqlDbType.NChar,
                        ParameterName = "@ColumnName",
                        Value = colName
                    }
                };
            try
            {
                return RainSong.Common.SqlHelper.ExecuteScalar(sql, paras);
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowError(this, "获取备注失败");
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetExtendedProperty(Column column)
        {
            if (column.ExtendedProperties == null)
            {
                return string.Empty;
            }
            var first = column.ExtendedProperties.FirstOrDefault(o => o.Name.Equals("MS_Description"));
            if (first == null)
            {
                return string.Empty;
            }
            return first.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string FormatIdentity(Column column)
        {
            if (column.Identity == null)
            {
                return string.Empty;
            }
            return column.Identity.SeedValue + "," + column.Identity.IncrementValue;
        }
    }

    class GridDataItemColumn
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public int MaxLength { get; set; }
        public string NullAble { get; set; }
        public string Identity { get; set; }
        public string PrimaryKey { get; set; }
        public string FerenceKey { get; set; }
        public string Comment { get; set; }
    }
}
