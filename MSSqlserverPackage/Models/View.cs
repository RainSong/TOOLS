using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSqlserverPackage.Models
{
    public class View : DBObject
    {
        public IEnumerable<Column> Columns { get; set; }
        public IEnumerable<Index> Indexes { get; set; }
    }
}
