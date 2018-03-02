using System.Collections.Generic;

namespace MSSqlserverPackage.Models
{
    public class Table : DBObject, IQueryableObject
    {
        public IEnumerable<Column> Columns { get; set; }
        public PrimaryKey PrimaryKey { get; set; }
        public IEnumerable<ReferenceKey> ReferenceKeys { get; set; }
        public IEnumerable<Index> Indexes { get; set; }
    }
}
