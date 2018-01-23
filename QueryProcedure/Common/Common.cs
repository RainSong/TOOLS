using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QueryProcedure
{
    public class Common
    {
        public static void ShowMessage(string msg, string type = "Info")
        {
            MessageBoxIcon icon;
            string caption;
            switch (type)
            {
                case "Waring":
                    icon = MessageBoxIcon.Warning;
                    caption = "提醒";
                    break;
                case "Error":
                    icon = MessageBoxIcon.Error;
                    caption = "错误";
                    break;
                default:
                    icon = MessageBoxIcon.Information;
                    caption = "信息";
                    break;
            }
            MessageBox.Show(msg, caption, MessageBoxButtons.OK, icon);
        }
    }
}
