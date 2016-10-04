using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace EncodeDecodeUrl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            var text = (this.txtOld.Text ?? string.Empty).Trim();
            if (CheckInput(text))
            {
                try
                {
                    if (this.rbUseServer.Checked)
                    {
                        text = HttpContext.Current.Server.UrlEncode(text);
                    }
                    else
                    {
                        text = HttpUtility.UrlEncode(text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发生错误，编码失败！\r\n错误原因：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.txtNew.Text = text;
            }
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            var text = (this.txtOld.Text ?? string.Empty).Trim();
            if (CheckInput(text))
            {
                try
                {
                    if (this.rbUseServer.Checked)
                    {
                        text = HttpContext.Current.Server.UrlDecode(text);
                    }
                    else
                    {
                        text = HttpUtility.UrlDecode(text);
                    }
                    this.txtNew.Text = text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发生错误，解码失败！\r\n错误原因：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool CheckInput(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("请输入要编码/解码的内容！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
