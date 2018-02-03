using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace QueryTable
{
    public partial class LoginServer : Form
    {
        #region fields
        private string authType;
        #endregion

        #region constructor
        public LoginServer()
        {
            InitializeComponent();
            this.Text = "登录服务器";
            this.InitAuthTypeCombobox();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        #endregion

        #region events
        private void LoginServer_Load(object sender, EventArgs e)
        {
            ReadFile();
        }

        private void cboAuthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dynamic item = (dynamic)this.cboAuthType.SelectedItem;
            this.authType = item.Value;
            SetControlEnable();
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            this.btnConnect.Enabled = !string.IsNullOrEmpty(this.txtServer.Text.Trim());
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                try
                {
                    var conBuilder = BuildConnection();
                    var sqlComment = "select 1 as value from sys.databases";
                    var result = SqlHelper.ExecuteScalar<int>(conBuilder.ToString(), sqlComment);
                    if (result > 0)
                    {
                        WirteFle();
                        var mainFrom = new FormQueryTable(conBuilder);
                        mainFrom.LoginForm = this;
                        mainFrom.StartPosition = FormStartPosition.CenterScreen;
                        mainFrom.Show();
                        this.Hide();
                    }
                    else
                    {
                        Common.ShowMessage("连接数据库失败", "Waring");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("读取数据库信息失败", ex);
                    Common.ShowMessage(ex.Message, "Error");
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                try
                {
                    var conBuilder = BuildConnection();
                    var sqlComment = "select name as txt,name as value from sys.databases";
                    var dtDbs = SqlHelper.ExecuteQuery(conBuilder.ToString(), sqlComment);
                    this.cboDataBases.DataSource = dtDbs;
                    this.cboDataBases.DisplayMember = "text";
                    this.cboDataBases.ValueMember = "value";
                    Common.ShowMessage("连接成功");
                }
                catch (Exception ex)
                {
                    Logger.Error("读取数据库信息失败", ex);
                    Common.ShowMessage(ex.Message, "Error");
                }
            }
        }


        #region TextBox 控件的KeyDown事件
        private void txtServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.cboAuthType.Focus();
            }
        }

        private void txtUID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtPassword.SelectAll();
                this.txtPassword.Focus();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(this.cboDataBases.Text))
                {
                    this.btnTest.PerformClick();
                }
                else
                {
                    this.btnConnect.PerformClick();
                }
            }
        }
        #endregion

        #endregion

        #region methods

        private void InitAuthTypeCombobox()
        {
            this.cboAuthType.Items.Add(new { Text = "Windows 身份验证", Value = "NT" });
            this.cboAuthType.Items.Add(new { Text = "SQL Server 身份验证", Value = "SQLSERVER" });
            this.cboAuthType.DisplayMember = "Text";
            this.cboAuthType.ValueMember = "Value";
            this.cboAuthType.SelectedIndex = 0;
        }

        private void SetControlEnable()
        {
            var enable = string.Equals(this.authType, "SQLSERVER");
            this.lblUID.Enabled =
            this.lblPassword.Enabled =
            this.txtUID.Enabled =
                this.txtPassword.Enabled = enable;
        }

        private bool CheckInput()
        {
            if (string.Equals(this.authType, "SQLSERVER"))
            {
                if (string.IsNullOrEmpty(this.txtUID.Text.Trim()))
                {
                    Common.ShowMessage("请输入用户名", "Waring");
                    return false;
                }
                if (string.IsNullOrEmpty(this.txtPassword.Text.Trim()))
                {
                    Common.ShowMessage("请输入密码", "Waring");
                    return false;
                }
            }
            return true;
        }

        private SqlConnectionStringBuilder BuildConnection()
        {
            string dbName = "master";
            if (this.cboDataBases.SelectedIndex >= 0)
            {
                dbName = this.cboDataBases.Text;
            }

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = this.txtServer.Text.Trim(),
                InitialCatalog = dbName
            };
            var str = builder.ToString();
            if (string.Equals(authType, "SQLSERVER"))
            {
                builder.PersistSecurityInfo = true;
                builder.IntegratedSecurity = false;
                builder.UserID = this.txtUID.Text.Trim();
                builder.Password = this.txtPassword.Text.Trim();
            }
            else
            {
                builder.IntegratedSecurity = true;
                builder.PersistSecurityInfo = false;
            }
            return builder;
        }

        private void WirteFle()
        {
            var contentBuilder = new StringBuilder();
            contentBuilder.AppendFormat("SERVER={0}", this.txtServer.Text.Trim());
            if (string.Equals(this.authType, "SQLSERVER"))
            {
                contentBuilder.AppendLine();
                contentBuilder.AppendFormat("UID={0}", this.txtUID.Text.Trim());
                contentBuilder.AppendLine();
                contentBuilder.AppendFormat("IS=True");
            }

            var content = contentBuilder.ToString();
            var path = Path.Combine(Application.StartupPath, "login");
            var bytes = UTF8Encoding.UTF8.GetBytes(content);
            try
            {
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("保存登录信息到文件失败", ex);
            }
        }

        private void ReadFile()
        {
            var path = Path.Combine(Application.StartupPath, "login");
            if (!File.Exists(path)) return;
            string content = null;
            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    long size = fs.Length;
                    var bytes = new byte[size];
                    fs.Read(bytes, 0, (int)size);
                    content = UTF8Encoding.UTF8.GetString(bytes);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("读取上次的登录信息失败", ex);
                return;
            }
            if (string.IsNullOrEmpty(content)) return;
            var arr = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in arr)
            {
                var index = str.IndexOf("=");
                if (index < 0) continue;
                var value = str.Substring(index + 1);
                if (string.IsNullOrEmpty(value)) continue;
                if (str.StartsWith("SERVER"))
                {
                    this.txtServer.Text = value;
                }
                if (str.StartsWith("UID"))
                {
                    this.txtUID.Text = value;
                }
                if (str.StartsWith("IS"))
                {
                    if (string.Equals(value, "TRUE", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.cboAuthType.SelectedIndex = 1;
                    }
                }

            }
        }

        #endregion
    }
}
