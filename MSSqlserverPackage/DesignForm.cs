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

            //this.Text = "设计：" + this.ObjectType + this.dbObject.Name;
            //this.Name = "DesignOf" + this.ObjectType + this.dbObject.Name;
            this.dataGridView1.BorderStyle = BorderStyle.None;

            this.colMaxLength.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void Design_Load(object sender, EventArgs e)
        {
            BindDataToGrid();

            this.table = this.dbObject as Table;
        }

        private void BindDataToGrid()
        {

            IEnumerable<Column> columns = null;
            if (this.dbObject is Table)
            {
                columns = ((Table)this.dbObject).Columns;
            }
            else if (this.dbObject is Models.View)
            {
                columns = ((Models.View)this.dbObject).Columns;
            }
            if (columns != null)
            {

                //var result = (from c in columns
                //              select new
                //              {
                //                  name = c.Name,
                //                  dataType = c.DataType,
                //                  maxLength = c.MaxLength,
                //                  nullable = c.Nullable ? "是" : "否",
                //                  identity = c.Identity == null ? string.Empty : string.Format("{0},{1}", c.Identity.SeedValue, c.Identity.IncrementValue),
                //                  comment = funcGetComment(c)
                //              }).ToList();
                this.dataGridView1.DataSource = columns.ToList();
            }
        }

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

            //var drv = (this.dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView);
            //if (drv != null)
            //{
            //    var nullable = drv.Row.Field<bool>("is_nullable");
            //    var cellNullable = this.dataGridView1.Rows[e.RowIndex].Cells[this.colNullAble.Name];
            //    if (nullable)
            //    {
            //        cellNullable.Value = "是";
            //    }
            //    else
            //    {
            //        cellNullable.Value = "否";
            //    }
            //}
            var column = (this.dataGridView1.Rows[e.RowIndex].DataBoundItem) as Column;
            if (column != null)
            {
                var row = this.dataGridView1.Rows[e.RowIndex];

                row.Cells[this.colNullAble.Name].Value = column.Nullable ? "是" : "否";

                var extendProperty = GetExtendedProperty(column);
                row.Cells[this.colComment.Name].Value = extendProperty;

                var identity = GetIdentity(column);
                row.Cells[this.colIdentity.Name].Value = identity;

                if (this.table != null)
                {
                    if (table.PrimaryKey != null && table.PrimaryKey.ColumnID == column.ID)
                    {
                        row.Cells[this.colPrimaryKey.Name].Value = table.PrimaryKey.Name;
                    }
                    if (table.ReferenceKeys != null)
                    {
                        var currentReferenceKey = table.ReferenceKeys.FirstOrDefault(o => o.ColumnID == column.ID);
                        if (currentReferenceKey != null)
                        {
                            row.Cells[this.colFerenceKey.Name].Value = currentReferenceKey.ReferenceObject.Name + '.' + currentReferenceKey.ReferenceColumn.Name;
                        }
                    }
                }
            }
        }

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
            var paraResult =
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output,
                    ParameterName = "@Result"
                };
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
                },
                paraResult
                };
            try
            {
                SqlHelper.ExecuteNonQuery(sql, paras);
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowError(this, "修改备注失败");
            }
        }

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
                return SqlHelper.ExecuteScalar(sql, paras);
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowError(this, "获取备注失败");
                return string.Empty;
            }
        }

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

        private string GetIdentity(Column column)
        {
            if (column.Identity == null)
            {
                return string.Empty;
            }
            return column.Identity.SeedValue + "," + column.Identity.IncrementValue;
        }
    }
}
