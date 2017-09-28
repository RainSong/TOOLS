using MSSqlserverPackage.Common;

namespace MSSqlserverPackage.Models
{
    public class ReferenceKey : Key
    {
        [Field("index_name")]
        public string IndexName { get; set; }
        [Field("referenced_object_id")]
        public int ReferenceObjectID { get; set; }
        public DBObject ReferenceObject { get; set; }
        [Field("referenced_column_id")]
        public int ReferenceColumnID { get; set; }
        public Column ReferenceColumn { get; set; }
    }
}
