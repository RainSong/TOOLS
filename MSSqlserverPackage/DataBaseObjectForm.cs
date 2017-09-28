using MSSqlserverPackage.Models;
using System;
using System.Runtime.Caching;
using System.Windows.Forms;

namespace MSSqlserverPackage
{
    public class DataBaseObjectForm : Form
    {
        public DBObject dbObject { get; set; }
        public string ObjectType
        {
            get
            {
                if (string.IsNullOrEmpty(this.dbObject.ObjectType))
                {
                    return "UnKnow";
                }
                else
                {
                    if (this.dbObject.ObjectType.Trim().Equals("U"))
                    {
                        return "TABLE";
                    }
                    else if (this.dbObject.ObjectType.Trim().Equals("V"))
                    {
                        return "VIEW";
                    }
                    return "OTHER";
                }
            }
        }
        protected string connectionString = string.Empty;

        public DataBaseObjectForm()
        {
            ObjectCache cache = MemoryCache.Default;
            this.connectionString = cache["ConnectionString"] as string;
        }
    }
}
