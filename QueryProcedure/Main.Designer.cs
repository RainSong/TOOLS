namespace QueryProcedure
{
    partial class Main
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.txtNotContains = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtKeyWord = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grid = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModefiyDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblSplit1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblDataBase = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblSplit2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblUserID = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblSplit3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblExecuteTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblSplit4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLblRowCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbLeft = new System.Windows.Forms.GroupBox();
            this.panelGrid = new System.Windows.Forms.Panel();
            this.gbRight = new System.Windows.Forms.GroupBox();
            this.txtScript = new ScintillaNET.Scintilla();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.gbLeft.SuspendLayout();
            this.panelGrid.SuspendLayout();
            this.gbRight.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.txtNotContains);
            this.panelTop.Controls.Add(this.label3);
            this.panelTop.Controls.Add(this.txtName);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.btnQuery);
            this.panelTop.Controls.Add(this.txtKeyWord);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(5, 19);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(604, 65);
            this.panelTop.TabIndex = 0;
            // 
            // txtNotContains
            // 
            this.txtNotContains.Location = new System.Drawing.Point(265, 36);
            this.txtNotContains.Name = "txtNotContains";
            this.txtNotContains.Size = new System.Drawing.Size(314, 21);
            this.txtNotContains.TabIndex = 7;
            this.txtNotContains.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(245, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "不包含的关键字(多个关键字使用“，”分割)";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(321, 8);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(160, 21);
            this.txtName.TabIndex = 5;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(252, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "存储过程名";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(504, 6);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtKeyWord
            // 
            this.txtKeyWord.Location = new System.Drawing.Point(81, 8);
            this.txtKeyWord.Name = "txtKeyWord";
            this.txtKeyWord.Size = new System.Drawing.Size(160, 21);
            this.txtKeyWord.TabIndex = 2;
            this.txtKeyWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "查询关键字";
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.BackgroundColor = System.Drawing.Color.White;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colName,
            this.colCreateDate,
            this.colModefiyDate});
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(5, 0);
            this.grid.Margin = new System.Windows.Forms.Padding(0);
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowTemplate.Height = 23;
            this.grid.Size = new System.Drawing.Size(596, 253);
            this.grid.TabIndex = 1;
            this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_CellFormatting);
            this.grid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.grid_RowPostPaint);
            // 
            // colId
            // 
            this.colId.DataPropertyName = "id";
            this.colId.HeaderText = "ID";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Visible = false;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "name";
            this.colName.HeaderText = "名称";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 330;
            // 
            // colCreateDate
            // 
            this.colCreateDate.DataPropertyName = "create_date";
            this.colCreateDate.HeaderText = "创建时间";
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.ReadOnly = true;
            this.colCreateDate.Width = 150;
            // 
            // colModefiyDate
            // 
            this.colModefiyDate.DataPropertyName = "modify_date";
            this.colModefiyDate.HeaderText = "修改时间";
            this.colModefiyDate.Name = "colModefiyDate";
            this.colModefiyDate.ReadOnly = true;
            this.colModefiyDate.Width = 150;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel6,
            this.tssLblServer,
            this.tssLblSplit1,
            this.tssLblDataBase,
            this.tssLblSplit2,
            this.tssLblUserID,
            this.tssLblSplit3,
            this.tssLblExecuteTime,
            this.tssLblSplit4,
            this.tssLblRowCount,
            this.toolStripStatusLabel5});
            this.statusStrip1.Location = new System.Drawing.Point(0, 352);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(980, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(598, 17);
            this.toolStripStatusLabel6.Spring = true;
            this.toolStripStatusLabel6.Text = "    ";
            // 
            // tssLblServer
            // 
            this.tssLblServer.Name = "tssLblServer";
            this.tssLblServer.Size = new System.Drawing.Size(79, 17);
            this.tssLblServer.Text = "serverName";
            // 
            // tssLblSplit1
            // 
            this.tssLblSplit1.Name = "tssLblSplit1";
            this.tssLblSplit1.Size = new System.Drawing.Size(19, 17);
            this.tssLblSplit1.Text = " | ";
            // 
            // tssLblDataBase
            // 
            this.tssLblDataBase.Name = "tssLblDataBase";
            this.tssLblDataBase.Size = new System.Drawing.Size(59, 17);
            this.tssLblDataBase.Text = "dbName";
            // 
            // tssLblSplit2
            // 
            this.tssLblSplit2.Name = "tssLblSplit2";
            this.tssLblSplit2.Size = new System.Drawing.Size(19, 17);
            this.tssLblSplit2.Text = " | ";
            // 
            // tssLblUserID
            // 
            this.tssLblUserID.Name = "tssLblUserID";
            this.tssLblUserID.Size = new System.Drawing.Size(46, 17);
            this.tssLblUserID.Text = "userID";
            // 
            // tssLblSplit3
            // 
            this.tssLblSplit3.Name = "tssLblSplit3";
            this.tssLblSplit3.Size = new System.Drawing.Size(19, 17);
            this.tssLblSplit3.Text = " | ";
            // 
            // tssLblExecuteTime
            // 
            this.tssLblExecuteTime.Name = "tssLblExecuteTime";
            this.tssLblExecuteTime.Size = new System.Drawing.Size(56, 17);
            this.tssLblExecuteTime.Text = "00:00:00";
            // 
            // tssLblSplit4
            // 
            this.tssLblSplit4.Name = "tssLblSplit4";
            this.tssLblSplit4.Size = new System.Drawing.Size(19, 17);
            this.tssLblSplit4.Text = " | ";
            // 
            // tssLblRowCount
            // 
            this.tssLblRowCount.Name = "tssLblRowCount";
            this.tssLblRowCount.Size = new System.Drawing.Size(27, 17);
            this.tssLblRowCount.Text = "0行";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(24, 17);
            this.toolStripStatusLabel5.Text = "    ";
            // 
            // gbLeft
            // 
            this.gbLeft.Controls.Add(this.panelGrid);
            this.gbLeft.Controls.Add(this.panelTop);
            this.gbLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLeft.Location = new System.Drawing.Point(5, 5);
            this.gbLeft.Name = "gbLeft";
            this.gbLeft.Padding = new System.Windows.Forms.Padding(5, 5, 3, 5);
            this.gbLeft.Size = new System.Drawing.Size(612, 342);
            this.gbLeft.TabIndex = 4;
            this.gbLeft.TabStop = false;
            this.gbLeft.Text = "浏览";
            // 
            // panelGrid
            // 
            this.panelGrid.Controls.Add(this.grid);
            this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrid.Location = new System.Drawing.Point(5, 84);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Padding = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.panelGrid.Size = new System.Drawing.Size(604, 253);
            this.panelGrid.TabIndex = 2;
            // 
            // gbRight
            // 
            this.gbRight.Controls.Add(this.txtScript);
            this.gbRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRight.Location = new System.Drawing.Point(3, 5);
            this.gbRight.Name = "gbRight";
            this.gbRight.Padding = new System.Windows.Forms.Padding(3, 5, 5, 5);
            this.gbRight.Size = new System.Drawing.Size(352, 342);
            this.gbRight.TabIndex = 5;
            this.gbRight.TabStop = false;
            this.gbRight.Text = "存储过程脚本";
            // 
            // txtScript
            // 
            this.txtScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtScript.Location = new System.Drawing.Point(3, 19);
            this.txtScript.Name = "txtScript";
            this.txtScript.ReadOnly = true;
            this.txtScript.Size = new System.Drawing.Size(344, 318);
            this.txtScript.TabIndex = 0;
            this.txtScript.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtScript_KeyDown);
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.gbLeft);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(5, 5, 3, 5);
            this.panelLeft.Size = new System.Drawing.Size(620, 352);
            this.panelLeft.TabIndex = 6;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.gbRight);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(620, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(3, 5, 5, 5);
            this.panelRight.Size = new System.Drawing.Size(360, 352);
            this.panelRight.TabIndex = 7;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 374);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Main";
            this.Text = "Main";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.gbLeft.ResumeLayout(false);
            this.panelGrid.ResumeLayout(false);
            this.gbRight.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.TextBox txtKeyWord;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssLblServer;
        private System.Windows.Forms.ToolStripStatusLabel tssLblSplit1;
        private System.Windows.Forms.ToolStripStatusLabel tssLblDataBase;
        private System.Windows.Forms.ToolStripStatusLabel tssLblSplit2;
        private System.Windows.Forms.ToolStripStatusLabel tssLblUserID;
        private System.Windows.Forms.ToolStripStatusLabel tssLblSplit3;
        private System.Windows.Forms.ToolStripStatusLabel tssLblExecuteTime;
        private System.Windows.Forms.ToolStripStatusLabel tssLblSplit4;
        private System.Windows.Forms.ToolStripStatusLabel tssLblRowCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNotContains;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModefiyDate;
        private System.Windows.Forms.GroupBox gbLeft;
        private System.Windows.Forms.GroupBox gbRight;
        private ScintillaNET.Scintilla txtScript;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelGrid;
    }
}