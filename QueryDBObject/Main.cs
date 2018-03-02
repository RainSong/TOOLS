using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QueryDBObject
{
    public partial class Main : Form
    {
        public LoginServer LoginForm { get; set; }
        public int DataBaseID { get; set; }
        private SqlConnectionStringBuilder sqlConnectionStringBuilder;
        public Main(SqlConnectionStringBuilder sqlConnectionStringBuilder)
        {
            InitializeComponent();
            this.sqlConnectionStringBuilder = sqlConnectionStringBuilder;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            InitForm();
        }

        private void InitForm()
        {
            var tp = new TabPage
            {
                Name = "tpQP",
                Text = "查询存储过程"
            };
            var qp = new FormQueryProcedure(this.sqlConnectionStringBuilder);
            qp.FormBorderStyle = FormBorderStyle.None;
            qp.Dock = DockStyle.Fill;
            qp.TopLevel = false;
            qp.Parent = tp;
            tp.Controls.Add(qp);
            this.tabControl1.TabPages.Add(tp);
            qp.Show();

            tp = new TabPage
            {
                Name = "tpT",
                Text = "查询表/视图"
            };
            var qt = new FormQueryTable(this.sqlConnectionStringBuilder)
            {
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                TopLevel = false,
                DataBaseID = this.DataBaseID
            };
            tp.Controls.Add(qt);
            this.tabControl1.TabPages.Add(tp);
            qt.Show();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            var dialogResult = MessageBox.Show("是否返回重新连接服务器？\r\n是：重新连接，否：退出程序！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {

                if (this.LoginForm != null)
                {
                    this.LoginForm.Show();
                }
                else
                {
                    var lf = new LoginServer();
                    lf.StartPosition = FormStartPosition.CenterScreen;
                    lf.Show();
                    this.Close();
                }
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
