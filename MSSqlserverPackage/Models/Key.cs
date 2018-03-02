using MSSqlserverPackage.Common;
using RainSong.Common;

namespace MSSqlserverPackage.Models
{
    public class Key
    {
        [Field("key_name")]
        public string Name { get; set; }
        [Field("column_id")]
        public int ColumnID { get; set; }
        [Field("object_id")]
        public int ObjectID { get; set; }
    }
}
