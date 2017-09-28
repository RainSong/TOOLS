using MSSqlserverPackage.Common;
using System.Collections.Generic;

namespace MSSqlserverPackage.Models
{
    public class Column
    {
        [Field("column_id")]
        public int ID { get; set; }
        [Field("name")]
        public string Name { get; set; }
        [Field("data_type")]
        public string DataType { get; set; }
        [Field("max_length")]
        public short MaxLength { get; set; }
        [Field("is_nullable")]
        public bool Nullable { get; set; }
        [Field("object_id")]
        public int ObjectID { get; set; }

        public Identity Identity { get; set; }
        public IEnumerable<ExtendedProperty> ExtendedProperties { get; set; }
    }
}
