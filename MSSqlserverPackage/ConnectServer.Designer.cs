namespace MSSqlserverPackage
{
    partial class ConnectServer
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rbWindowsUser = new System.Windows.Forms.RadioButton();
            this.rbPWD = new System.Windows.Forms.RadioButton();
            this.labUserName = new System.Windows.Forms.Label();
            this.labPWD = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPWD = new System.Windows.Forms.TextBox();
            this.cbDataBase = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSure = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入服务器名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "登录凭据";
            // 
            // rbWindowsUser
            // 
            this.rbWindowsUser.AutoSize = true;
            this.rbWindowsUser.Checked = true;
            this.rbWindowsUser.Location = new System.Drawing.Point(53, 82);
            this.rbWindowsUser.Name = "rbWindowsUser";
            this.rbWindowsUser.Size = new System.Drawing.Size(191, 16);
            this.rbWindowsUser.TabIndex = 2;
            this.rbWindowsUser.TabStop = true;
            this.rbWindowsUser.Text = "使用 Windows NT 集成安全设置";
            this.rbWindowsUser.UseVisualStyleBackColor = true;
            this.rbWindowsUser.CheckedChanged += new System.EventHandler(this.rbWindowsUser_CheckedChanged);
            // 
            // rbPWD
            // 
            this.rbPWD.AutoSize = true;
            this.rbPWD.Location = new System.Drawing.Point(53, 108);
            this.rbPWD.Name = "rbPWD";
            this.rbPWD.Size = new System.Drawing.Size(107, 16);
            this.rbPWD.TabIndex = 3;
            this.rbPWD.Text = "使用户名和密码";
            this.rbPWD.UseVisualStyleBackColor = true;
            // 
            // labUserName
            // 
            this.labUserName.AutoSize = true;
            this.labUserName.Location = new System.Drawing.Point(53, 140);
            this.labUserName.Name = "labUserName";
            this.labUserName.Size = new System.Drawing.Size(41, 12);
            this.labUserName.TabIndex = 4;
            this.labUserName.Text = "用户名";
            // 
            // labPWD
            // 
            this.labPWD.AutoSize = true;
            this.labPWD.Location = new System.Drawing.Point(53, 167);
            this.labPWD.Name = "labPWD";
            this.labPWD.Size = new System.Drawing.Size(29, 12);
            this.labPWD.TabIndex = 5;
            this.labPWD.Text = "密码";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 229);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "选择数据库";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(123, 23);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(180, 21);
            this.txtServer.TabIndex = 7;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(123, 137);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(180, 21);
            this.txtUserName.TabIndex = 9;
            // 
            // txtPWD
            // 
            this.txtPWD.Location = new System.Drawing.Point(123, 164);
            this.txtPWD.Name = "txtPWD";
            this.txtPWD.PasswordChar = '*';
            this.txtPWD.Size = new System.Drawing.Size(180, 21);
            this.txtPWD.TabIndex = 10;
            // 
            // cbDataBase
            // 
            this.cbDataBase.FormattingEnabled = true;
            this.cbDataBase.Location = new System.Drawing.Point(123, 226);
            this.cbDataBase.Name = "cbDataBase";
            this.cbDataBase.Size = new System.Drawing.Size(180, 20);
            this.cbDataBase.TabIndex = 11;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(228, 193);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 12;
            this.btnTest.Text = "测试链接";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbDataBase);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtPWD);
            this.panel1.Controls.Add(this.rbWindowsUser);
            this.panel1.Controls.Add(this.txtUserName);
            this.panel1.Controls.Add(this.rbPWD);
            this.panel1.Controls.Add(this.labUserName);
            this.panel1.Controls.Add(this.txtServer);
            this.panel1.Controls.Add(this.labPWD);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(337, 272);
            this.panel1.TabIndex = 13;
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(160, 300);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 14;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(253, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ConnectServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 335);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectServer";
            this.Text = "ConnectServer";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbWindowsUser;
        private System.Windows.Forms.RadioButton rbPWD;
        private System.Windows.Forms.Label labUserName;
        private System.Windows.Forms.Label labPWD;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPWD;
        private System.Windows.Forms.ComboBox cbDataBase;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.Button btnCancel;
    }
}