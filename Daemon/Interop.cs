using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Daemon
{
    /// <summary>
    /// http://www.cnblogs.com/gnielee/archive/2010/04/07/session0-isolation-part1.html
    /// </summary>
    public class Interop
    {
        public enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        public enum TOKEN_TYPE
        {
            TokenPrimary = 1,
            TokenImpersonation
        }

        public const int GENERIC_ALL_ACCESS = 0x10000000;

        public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        /// <summary>
        /// 弹出消息框
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">消息标题</param>
        public static void ShowMessageBox(string message, string title)
        {
            int resp = 0;
            WTSSendMessage(
                WTS_CURRENT_SERVER_HANDLE,
                WTSGetActiveConsoleSessionId(),
                title, title.Length,
                message, message.Length,
                0, 0, out resp, false);
        }

        /// <summary>
        /// 调用相应的应用程序启动进程，并且显示窗口
        /// </summary>
        /// <param name="app"></param>
        /// <param name="path"></param>
        public static void CreateProcess(string app, string path)
        {
            bool result;
            IntPtr hToken = WindowsIdentity.GetCurrent().Token;
            IntPtr hDupedToken = IntPtr.Zero;

            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            SECURITY_ATTRIBUTES sa = new SECURITY_ATTRIBUTES();
            sa.Length = Marshal.SizeOf(sa);

            STARTUPINFO si = new STARTUPINFO();
            si.cb = Marshal.SizeOf(si);

            int dwSessionID = WTSGetActiveConsoleSessionId();
            result = WTSQueryUserToken(dwSessionID, out hToken);

            if (!result)
            {
                Logger.Error("WTSQueryUserToken failed");
                return;
            }
            else
            {
                result = DuplicateTokenEx(
                      hToken,
                      GENERIC_ALL_ACCESS,
                      ref sa,
                      (int)SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
                      (int)TOKEN_TYPE.TokenPrimary,
                      ref hDupedToken
                   );

                if (!result)
                {
                    Logger.Error("DuplicateTokenEx failed");
                }
                else
                {
                    IntPtr lpEnvironment = IntPtr.Zero;
                    result = CreateEnvironmentBlock(out lpEnvironment, hDupedToken, false);

                    if (!result)
                    {
                        Logger.Error("CreateEnvironmentBlock failed");
                    }
                    else
                    {

                        var status = SetCurrentDirectory(path);//如不调用这个函数，则CreateProcessAsUser调用失败
                        if (status == 0)
                        {
                            Logger.Error("SetCurrentDirectory failed");
                        }
                        else
                        {
                            result = CreateProcessAsUser(
                                                 hDupedToken,
                                                 app,
                                                 String.Empty,
                                                 ref sa, ref sa,
                                                 false, 0, IntPtr.Zero,
                                                 path, ref si, ref pi);

                            if (!result)
                            {
                                int error = Marshal.GetLastWin32Error();
                                string message = String.Format("CreateProcessAsUser Error: {0}", error);
                                Logger.Error(message);
                            }
                        }
                    }
                }
            }
            if (pi.hProcess != IntPtr.Zero)
                CloseHandle(pi.hProcess);
            if (pi.hThread != IntPtr.Zero)
                CloseHandle(pi.hThread);
            if (hDupedToken != IntPtr.Zero)
                CloseHandle(hDupedToken);
        }

        #region 导入函数

        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 dwProcessID;
            public Int32 dwThreadID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public Int32 Length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [DllImport("kernel32.dll", SetLastError = true,
            CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", SetLastError = true,
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandle,
            Int32 dwCreationFlags,
            IntPtr lpEnvrionment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            ref PROCESS_INFORMATION lpProcessInformation);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            Int32 dwDesiredAccess,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            Int32 ImpersonationLevel,
            Int32 dwTokenType,
            ref IntPtr phNewToken);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSQueryUserToken(
            Int32 sessionId,
            out IntPtr Token);

        [DllImport("userenv.dll", SetLastError = true)]
        static extern bool CreateEnvironmentBlock(
            out IntPtr lpEnvironment,
            IntPtr hToken,
            bool bInherit);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WTSGetActiveConsoleSessionId();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SetCurrentDirectory(string lpPathName);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSSendMessage(
            IntPtr hServer,
            int SessionId,
            String pTitle,
            int TitleLength,
            String pMessage,
            int MessageLength,
            int Style,
            int Timeout,
            out int pResponse,
            bool bWait);

        #endregion
    }
}
