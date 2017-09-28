namespace MSSqlserverPackage
{
    partial class DesignForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaxLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNullAble = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIdentity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrimaryKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFerenceKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colDataType,
            this.colMaxLength,
            this.colNullAble,
            this.colIdentity,
            this.colPrimaryKey,
            this.colFerenceKey,
            this.colComment});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 50;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(676, 362);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // colName
            // 
            this.colName.DataPropertyName = "name";
            this.colName.Frozen = true;
            this.colName.HeaderText = "名称";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 150;
            // 
            // colDataType
            // 
            this.colDataType.DataPropertyName = "dataType";
            this.colDataType.HeaderText = "数据类型";
            this.colDataType.Name = "colDataType";
            this.colDataType.ReadOnly = true;
            // 
            // colMaxLength
            // 
            this.colMaxLength.DataPropertyName = "maxLength";
            this.colMaxLength.HeaderText = "最大长度";
            this.colMaxLength.Name = "colMaxLength";
            this.colMaxLength.ReadOnly = true;
            // 
            // colNullAble
            // 
            this.colNullAble.HeaderText = "可空";
            this.colNullAble.Name = "colNullAble";
            this.colNullAble.ReadOnly = true;
            // 
            // colIdentity
            // 
            this.colIdentity.HeaderText = "自增";
            this.colIdentity.Name = "colIdentity";
            this.colIdentity.ReadOnly = true;
            // 
            // colPrimaryKey
            // 
            this.colPrimaryKey.HeaderText = "主键";
            this.colPrimaryKey.Name = "colPrimaryKey";
            this.colPrimaryKey.ReadOnly = true;
            // 
            // colFerenceKey
            // 
            this.colFerenceKey.HeaderText = "外键";
            this.colFerenceKey.Name = "colFerenceKey";
            this.colFerenceKey.ReadOnly = true;
            this.colFerenceKey.Width = 200;
            // 
            // colComment
            // 
            this.colComment.DataPropertyName = "comment";
            this.colComment.HeaderText = "说明";
            this.colComment.Name = "colComment";
            // 
            // DesignForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 362);
            this.Controls.Add(this.dataGridView1);
            this.Name = "DesignForm";
            this.Text = "Design";
            this.Load += new System.EventHandler(this.Design_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaxLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNullAble;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdentity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrimaryKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFerenceKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
    }
}