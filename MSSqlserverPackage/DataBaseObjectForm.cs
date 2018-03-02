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
                if (this.dbObject == null || string.IsNullOrEmpty(this.dbObject.TypeName))
                {
                    return "UnKnow";
                }
                else
                {
                    if (this.dbObject.TypeName.Trim().Equals("U"))
                    {
                        return "TABLE";
                    }
                    else if (this.dbObject.TypeName.Trim().Equals("V"))
                    {
                        return "VIEW";
                    }
                    else if (this.dbObject.TypeName.Trim().Equals("P"))
                    {
                        return "PROCEDURE";
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
