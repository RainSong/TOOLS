using MSSqlserverPackage.Common;
using MSSqlserverPackage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MSSqlserverPackage
{
    public partial class ObjectsForm : Form
    {
        public DBObject selectObject;
        public ObjectsForm()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            InitComboxTypeItems();

            this.ToolStripMenuItemViewData.Click += ToolStripMenuItemViewData_Click;
            this.ToolStripMenuItemViewDesign.Click += ToolStripMenuItemViewDesign_Click;
            this.ToolStripMenuItemViewScript.Click += ToolStripMenuItemViewScript_Click;
        }

        private void ToolStripMenuItemViewScript_Click(object sender, EventArgs e)
        {
            ShowForm<ScriptForm>(this.selectObject);
        }

        private void ToolStripMenuItemViewDesign_Click(object sender, EventArgs e)
        {
            ShowForm<DesignForm>(this.selectObject);
        }

        private void ToolStripMenuItemViewData_Click(object sender, EventArgs e)
        {
            ShowForm<DataResultForm>(this.selectObject);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            var objects = DataService.GetObjects();
            var name = this.txtName.Text.Trim();
            Func<string, string> funcGetType = (strType) =>
             {
                 if (string.IsNullOrEmpty(strType)) return string.Empty;
                 strType = strType.Trim();
                 switch (strType)
                 {
                     case "AF": return "聚合函数(CLR)";
                     case "C": return "CHECK 约束";
                     case "D": return "DEFAULT（约束或独立）";
                     case "F": return "FOREIGN KEY 约束";
                     case "FN": return "SQL 标量函数";
                     case "FS": return "程序集(CLR) 标量函数";
                     case "FT": return "程序集(CLR) 表值函数";
                     case "IF": return "SQL 内联表值函数";
                     case "IT": return "内部表";
                     case "P": return "SQL 存储过程";
                     case "PC": return "程序集(CLR) 存储过程";
                     case "PG": return "计划指南";
                     case "PK": return "PRIMARY KEY 约束";
                     case "R": return "规则（旧式，独立）";
                     case "RF": return "复制筛选过程";
                     case "S": return "系统基表";
                     case "SN": return "同义词";
                     case "SQ": return "服务队列";
                     case "TA": return "程序集(CLR) DML 触发器";
                     case "TF": return "SQL 表值函数";
                     case "TR": return "SQL DML 触发器";
                     case "TT": return "表类型";
                     case "U": return "表（用户定义类型）";
                     case "UQ": return "UNIQUE 约束";
                     case "V": return "视图";
                     case "X": return "扩展存储过程";
                     default: return strType;

                 }
             };
            var result = (from o in objects
                          orderby o.TypeName, o.Name
                          select new
                          {
                              object_id = o.ObjectID,
                              name = o.Name,
                              type = o.TypeName,
                              type_full_name = funcGetType(o.TypeName),
                              type_desc = o.TypeDesc,
                              parent_id = o.ParentID,
                              create_date = o.CreateDate,
                              modify_date = o.ModifyDate,
                              o.IsMsShipped
                          }).AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(o => !string.IsNullOrEmpty(o.name) && o.name.Contains(name));
            }
            if (!this.checkBoxShowSysObject.Checked)
            {
                result = result.Where(o => !o.IsMsShipped);
            }
            if (this.comboxType.SelectedItem != null)
            {
                var keyValue = (KeyValuePair<string, string>)this.comboxType.SelectedItem;
                if (!string.IsNullOrEmpty(keyValue.Key) && !keyValue.Key.Equals("ALL"))
                {
                    result = result.Where(o => o.type != null && o.type.Trim().Equals(keyValue.Key));
                }
            }
            this.dataGridView1.DataSource = result.ToList();
        }

        private void DBObjectsForm_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void checkBoxShowSysObject_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

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

        private void InitComboxTypeItems()
        {
            var items = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string,string>("ALL","全部"),
                new KeyValuePair<string,string>("U","表（用户定义类型）"),
                new KeyValuePair<string,string>("V","视图"),
                new KeyValuePair<string,string>("P","SQL 存储过程"),
                new KeyValuePair<string,string>("FN","SQL 标量函数"),
                new KeyValuePair<string,string>("TF","SQL 表值函数"),
                new KeyValuePair<string,string>("IF","SQL 内联表值函数"),
                new KeyValuePair<string,string>("AF","聚合函数(CLR)"),
                new KeyValuePair<string,string>("C","CHECK 约束"),
                new KeyValuePair<string,string>("PK","PRIMARY KEY 约束"),
                new KeyValuePair<string,string>("F","FOREIGN KEY 约束"),
                new KeyValuePair<string,string>("UQ","UNIQUE 约束"),
                new KeyValuePair<string,string>("D","DEFAULT（约束或独立）"),
                new KeyValuePair<string,string>("FS","程序集(CLR) 标量函数"),
                new KeyValuePair<string,string>("FT","程序集(CLR) 表值函数"),
                new KeyValuePair<string,string>("IT","内部表"),
                new KeyValuePair<string,string>("PC","程序集(CLR) 存储过程"),
                new KeyValuePair<string,string>("PG","计划指南"),
                new KeyValuePair<string,string>("R","规则（旧式，独立）"),
                new KeyValuePair<string,string>("RF","复制筛选过程"),
                new KeyValuePair<string,string>("S","系统基表"),
                new KeyValuePair<string,string>("SN","同义词"),
                new KeyValuePair<string,string>("SQ","服务队列"),
                new KeyValuePair<string,string>("TA","程序集(CLR) DML 触发器"),
                new KeyValuePair<string,string>("TR","SQL DML 触发器"),
                new KeyValuePair<string,string>("TT","表类型"),
                new KeyValuePair<string,string>("X","扩展存储过程")
            };
            this.comboxType.DataSource = items;
            this.comboxType.DisplayMember = "Value";
            this.comboxType.ValueMember = "Key";
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = sender as DataGridView;
            if (e.Button == MouseButtons.Right && dgv != null)
            {
                dynamic data = dgv.Rows[e.RowIndex].DataBoundItem;
                var type = string.IsNullOrEmpty(data.type) ? string.Empty : data.type.Trim();
                int objId = 0;
                if (data.object_id == null) return;
                if (!int.TryParse(data.object_id.ToString(), out objId)) return;
                var types = new string[] { "U", "V", "P", "FN", "TF" };
                if (!types.Any(o => o.Equals(type))) return;
                if (type == "U")
                {
                    var tables = DataService.GetTables();
                    if (tables == null) return;
                    this.selectObject = tables.FirstOrDefault(o => o.ObjectID == objId);

                    this.ToolStripMenuItemViewData.Visible =
                        this.ToolStripMenuItemViewDesign.Visible =
                        this.ToolStripMenuItemViewScript.Visible = true;
                }
                else if (type == "V")
                {
                    var views = DataService.GetViews();
                    if (views == null) return;
                    this.selectObject = views.FirstOrDefault(o => o.ObjectID == objId);

                    this.ToolStripMenuItemViewData.Visible =
                        this.ToolStripMenuItemViewDesign.Visible =
                        this.ToolStripMenuItemViewScript.Visible = true;
                }
                else if (type == "P")
                {
                    var procedures = DataService.GetProcedures();
                    if (procedures == null) return;
                    this.selectObject = procedures.FirstOrDefault(o => o.ObjectID == objId);

                    this.ToolStripMenuItemViewDesign.Visible =
                        this.ToolStripMenuItemViewScript.Visible = true;
                }
                this.contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void ShowForm<T>(DBObject dbObj)
            where T : DataBaseObjectForm, new()
        {
            var parent = this.Parent.Parent.Parent as Main;
            if (parent != null)
            {
                parent.ShowForm<T>(dbObj);
            }
        }

        private DBObject GetSelect()
        {
            var selectRows = this.dataGridView1.SelectedRows;
            if (selectRows == null || selectRows.Count == 0) return null;
            var currentRow = selectRows[0];
            dynamic data = currentRow.DataBoundItem;
            if (data == null || data.object_id == null) return null;
            int objId = 0;
            if (int.TryParse(data.object_id.ToString(), out objId))
            {
                var objects = DataService.GetObjects();
                if (objects == null) return null;
                return objects.FirstOrDefault(o => o.ObjectID == objId);
            }
            return null;
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
                this.dataGridView1.Rows[e.RowIndex].Selected = true;
        }
    }
}
