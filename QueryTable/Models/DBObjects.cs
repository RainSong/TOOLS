using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryTable.Models
{
    public class DBObject
    {
        public int ObjectID { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ObjectType { get; set; }
        public IEnumerable<DBColumn> Columns { get; set; }
    }
}
