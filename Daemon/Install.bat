%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe  %~dp0\Daemon.exe
Net Start Daemon
sc config Daemon start= auto
pause