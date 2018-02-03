using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QueryDBObject
{
    public partial class Main : Form
    {
        public LoginServer LoginForm { get; set; }
        private SqlConnectionStringBuilder sqlConnectionStringBuilder;
        public Main(SqlConnectionStringBuilder sqlConnectionStringBuilder)
        {
            InitializeComponent();
            this.sqlConnectionStringBuilder = sqlConnectionStringBuilder;

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
            var qt = new FormQueryTable(this.sqlConnectionStringBuilder);
            qt.FormBorderStyle = FormBorderStyle.None;
            qt.Dock = DockStyle.Fill;
            qt.TopLevel = false;
            tp.Controls.Add(qt);
            this.tabControl1.TabPages.Add(tp);
            qt.Show();

        }

        private void Main_Load(object sender, EventArgs e)
        {
            //this.tpQueryProcedure.Text = "查询存储过程";
            //this.tpQueryTable.Text = "查询表";
        }

        private void InitForm()
        {
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
