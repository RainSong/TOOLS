using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace FileRenamDelete
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.toolStripStatusLabel1.Text = "请选择路径";
            this.toolStripProgressBar1.Visible = false;
        }
        private List<FilePathInfo> listFilePaths;
        private List<string> listMD5s;
        private long totalFileLength;
        //private string oldPath;

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

        private void DGVReload()
        {
            this.dataGridView1.DataSource = null;
        }

        private void LoadFiles()
        {
            if (string.IsNullOrEmpty(this.txtPath.Text))
            {
                MessageBox.Show("请选择文件夹！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!Directory.Exists(this.txtPath.Text))
            {
                MessageBox.Show(string.Format("{0}不是有效的路径！", this.txtPath.Text), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.toolStripStatusLabel1.Text = "正在加载文件信息...";
            //oldPath = this.txtPath.Text.Trim();
            if (listFilePaths == null) listFilePaths = new List<FilePathInfo>();
            else listFilePaths.Clear();
            LoadFiles(this.txtPath.Text);
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.listFilePaths;
            this.toolStripStatusLabel1.Text = "加载完成";
            this.totalFileLength = this.listFilePaths.Sum(o => o.Length);
        }

        private void LoadFiles(string path)
        {
            if (Directory.Exists(path))
            {
                DirectorySecurity ds = new DirectorySecurity(path, AccessControlSections.Access);
                if (ds.AreAccessRulesProtected) return;
                DirectoryInfo di = new DirectoryInfo(path);
                var dirs = di.GetDirectories();
                foreach (var dir in dirs)
                {
                    var pathInfo = new FilePathInfo
                    {
                        IsPath = true,
                        Name = dir.FullName
                    };
                    listFilePaths.Add(pathInfo);
                    LoadFiles(pathInfo.Name);
                }
                var files = di.GetFiles();
                foreach (var file in files)
                {
                    var fileInfo = new FilePathInfo
                    {
                        IsPath = false,
                        Name = file.FullName,
                        Length = file.Length,
                        Extension = file.Extension.Replace(".", "")
                    };
                    listFilePaths.Add(fileInfo);
                }
            }
        }

        private void GenMD5()
        {
            if (listMD5s == null)
            {
                listMD5s = new List<string>();
            }
            else
            {
                listMD5s.Clear();
            }
            foreach (var fpInfo in listFilePaths)
            {
                if (fpInfo.IsPath) continue;
                var md5 = GenMD5(fpInfo.Name);
                if (listMD5s.All(o => !o.Equals(md5)))
                {
                    listMD5s.Add(md5);
                }
                fpInfo.MD5 = md5;
            }
            this.cmbDelMD5.DataSource = listMD5s;

            this.dataGridView1.DataSource = listFilePaths;
        }

        private string GenMD5(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            int bufferSize = 1048576; // 缓冲区大小，1MB
            byte[] buff = new byte[bufferSize];

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.Initialize();

            long offset = 0;
            while (offset < fs.Length)
            {
                long readSize = bufferSize;
                if (offset + readSize > fs.Length)
                {
                    readSize = fs.Length - offset;
                }

                fs.Read(buff, 0, Convert.ToInt32(readSize)); // 读取一段数据到缓冲区

                if (offset + readSize < fs.Length) // 不是最后一块
                {
                    md5.TransformBlock(buff, 0, Convert.ToInt32(readSize), buff, 0);
                }
                else // 最后一块
                {
                    md5.TransformFinalBlock(buff, 0, Convert.ToInt32(readSize));
                }

                offset += bufferSize;
            }

            fs.Close();
            byte[] result = md5.Hash;
            md5.Clear();

            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
