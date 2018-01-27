namespace QueryProcedure
{
    partial class LoginServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginServer));
            this.label1 = new System.Windows.Forms.Label();
            this.lblUID = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUID = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cboAuthType = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.labDataBases = new System.Windows.Forms.Label();
            this.cboDataBases = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器名称";
            // 
            // lblUID
            // 
            this.lblUID.AutoSize = true;
            this.lblUID.Location = new System.Drawing.Point(52, 114);
            this.lblUID.Name = "lblUID";
            this.lblUID.Size = new System.Drawing.Size(41, 12);
            this.lblUID.TabIndex = 1;
            this.lblUID.Text = "用户名";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(99, 28);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(168, 21);
            this.txtServer.TabIndex = 2;
            this.txtServer.Text = ".";
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            this.txtServer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtServer_KeyDown);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(52, 156);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(29, 12);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "密码";
            // 
            // txtUID
            // 
            this.txtUID.Location = new System.Drawing.Point(99, 111);
            this.txtUID.Name = "txtUID";
            this.txtUID.Size = new System.Drawing.Size(168, 21);
            this.txtUID.TabIndex = 4;
            this.txtUID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUID_KeyDown);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(99, 153);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(168, 21);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(192, 230);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "身份验证";
            // 
            // cboAuthType
            // 
            this.cboAuthType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAuthType.FormattingEnabled = true;
            this.cboAuthType.Location = new System.Drawing.Point(99, 70);
            this.cboAuthType.Name = "cboAuthType";
            this.cboAuthType.Size = new System.Drawing.Size(168, 20);
            this.cboAuthType.TabIndex = 8;
            this.cboAuthType.SelectedIndexChanged += new System.EventHandler(this.cboAuthType_SelectedIndexChanged);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(99, 230);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 9;
            this.btnTest.Text = "测试连接";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // labDataBases
            // 
            this.labDataBases.AutoSize = true;
            this.labDataBases.Location = new System.Drawing.Point(52, 197);
            this.labDataBases.Name = "labDataBases";
            this.labDataBases.Size = new System.Drawing.Size(41, 12);
            this.labDataBases.TabIndex = 10;
            this.labDataBases.Text = "数据库";
            // 
            // cboDataBases
            // 
            this.cboDataBases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataBases.FormattingEnabled = true;
            this.cboDataBases.Location = new System.Drawing.Point(99, 194);
            this.cboDataBases.Name = "cboDataBases";
            this.cboDataBases.Size = new System.Drawing.Size(168, 20);
            this.cboDataBases.TabIndex = 11;
            // 
            // LoginServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 276);
            this.Controls.Add(this.cboDataBases);
            this.Controls.Add(this.labDataBases);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.cboAuthType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUID);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblUID);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginServer";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.LoginServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUID;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUID;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboAuthType;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label labDataBases;
        private System.Windows.Forms.ComboBox cboDataBases;
    }
}

