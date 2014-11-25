namespace FileRenamDelete
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrower = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExtension = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMD5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxDelete = new System.Windows.Forms.GroupBox();
            this.cmbDelMD5 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDelExtension = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDeleteKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxRename = new System.Windows.Forms.GroupBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.txt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtReDel = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnGenMD5 = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbOnlyFile = new System.Windows.Forms.CheckBox();
            this.txtTargetPath = new System.Windows.Forms.TextBox();
            this.cbDeleteDir = new System.Windows.Forms.CheckBox();
            this.btnTarBrower = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBoxDelete.SuspendLayout();
            this.groupBoxRename.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "路径：";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(59, 9);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(341, 21);
            this.txtPath.TabIndex = 1;
            this.txtPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPath_KeyDown);
            // 
            // btnBrower
            // 
            this.btnBrower.Location = new System.Drawing.Point(417, 7);
            this.btnBrower.Name = "btnBrower";
            this.btnBrower.Size = new System.Drawing.Size(75, 23);
            this.btnBrower.TabIndex = 2;
            this.btnBrower.Text = "浏览...";
            this.btnBrower.UseVisualStyleBackColor = true;
            this.btnBrower.Click += new System.EventHandler(this.btnBrower_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPath,
            this.colSize,
            this.colExtension,
            this.colMD5});
            this.dataGridView1.Location = new System.Drawing.Point(12, 188);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(652, 230);
            this.dataGridView1.TabIndex = 3;
            // 
            // colPath
            // 
            this.colPath.DataPropertyName = "Name";
            this.colPath.HeaderText = "路径";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 250;
            // 
            // colSize
            // 
            this.colSize.DataPropertyName = "Size";
            this.colSize.HeaderText = "大小";
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            // 
            // colExtension
            // 
            this.colExtension.DataPropertyName = "Extension";
            this.colExtension.HeaderText = "后缀";
            this.colExtension.Name = "colExtension";
            this.colExtension.ReadOnly = true;
            this.colExtension.Width = 80;
            // 
            // colMD5
            // 
            this.colMD5.DataPropertyName = "MD5";
            this.colMD5.HeaderText = "MD5";
            this.colMD5.Name = "colMD5";
            this.colMD5.ReadOnly = true;
            this.colMD5.Width = 180;
            // 
            // groupBoxDelete
            // 
            this.groupBoxDelete.Controls.Add(this.cmbDelMD5);
            this.groupBoxDelete.Controls.Add(this.label5);
            this.groupBoxDelete.Controls.Add(this.cmbDelExtension);
            this.groupBoxDelete.Controls.Add(this.label4);
            this.groupBoxDelete.Controls.Add(this.txtDeleteKey);
            this.groupBoxDelete.Controls.Add(this.label3);
            this.groupBoxDelete.Location = new System.Drawing.Point(12, 39);
            this.groupBoxDelete.Name = "groupBoxDelete";
            this.groupBoxDelete.Size = new System.Drawing.Size(315, 100);
            this.groupBoxDelete.TabIndex = 4;
            this.groupBoxDelete.TabStop = false;
            this.groupBoxDelete.Text = "删除以下文件(夹)";
            // 
            // cmbDelMD5
            // 
            this.cmbDelMD5.FormattingEnabled = true;
            this.cmbDelMD5.Location = new System.Drawing.Point(90, 72);
            this.cmbDelMD5.Name = "cmbDelMD5";
            this.cmbDelMD5.Size = new System.Drawing.Size(202, 20);
            this.cmbDelMD5.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "MD5为：";
            // 
            // cmbDelExtension
            // 
            this.cmbDelExtension.FormattingEnabled = true;
            this.cmbDelExtension.Location = new System.Drawing.Point(90, 44);
            this.cmbDelExtension.Name = "cmbDelExtension";
            this.cmbDelExtension.Size = new System.Drawing.Size(202, 20);
            this.cmbDelExtension.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "后缀名为：";
            // 
            // txtDeleteKey
            // 
            this.txtDeleteKey.Location = new System.Drawing.Point(90, 18);
            this.txtDeleteKey.Name = "txtDeleteKey";
            this.txtDeleteKey.Size = new System.Drawing.Size(202, 21);
            this.txtDeleteKey.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "文件名包含：";
            // 
            // groupBoxRename
            // 
            this.groupBoxRename.Controls.Add(this.textBox5);
            this.groupBoxRename.Controls.Add(this.txt);
            this.groupBoxRename.Controls.Add(this.label8);
            this.groupBoxRename.Controls.Add(this.label7);
            this.groupBoxRename.Controls.Add(this.txtReDel);
            this.groupBoxRename.Controls.Add(this.label6);
            this.groupBoxRename.Location = new System.Drawing.Point(349, 39);
            this.groupBoxRename.Name = "groupBoxRename";
            this.groupBoxRename.Size = new System.Drawing.Size(315, 100);
            this.groupBoxRename.TabIndex = 5;
            this.groupBoxRename.TabStop = false;
            this.groupBoxRename.Text = "重命名文件(夹)";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(82, 73);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(217, 21);
            this.textBox5.TabIndex = 5;
            // 
            // txt
            // 
            this.txt.Location = new System.Drawing.Point(82, 45);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(217, 21);
            this.txt.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "添加后缀：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "添加前缀：";
            // 
            // txtReDel
            // 
            this.txtReDel.Location = new System.Drawing.Point(82, 17);
            this.txtReDel.Name = "txtReDel";
            this.txtReDel.Size = new System.Drawing.Size(217, 21);
            this.txtReDel.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "移除字符：";
            // 
            // btnGenMD5
            // 
            this.btnGenMD5.Location = new System.Drawing.Point(504, 7);
            this.btnGenMD5.Name = "btnGenMD5";
            this.btnGenMD5.Size = new System.Drawing.Size(75, 23);
            this.btnGenMD5.TabIndex = 6;
            this.btnGenMD5.Text = "生成MD5";
            this.btnGenMD5.UseVisualStyleBackColor = true;
            this.btnGenMD5.Click += new System.EventHandler(this.btnGenMD5_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(589, 7);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 7;
            this.btnExecute.Text = "执行";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "移至：";
            // 
            // cbOnlyFile
            // 
            this.cbOnlyFile.AutoSize = true;
            this.cbOnlyFile.Location = new System.Drawing.Point(453, 155);
            this.cbOnlyFile.Name = "cbOnlyFile";
            this.cbOnlyFile.Size = new System.Drawing.Size(84, 16);
            this.cbOnlyFile.TabIndex = 9;
            this.cbOnlyFile.Text = "只移动文件";
            this.cbOnlyFile.UseVisualStyleBackColor = true;
            this.cbOnlyFile.CheckedChanged += new System.EventHandler(this.cbOnlyFile_CheckedChanged);
            // 
            // txtTargetPath
            // 
            this.txtTargetPath.Location = new System.Drawing.Point(48, 153);
            this.txtTargetPath.Name = "txtTargetPath";
            this.txtTargetPath.Size = new System.Drawing.Size(293, 21);
            this.txtTargetPath.TabIndex = 10;
            // 
            // cbDeleteDir
            // 
            this.cbDeleteDir.AutoSize = true;
            this.cbDeleteDir.Enabled = false;
            this.cbDeleteDir.Location = new System.Drawing.Point(544, 155);
            this.cbDeleteDir.Name = "cbDeleteDir";
            this.cbDeleteDir.Size = new System.Drawing.Size(120, 16);
            this.cbDeleteDir.TabIndex = 11;
            this.cbDeleteDir.Text = "移动后删除文件夹";
            this.cbDeleteDir.UseVisualStyleBackColor = true;
            // 
            // btnTarBrower
            // 
            this.btnTarBrower.Location = new System.Drawing.Point(360, 151);
            this.btnTarBrower.Name = "btnTarBrower";
            this.btnTarBrower.Size = new System.Drawing.Size(75, 23);
            this.btnTarBrower.TabIndex = 12;
            this.btnTarBrower.Text = "浏览...";
            this.btnTarBrower.UseVisualStyleBackColor = true;
            this.btnTarBrower.Click += new System.EventHandler(this.btnTarBrower_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "请选择一个文件夹";
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 430);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(676, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(200, 16);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 452);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnTarBrower);
            this.Controls.Add(this.cbDeleteDir);
            this.Controls.Add(this.txtTargetPath);
            this.Controls.Add(this.cbOnlyFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.btnGenMD5);
            this.Controls.Add(this.groupBoxRename);
            this.Controls.Add(this.groupBoxDelete);
            this.Controls.Add(this.btnBrower);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "移除重命名文件(夹)";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBoxDelete.ResumeLayout(false);
            this.groupBoxDelete.PerformLayout();
            this.groupBoxRename.ResumeLayout(false);
            this.groupBoxRename.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrower;
        private System.Windows.Forms.GroupBox groupBoxDelete;
        private System.Windows.Forms.GroupBox groupBoxRename;
        private System.Windows.Forms.Button btnGenMD5;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbOnlyFile;
        private System.Windows.Forms.TextBox txtTargetPath;
        private System.Windows.Forms.CheckBox cbDeleteDir;
        private System.Windows.Forms.Button btnTarBrower;
        private System.Windows.Forms.ComboBox cmbDelMD5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDelExtension;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDeleteKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox txt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtReDel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExtension;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMD5;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
    }
}

