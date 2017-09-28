namespace MSSqlserverPackage
{
    partial class Main
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemFileConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.打包ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemExportDesign = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemExportDesignToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemExportDesignToSqlScript = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSeeting = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSelectLanuage = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDesign = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemScript = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemFile,
            this.MenuItemExportDesign,
            this.MenuItemSeeting});
            this.menuStrip1.Location = new System.Drawing.Point(20, 60);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(668, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuItemFile
            // 
            this.MenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemFileConnect,
            this.MenuItemFileOpen,
            this.打包ToolStripMenuItem});
            this.MenuItemFile.Name = "MenuItemFile";
            this.MenuItemFile.Size = new System.Drawing.Size(44, 21);
            this.MenuItemFile.Text = "文件";
            // 
            // MenuItemFileConnect
            // 
            this.MenuItemFileConnect.Name = "MenuItemFileConnect";
            this.MenuItemFileConnect.Size = new System.Drawing.Size(100, 22);
            this.MenuItemFileConnect.Text = "链接";
            this.MenuItemFileConnect.Click += new System.EventHandler(this.MenuItemFileConnect_Click);
            // 
            // MenuItemFileOpen
            // 
            this.MenuItemFileOpen.Name = "MenuItemFileOpen";
            this.MenuItemFileOpen.Size = new System.Drawing.Size(100, 22);
            this.MenuItemFileOpen.Text = "打开";
            // 
            // 打包ToolStripMenuItem
            // 
            this.打包ToolStripMenuItem.Name = "打包ToolStripMenuItem";
            this.打包ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.打包ToolStripMenuItem.Text = "打包";
            // 
            // MenuItemExportDesign
            // 
            this.MenuItemExportDesign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemExportDesignToExcel,
            this.MenuItemExportDesignToSqlScript});
            this.MenuItemExportDesign.Name = "MenuItemExportDesign";
            this.MenuItemExportDesign.Size = new System.Drawing.Size(104, 21);
            this.MenuItemExportDesign.Text = "导出数据库设计";
            // 
            // MenuItemExportDesignToExcel
            // 
            this.MenuItemExportDesignToExcel.Name = "MenuItemExportDesignToExcel";
            this.MenuItemExportDesignToExcel.Size = new System.Drawing.Size(152, 22);
            this.MenuItemExportDesignToExcel.Text = "到Execel文件";
            // 
            // MenuItemExportDesignToSqlScript
            // 
            this.MenuItemExportDesignToSqlScript.Name = "MenuItemExportDesignToSqlScript";
            this.MenuItemExportDesignToSqlScript.Size = new System.Drawing.Size(152, 22);
            this.MenuItemExportDesignToSqlScript.Text = "到Script文件";
            // 
            // MenuItemSeeting
            // 
            this.MenuItemSeeting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemSelectLanuage});
            this.MenuItemSeeting.Name = "MenuItemSeeting";
            this.MenuItemSeeting.Size = new System.Drawing.Size(44, 21);
            this.MenuItemSeeting.Text = "设置";
            // 
            // MenuItemSelectLanuage
            // 
            this.MenuItemSelectLanuage.Name = "MenuItemSelectLanuage";
            this.MenuItemSelectLanuage.Size = new System.Drawing.Size(124, 22);
            this.MenuItemSelectLanuage.Text = "选择语言";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(20, 85);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(180, 424);
            this.treeView1.TabIndex = 1;
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemData,
            this.ToolStripMenuItemDesign,
            this.ToolStripMenuItemScript});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 70);
            // 
            // ToolStripMenuItemData
            // 
            this.ToolStripMenuItemData.Name = "ToolStripMenuItemData";
            this.ToolStripMenuItemData.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItemData.Text = "查看数据";
            // 
            // ToolStripMenuItemDesign
            // 
            this.ToolStripMenuItemDesign.Name = "ToolStripMenuItemDesign";
            this.ToolStripMenuItemDesign.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItemDesign.Text = "查看设计";
            // 
            // ToolStripMenuItemScript
            // 
            this.ToolStripMenuItemScript.Name = "ToolStripMenuItemScript";
            this.ToolStripMenuItemScript.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItemScript.Text = "查看设计脚本";
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(200, 85);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Size = new System.Drawing.Size(488, 424);
            this.tabControl1.TabIndex = 2;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 529);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "MainForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem MenuItemFileConnect;
        private System.Windows.Forms.ToolStripMenuItem MenuItemFileOpen;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDesign;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemScript;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSeeting;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSelectLanuage;
        private System.Windows.Forms.ToolStripMenuItem MenuItemExportDesign;
        private System.Windows.Forms.ToolStripMenuItem MenuItemExportDesignToExcel;
        private System.Windows.Forms.ToolStripMenuItem MenuItemExportDesignToSqlScript;
        private System.Windows.Forms.ToolStripMenuItem 打包ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
    }
}

