using MSSqlserverPackage.Common;

namespace MSSqlserverPackage.Models
{
    public class Identity
    {
        [Field("object_id")]
        public int ObjectID { get; set; }
        [Field("column_id")]
        public int ColumnID { get; set; }
        [Field("is_identity")]
        public bool IsIdentity { get; set; }
        [Field("seed_value")]
        public int SeedValue { get; set; }
        [Field("increment_value")]
        public int IncrementValue { get; set; }
    }
}
