using MSSqlserverPackage.Common;
using System;

namespace MSSqlserverPackage.Models
{
    public class DBObject
    {
        [Field("object_id")]
        public int ObjectID { get; set; }
        [Field("parent_object_id")]
        public int ParentID { get; set; }
        [Field("name")]
        public string Name { get; set; }
        [Field("schema_id")]
        public int SchemaID { get; set; }
        [Field("type")]
        public string ObjectType { get; set; }
        [Field("type_desc")]
        public string TypeDesc { get; set; }
        [Field("create_date")]
        public DateTime CreateDate { get; set; }
        [Field("modify_date")]
        public DateTime ModifyDate { get; set; }
        [Field("is_ms_shipped")]
        public bool IsMsShipped { get; set; }

        public Schema Schema { get; set; }
    }
}
