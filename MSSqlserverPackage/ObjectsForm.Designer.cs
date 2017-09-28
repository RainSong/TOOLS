namespace MSSqlserverPackage
{
    partial class ObjectsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTypeDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModfiyDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboxType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxShowSysObject = new System.Windows.Forms.CheckBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemViewData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemViewDesign = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemViewScript = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colName,
            this.colParentID,
            this.colType,
            this.colTypeDesc,
            this.colCreateTime,
            this.colModfiyDate});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 50);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(750, 404);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseUp);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // colID
            // 
            this.colID.DataPropertyName = "object_id";
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "name";
            this.colName.HeaderText = "名称";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 400;
            // 
            // colParentID
            // 
            this.colParentID.DataPropertyName = "parent_id";
            this.colParentID.HeaderText = "父对象ID";
            this.colParentID.Name = "colParentID";
            this.colParentID.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.DataPropertyName = "type";
            this.colType.HeaderText = "类型";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colTypeDesc
            // 
            this.colTypeDesc.DataPropertyName = "type_full_name";
            this.colTypeDesc.HeaderText = "类型描述";
            this.colTypeDesc.Name = "colTypeDesc";
            this.colTypeDesc.ReadOnly = true;
            this.colTypeDesc.Width = 150;
            // 
            // colCreateTime
            // 
            this.colCreateTime.DataPropertyName = "create_date";
            this.colCreateTime.HeaderText = "添加时间";
            this.colCreateTime.Name = "colCreateTime";
            this.colCreateTime.ReadOnly = true;
            this.colCreateTime.Width = 130;
            // 
            // colModfiyDate
            // 
            this.colModfiyDate.DataPropertyName = "modify_date";
            this.colModfiyDate.HeaderText = "修改时间";
            this.colModfiyDate.Name = "colModfiyDate";
            this.colModfiyDate.ReadOnly = true;
            this.colModfiyDate.Width = 130;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboxType);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.checkBoxShowSysObject);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(750, 50);
            this.panel1.TabIndex = 1;
            // 
            // comboxType
            // 
            this.comboxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxType.FormattingEnabled = true;
            this.comboxType.Location = new System.Drawing.Point(272, 17);
            this.comboxType.Name = "comboxType";
            this.comboxType.Size = new System.Drawing.Size(199, 20);
            this.comboxType.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(213, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "对象类型";
            // 
            // checkBoxShowSysObject
            // 
            this.checkBoxShowSysObject.AutoSize = true;
            this.checkBoxShowSysObject.Location = new System.Drawing.Point(489, 19);
            this.checkBoxShowSysObject.Name = "checkBoxShowSysObject";
            this.checkBoxShowSysObject.Size = new System.Drawing.Size(96, 16);
            this.checkBoxShowSysObject.TabIndex = 7;
            this.checkBoxShowSysObject.Text = "显示系统对象";
            this.checkBoxShowSysObject.UseVisualStyleBackColor = true;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(605, 15);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(71, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(120, 21);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "对象名称";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemViewData,
            this.ToolStripMenuItemViewDesign,
            this.ToolStripMenuItemViewScript});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 70);
            // 
            // ToolStripMenuItemViewData
            // 
            this.ToolStripMenuItemViewData.Name = "ToolStripMenuItemViewData";
            this.ToolStripMenuItemViewData.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItemViewData.Text = "查看数据";
            this.ToolStripMenuItemViewData.Visible = false;
            // 
            // ToolStripMenuItemViewDesign
            // 
            this.ToolStripMenuItemViewDesign.Name = "ToolStripMenuItemViewDesign";
            this.ToolStripMenuItemViewDesign.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItemViewDesign.Text = "查看设计";
            this.ToolStripMenuItemViewDesign.Visible = false;
            // 
            // ToolStripMenuItemViewScript
            // 
            this.ToolStripMenuItemViewScript.Name = "ToolStripMenuItemViewScript";
            this.ToolStripMenuItemViewScript.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItemViewScript.Text = "查看脚本";
            // 
            // ObjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 454);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Name = "ObjectsForm";
            this.Text = "数据库对象概览";
            this.Load += new System.EventHandler(this.DBObjectsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTypeDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModfiyDate;
        private System.Windows.Forms.CheckBox checkBoxShowSysObject;
        private System.Windows.Forms.ComboBox comboxType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemViewData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemViewDesign;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemViewScript;
    }
}