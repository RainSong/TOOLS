using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryDBObject.Models
{
    public class DBTable
    {
        public int ObjectID { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ObjectType { get; set; }
        public IEnumerable<DBColumn> Columns { get; set; }
    }
}
