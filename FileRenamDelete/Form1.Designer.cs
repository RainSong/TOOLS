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
            this.label1 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrower = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBoxDelete = new System.Windows.Forms.GroupBox();
            this.groupBoxRename = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbOnlyFile = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbDeleteDir = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            // 
            // btnBrower
            // 
            this.btnBrower.Location = new System.Drawing.Point(417, 7);
            this.btnBrower.Name = "btnBrower";
            this.btnBrower.Size = new System.Drawing.Size(75, 23);
            this.btnBrower.TabIndex = 2;
            this.btnBrower.Text = "浏览...";
            this.btnBrower.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 188);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(652, 250);
            this.dataGridView1.TabIndex = 3;
            // 
            // groupBoxDelete
            // 
            this.groupBoxDelete.Location = new System.Drawing.Point(12, 39);
            this.groupBoxDelete.Name = "groupBoxDelete";
            this.groupBoxDelete.Size = new System.Drawing.Size(315, 60);
            this.groupBoxDelete.TabIndex = 4;
            this.groupBoxDelete.TabStop = false;
            this.groupBoxDelete.Text = "删除以下文件(夹)";
            // 
            // groupBoxRename
            // 
            this.groupBoxRename.Location = new System.Drawing.Point(349, 39);
            this.groupBoxRename.Name = "groupBoxRename";
            this.groupBoxRename.Size = new System.Drawing.Size(315, 60);
            this.groupBoxRename.TabIndex = 5;
            this.groupBoxRename.TabStop = false;
            this.groupBoxRename.Text = "重命名文件(夹)";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(508, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(589, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "移至：";
            // 
            // cbOnlyFile
            // 
            this.cbOnlyFile.AutoSize = true;
            this.cbOnlyFile.Location = new System.Drawing.Point(453, 113);
            this.cbOnlyFile.Name = "cbOnlyFile";
            this.cbOnlyFile.Size = new System.Drawing.Size(84, 16);
            this.cbOnlyFile.TabIndex = 9;
            this.cbOnlyFile.Text = "只移动文件";
            this.cbOnlyFile.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(48, 111);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(293, 21);
            this.textBox1.TabIndex = 10;
            // 
            // cbDeleteDir
            // 
            this.cbDeleteDir.AutoSize = true;
            this.cbDeleteDir.Enabled = false;
            this.cbDeleteDir.Location = new System.Drawing.Point(544, 113);
            this.cbDeleteDir.Name = "cbDeleteDir";
            this.cbDeleteDir.Size = new System.Drawing.Size(120, 16);
            this.cbDeleteDir.TabIndex = 11;
            this.cbDeleteDir.Text = "移动后删除文件夹";
            this.cbDeleteDir.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(360, 109);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 452);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cbDeleteDir);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cbOnlyFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBoxRename);
            this.Controls.Add(this.groupBoxDelete);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnBrower);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "移除重命名文件(夹)";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrower;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBoxDelete;
        private System.Windows.Forms.GroupBox groupBoxRename;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbOnlyFile;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox cbDeleteDir;
        private System.Windows.Forms.Button button3;
    }
}

