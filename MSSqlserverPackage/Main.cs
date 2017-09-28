using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MSSqlserverPackage.Common;
using MSSqlserverPackage.Models;

namespace MSSqlserverPackage
{
    public partial class Main : Form
    {
        #region 字段
        /// <summary>
        /// 设置的数据库链接字符串
        /// </summary>
        private string connectionString = string.Empty;
        /// <summary>
        /// 表节点
        /// </summary>
        private TreeNode tnTables;
        /// <summary>
        /// 视图节点
        /// </summary>
        private TreeNode tnViews;

        private LanuageContent lc;
        #endregion
        #region 事件
        /// <summary>
        /// 菜单选项“链接”的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemFileConnect_Click(object sender, EventArgs e)
        {
            var connectForm = new ConnectServer();
            connectForm.StartPosition = FormStartPosition.CenterParent;
            if (DialogResult.OK == connectForm.ShowDialog())
            {
                BindInfoToTree();
                InitObjectsForm();
            }
        }

        /// <summary>
        /// 树的鼠标按键叹气事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var checkNode = this.treeView1.GetNodeAt(e.Location);
                if (checkNode != null)
                {
                    this.treeView1.SelectedNode = checkNode;
                    contextMenuStrip1.Show(this.treeView1, e.Location);
                }
            }
        }

        private void MenuItemExportDesignToExcel_Click(object sender, EventArgs e)
        {

        }

        private void MenuItemExportDesignToSqlScript_Click(object sender, EventArgs e)
        {

        }

        #region 右键菜单事件
        private void ToolStripMenuItemData_Click(object sender, EventArgs e)
        {
            var checkedNode = this.treeView1.SelectedNode;
            if (checkedNode != null && checkedNode.Tag != null)
            {
                ShowForm<DataResultForm>((DBObject)checkedNode.Tag);
            }
        }

        private void ToolStripMenuItemScript_Click(object sender, EventArgs e)
        {
            var checkedNode = this.treeView1.SelectedNode;
            if (checkedNode != null && checkedNode.Tag != null)
            {
                ShowForm<ScriptForm>((DBObject)checkedNode.Tag);
            }
            
        }

        private void ToolStripMenuItemDesign_Click(object sender, System.EventArgs e)
        {
            var checkedNode = this.treeView1.SelectedNode;
            if (checkedNode != null && checkedNode.Tag != null)
            {
                ShowForm<DesignForm>((DBObject)checkedNode.Tag);
            }
        }

        #endregion
        #endregion
        #region 方法
        public Main()
        {
            lc = LanguageLoader.GetLanguage(AppContext.BaseDirectory + "\\languages\\ChineseSimplified.xml");
            InitializeComponent();
            InitDefaultNodes();
            this.StartPosition = FormStartPosition.CenterScreen;



            //ObjectCache cache = MemoryCache.Default;
            //lc = cache["LanuageConfig"] as LanuageContent;
            this.ToolStripMenuItemData.Click += this.ToolStripMenuItemData_Click;
            this.ToolStripMenuItemDesign.Click += this.ToolStripMenuItemDesign_Click;
            this.ToolStripMenuItemScript.Click += this.ToolStripMenuItemScript_Click;

            this.MenuItemExportDesignToExcel.Click += this.MenuItemExportDesignToExcel_Click;
            this.MenuItemExportDesignToSqlScript.Click += this.MenuItemExportDesignToSqlScript_Click;
        }
        /// <summary>
        /// 初始化树种默认的节点
        /// </summary>
        private void InitDefaultNodes()
        {
            tnTables = new TreeNode
            {
                Name = "TreeNodeTables",
                Text = lc.MainFormTreeNodeTable
            };
            tnViews = new TreeNode
            {
                Name = "TreeNodeViews",
                Text = lc.MainFormTreeNodeView
            };
            this.treeView1.Nodes.AddRange(new TreeNode[]
            {
                tnTables,
                tnViews
            });
        }
        /// <summary>
        /// 查询数据库对象的方法
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataBaseObjectsInfos()
        {
            var sql = "SELECT name,id,type FROM sysobjects WHERE type in ('u','v')";
            try
            {
                return SqlHelper.GetTable(connectionString, sql);
            }
            catch
            {
                MessageBoxEx.ShowError(this, lc.MainFormErrorMessageQueryDataBaseObjectInfo);
                return null;
            }
        }
        /// <summary>
        /// 将数据库对象绑定到树上
        /// </summary>
        /// <param name="dt"></param>
        private void BindInfoToTree()
        {
            var tables = DataService.GetTables();
            BindObjectInfosToTreeNode(tables, this.tnTables);
            var view = DataService.GetViews();
            BindObjectInfosToTreeNode(view, this.tnViews);
        }
        /// <summary>
        /// 将一些数据库对象绑定到某个树的节点上
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="node"></param>
        private void BindObjectInfosToTreeNode(IEnumerable<DBObject> objects, TreeNode node)
        {
            foreach (DBObject obj in objects)
            {
                var childNode = new TreeNode
                {
                    Text = (obj.Schema == null ? string.Empty : (obj.Schema.Name + ".")) + obj.Name,
                    Name = obj.Name,
                    Tag = obj
                };
                node.Nodes.Add(childNode);
            }
        }

        public void ShowForm<T>(DBObject dbObject)
            where T : DataBaseObjectForm, new()
        {
            var tabPage = new TabPage();

            var form =  new T();
            form.dbObject = dbObject;
            form.TopLevel = false;
            form.Parent = tabPage;
            form.ControlBox = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;

            tabPage.Text = form.Text;
            tabPage.Controls.Add(form);
            this.tabControl1.TabPages.Add(tabPage);

            form.Show();
        }

        private void InitObjectsForm()
        {
            var tabPage = new TabPage();
            var form = new ObjectsForm();
            form.TopLevel = false;
            form.Parent = this;
            form.ControlBox = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;

            tabPage.Controls.Add(form);
            tabPage.Text = form.Text;
            this.tabControl1.TabPages.Add(tabPage);
            form.Show();
        }
        #endregion
    }
}
