using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QueryTable
{
    public partial class FindWindow : Form
    {
        public List<int> Indexes;

        public string Content { get; set; }
        public string Target { get; private set; }
        private bool Searched = false;
        private int nextIndex = 0;

        public event FindNexEventHandle FindeNext;
        #region 窗口闪烁
        [DllImport("user32.dll")] //闪烁窗体
        public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        //闪烁窗体参数
        public const UInt32 FLASHW_STOP = 0;//停止闪动.系统将窗体恢复到初始状态.
        public const UInt32 FLASHW_CAPTION = 1;//闪动窗体的标题.
        public const UInt32 FLASHW_TRAY = 2;//闪动任务栏按钮
        public const UInt32 FLASHW_ALL = 3;//闪动窗体标题和任务栏按钮
        public const UInt32 FLASHW_TIMER = 4;//连续不停的闪动,直到此参数被设置为:FLASHW_STOP
        public const UInt32 FLASHW_TIMERNOFG = 12;//连续不停的闪动,直到窗体用户被激活.通常用法将参数设置为
        // <summary>
        /// 窗口闪烁
        /// </summary>
        /// <param name="handle">窗口句柄</param>
        public static void FlashWindow(IntPtr handle)
        {
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = handle;
            fInfo.dwFlags = FLASHW_CAPTION;
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;

            FlashWindowEx(ref fInfo);
        }
        #endregion

        public FindWindow(string target, string content)
        {
            InitializeComponent();
            this.Target = target;
            this.Content = content;

            this.Indexes = new List<int>();
        }

        public void GetCountAndIndex()
        {
            if (string.IsNullOrEmpty(Content) || string.IsNullOrEmpty(Target)) return;
            //if (this.cbIgonreCase.Checked)
            //{
            //    this.Count = Regex.Match(this.Content, this.Target, RegexOptions.IgnoreCase).Length;
            //}
            //else
            //{
            //    this.Count = Regex.Match(this.Content, this.Target, RegexOptions.IgnoreCase).Length;
            //}
            for (int i = 0; i < this.Content.Length;)
            {
                if (char.Equals(this.Content[i], this.Target[0]) && string.Equals(this.Content.Substring(i, this.Target.Length), this.Target))
                {
                    this.Indexes.Add(i);
                    i += this.Target.Length;
                }
                else
                {
                    i++;
                }
            }
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            var newTarget = this.txtTarget.Text.Trim();
            if (string.IsNullOrEmpty(newTarget)) return;
            if (!string.Equals(this.Target, newTarget))
            {
                this.Target = newTarget;
                this.Searched = false;
                this.nextIndex = 0;
            }
            if (!Searched)
            {
                GetCountAndIndex();
                Searched = true;
            }
            if (this.Indexes.Any())
            {
                if (nextIndex < this.Indexes.Count && this.FindeNext != null)
                {
                    FindeNext(null, new FindNextEventArgs
                    {
                        NextIndex = this.Indexes[nextIndex],
                        SearchTarget = this.Target
                    });
                    nextIndex += 1;
                }
                if (nextIndex == this.Indexes.Count)
                {
                    MessageBox.Show("已到最后");
                    nextIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("没有匹配的内容");
            }
        }

        private void FindWindow_Leave(object sender, EventArgs e)
        {
            this.Opacity = 0.35;
        }
    }

    public class FindNextEventArgs : EventArgs
    {
        public int NextIndex { get; set; }
        public string SearchTarget { get; set; }
    }

    public delegate void FindNexEventHandle(object sender, FindNextEventArgs e);

}
