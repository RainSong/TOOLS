using System;
using System.Runtime.Caching;
using System.Windows.Forms;
using MSSqlserverPackage.Common;

namespace MSSqlserverPackage
{
    public partial class ConnectServer : Form
    {
        private string LoginType = "NT";
        public string ConnectionString { get; private set; }
        public ConnectServer()
        {
            InitializeComponent();
            LoginTypeChanged();
#if DEBUG
            this.txtServer.Text = ".";
#endif
        }
        #region 事件
        private void rbWindowsUser_CheckedChanged(object sender, EventArgs e)
        {
            LoginTypeChanged();
            this.cbDataBase.DataSource = null;
        }
        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                BuildConnectionString();
                try
                {
                    BindDataToCombobox();
                }
                catch (Exception ex)
                {
                    MessageBoxEx.ShowError(this, ex.Message);
                    return;
                }
                MessageBoxEx.ShowMessage(this, "连接成功");
            }
        }
        private void btnSure_Click(object sender, EventArgs e)
        {
            if (CheckInput(true))
            {
                BuildConnectionString();
                CacheManager.SetValue("ConnectionString", this.ConnectionString);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion
        #region 方法
        private void LoginTypeChanged()
        {
            if (this.rbWindowsUser.Checked)
            {
                this.LoginType = "NT";

            }
            else
            {
                this.LoginType = "PWD";
            }
            this.labUserName.Enabled =
                this.labPWD.Enabled =
                this.txtUserName.Enabled =
                this.txtPWD.Enabled = this.rbPWD.Checked;
        }

        private bool CheckInput(bool checkDb = false)
        {
            var strServerName = this.txtServer.Text.Trim();
            if (string.IsNullOrEmpty(strServerName))
            {
                MessageBoxEx.ShowWarning(this, "请输入服务器名");
                return false;
            }
            if (LoginType.Equals("PWD"))
            {
                var strUserName = this.txtUserName.Text.Trim();
                var strPwd = this.txtPWD.Text.Trim();
                if (string.IsNullOrEmpty(strUserName))
                {
                    MessageBoxEx.ShowWarning(this, "请输入登录用户名");
                    return false;
                }
                if (string.IsNullOrEmpty(strPwd))
                {
                    MessageBoxEx.ShowWarning(this, "请输入登录密码");
                    return false;
                }
            }
            if (checkDb)
            {
                if (this.cbDataBase.SelectedItem == null)
                {
                    MessageBoxEx.ShowWarning(this, "请选择数据库");
                    return false;
                }
            }
            return true;
        }

        private void BuildConnectionString()
        {
            var dbName = this.cbDataBase.Text;

            if (string.IsNullOrEmpty(dbName))
            {
                dbName = "master";
            }
            if (this.LoginType.Equals("NT"))
            {
                this.ConnectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" + dbName + ";Data Source=" + this.txtServer.Text.Trim();
            }
            else
            {
                this.ConnectionString = "Password=" + this.txtPWD.Text.Trim() + ";Persist Security Info=True;User ID=" + this.txtUserName.Text.Trim() + ";Initial Catalog=" + dbName + ";Data Source=" + this.txtServer.Text.Trim();
            }
        }

        private void BindDataToCombobox()
        {
            var sql = "SELECT name,dbid FROM sysdatabases";
            var dtDataBases = RainSong.Common.SqlHelper.GetTable(this.ConnectionString, sql);
            this.cbDataBase.DataSource = dtDataBases;
            this.cbDataBase.DisplayMember = "name";
            this.cbDataBase.ValueMember = "dbid";
        }
        #endregion
    }
}
