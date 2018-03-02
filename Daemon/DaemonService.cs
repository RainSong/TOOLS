using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace Daemon
{
    public partial class DaemonService : ServiceBase
    {

        private int period = 60 * 1000;
        private string targetPath;
        private string fileName;

        Timer timer = null;
        public DaemonService()
        {
            InitializeComponent();
            try
            {
                LoadConfig();
            }
            catch (Exception ex)
            {
                Logger.Error("读取配置文件失败", ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {

                Logger.Info("服务已启动");
                this.StartWatch();
            }
            catch (Exception ex)
            {
                Logger.Error("服务启动失败", ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (timer != null)
                {
                    timer.Dispose();
                }
                Logger.Info("服务已停止");
            }
            catch { }

        }

        /// <summary>
        /// 开始监控
        /// </summary>
        private void StartWatch()
        {
            if (!string.IsNullOrEmpty(this.targetPath))
            {
                if (File.Exists(targetPath))
                {
                    var autoEvent = new AutoResetEvent(false);
                    var watcher = new ProcessWatcher(this.targetPath + this.fileName);
                    timer = new Timer(watcher.ScanProdess, autoEvent, 0, this.period);
                    //this.ScanProcessList(this.targetPath);
                }
                else
                {
                    Logger.Error("目标文件不存在");

                }
            }
            else
            {
                Logger.Error("目标文件路径为空");
            }
        }

        private void LoadConfig()
        {
            string[] lines = null;
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "watch.txt");
                lines = File.ReadAllLines(path);
            }
            catch (Exception ex)
            {
                Logger.Error("读取配置文件失败", ex);
            }
            if (lines != null)
            {
                if (lines.Length > 0)
                {
                    int period = 0;
                    if (int.TryParse(lines[0], out period))
                    {
                        this.period = period;
                    }
                    if (lines.Length > 1)
                    {
                        this.targetPath = lines[1];
                    }
                }
            }
        }

    }

    public class ProcessWatcher
    {
        private string path;
        public ProcessWatcher(string path)
        {
            this.path = path;
        }
        public void ScanProdess(Object stateInfo)
        {

            Logger.Info("扫描一次：" + (path ?? "Empty").ToString());
            //var processPath = objPath.ToString();
            var continueProcessNames = new string[] { "System", "Idle", "audiodg", "SearchFilterHost" };
            Process[] arrProcess = Process.GetProcesses();
            var query = from p in arrProcess
                        where !continueProcessNames.Any(o => string.Equals(o, p.ProcessName, StringComparison.CurrentCultureIgnoreCase))
                        select p;
            query = query.Where(o => string.Equals(o.MainModule.FileName, this.path, StringComparison.CurrentCultureIgnoreCase));
            if (query.Any()) return;
            //进程尚未启动
            try
            {
                var index = this.path.LastIndexOf("\\");
                if (index > 0)
                {
                    var fileName = this.path.Substring(index + 1);
                    var dir = this.path.Substring(0, index + 1);

                    Logger.Info(string.Format("path:{0},file:{1}", dir, fileName));

                    Interop.CreateProcess(fileName, dir);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("启动程序失败", ex);
            }
        }

    }
}
