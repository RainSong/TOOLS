using MSSqlserverPackage.Common;

namespace MSSqlserverPackage.Models
{
    public class Index : DBObject
    {
        [Field("index_id")]
        public int IndexID { get; set; }
        [Field("column_id")]
        public int ColumnID { get; set; }
        [Field("is_unique_constraint")]
        public bool IsUniqueConstraint { get; set; }
    }
}
