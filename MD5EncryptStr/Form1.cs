using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MD5EncryptStr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            List<string> listIn = Read();
            List<string> listOut = Encrypt(listIn);
            string strOut = string.Join("\r\n", listOut.ToArray());
            this.txtOut.Text = strOut;
        }

        private List<string> Read()
        {
            string strIn = this.txtIn.Text.Trim();
            if (string.IsNullOrEmpty(strIn)) return null;
            List<string> list = new List<string>();
            list.AddRange(strIn.Split('\n'));
            return list;
        }

        private List<string> Encrypt(List<string> listIn)
        {
            List<string> listOut = new List<string>();
            foreach (string str in listIn)
            {
                string strTemp = str.Trim();
                if (!string.IsNullOrEmpty(strTemp) && !strTemp.Equals("\n") && !strTemp.Equals("\r"))
                {
                    strTemp = strTemp.Replace("\r", "");
                    strTemp = Encrypt(strTemp);
                }
                listOut.Add(strTemp);
            }
            return listOut;
        }
        private string Encrypt(string str)
        {
            for (int i = 0; i < 5; i++)
            {
                str = MD5E(str);
            }
            return str;
        }
        public static string MD5E(String str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(str);
            byte[] result = md5.ComputeHash(data);
            String ret = "";
            for (int i = 0; i < result.Length; i++)
            {
                ret += result[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }
    }
}
