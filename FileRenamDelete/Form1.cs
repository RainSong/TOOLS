using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FileRenamDelete
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private List<string> listMD5;

        #region
        private void btnBrower_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.SelectedPath = string.Empty;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = folderBrowserDialog1.SelectedPath;
                LoadFiles();
            }
        }

        private void btnTarBrower_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.SelectedPath = string.Empty;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtTargetPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        #endregion

        private void btnGenMD5_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.RowCount == 0)
            {
                MessageBox.Show("没有文件可生成MD5！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            GenMD5();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtPath.Text))
            {
                MessageBox.Show("没有文件可执行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void txtPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                LoadFiles();
            }
        }

        private void cbOnlyFile_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LoadFiles()
        {
            if (string.IsNullOrEmpty(this.txtPath.Text))
            {
                MessageBox.Show("请选择文件夹！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void GenMD5()
        {
            if (listMD5 == null)
            {
                listMD5 = new List<string>();
            }
            else
            {
                listMD5.Clear();
            }
            for (var i = 0; i < this.dataGridView1.RowCount; i++)
            {
                var fileName = (string)this.dataGridView1[0, 1].Value;
                if (File.Exists(fileName))
                {

                }
            }
        }

        private string GenMD5(string fileName) 
        {
            return string.Empty;
        }
    }
}
