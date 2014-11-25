using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRenamDelete
{
    public class FilePathInfo
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Size
        {
            get
            {
                if (this.IsPath) return string.Empty;
                else 
                {
                    if (this.Length < 1024)
                    {
                        return "1KB";
                    }
                    else if (this.Length > 1024 && this.Length < (1024 * 1024))
                    {
                        return string.Format("{0}KB", this.Length / 1024);
                    }
                    else if (this.Length > 1024 * 1024 && this.Length < (1024 * 1024 * 1024))
                    {
                        return string.Format("{0}MB", this.Length / (1024 * 1024));
                    }
                    else
                    {
                        return string.Format("{0}GB", this.Length / (1024 * 1024 * 1024));
                    }
                }
            }
        }
        public long Length { get; set; }
        public bool IsPath { get; set; }
        public string MD5 { get; set; }
    }
}
